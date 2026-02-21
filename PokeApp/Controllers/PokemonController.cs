using InjectedServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PokeApp.Models;
using Models.Pokemon;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace PokeApp.Controllers
{
    /// <summary>
    /// Controlador para mostrar la lista de Pokémon. Utiliza el servicio IPokeService para obtener los datos de la API de Pokémon.
    /// </summary>
    public class PokemonController : Controller
    {
        private readonly IPokeService _pokeService;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;


        public PokemonController(IPokeService pokeService, IMemoryCache cache, IConfiguration configuration)
        {
            _pokeService = pokeService;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string? search = null, string? species = null)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                try
                {
                    var stats = await _pokeService.GetPokemonByName(search.Trim().ToLower());

                    var result = new PokemonListItem
                    {
                        name = stats.name,
                        url = $"https://pokeapi.co/api/v2/pokemon/{stats.id}/",
                        SpriteUrl = stats.sprites?.front_default ?? string.Empty,
                        Species = stats.species?.name ?? string.Empty
                    };

                    return View(new PokemonIndex
                    {
                        Pokemons = new[] { result },
                        Total = 1,
                        Page = 1,
                        PageSize = 1,
                        Search = search
                    });
                }
                catch   // PokeAPI devuelve 404 si el nombre no existe
                {
                    return View(new PokemonIndex
                    {
                        Pokemons = Array.Empty<PokemonListItem>(),
                        Search   = search   // para mostrar el mensaje de "no encontrado"
                    });
                }
            }

            int[] allowedSizes = { 10, 20, 50, 100 };
            if (!allowedSizes.Contains(pageSize)) pageSize = 20;

            int offset = (page - 1) * pageSize;
            var list   = await _pokeService.GetPokemon(offset, pageSize);
            var items = list?.results ?? new List<PokemonListItem>();

            // Obtener detalles de todos en paralelo
            var detailTasks = items.Select(async p =>
            {
                var stats = await _pokeService.GetPokemonById(p.Id);
                p.SpriteUrl = stats?.sprites?.front_default ?? string.Empty;
                p.Species = stats?.species?.name ?? string.Empty;
                return p;
            });

            var enrichedItems = await Task.WhenAll(detailTasks);

            if (!string.IsNullOrWhiteSpace(species))
            {
                enrichedItems = enrichedItems
                    .Where(p => p.Species.Equals(species, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            var vm = new PokemonIndex
            {
                Pokemons = enrichedItems,
                Total = list?.count ?? 0,
                Page = page,
                PageSize = pageSize,
                Species  = species
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecies(string q = "")
        {
            // Intentar obtener del caché
            if (!_cache.TryGetValue("all_species", out List<string>? allSpecies))
            {
                // Primera vez: llamar a la API y cachear por 24 horas
                var result = await _pokeService.SearchSpecies();
                allSpecies = result.results.Select(s => s.name).ToList();

                _cache.Set("all_species", allSpecies, TimeSpan.FromHours(24));
            }

            // Filtrar localmente por el término de búsqueda
            var filtered = string.IsNullOrWhiteSpace(q)
                ? allSpecies!.Take(10).ToList()
                : allSpecies!.Where(s => s.Contains(q.ToLower().Trim()))
                            .Take(10)
                            .ToList();

            return Json(filtered);
        }

        [HttpGet]
        public async Task<IActionResult> BySpecies(string name)
        {
            var speciesDetail = await _pokeService.GetSpeciesByName(name);

            // Extraer los IDs de las variedades
            var ids = speciesDetail.varieties
                .Select(v => v.pokemon.Id)
                .ToList();

            // Consultar cada pokémon en paralelo
            var detailTasks = ids.Select(async id =>
            {
                var stats = await _pokeService.GetPokemonById(id);
                return new PokemonListItem
                {
                    name = stats.name,
                    url = $"https://pokeapi.co/api/v2/pokemon/{stats.id}/",
                    SpriteUrl = stats.sprites?.front_default ?? string.Empty,
                    Species = stats.species?.name ?? string.Empty
                };
            });

            var pokemons = await Task.WhenAll(detailTasks);

            var vm = new PokemonIndex
            {
                Pokemons = pokemons,
                Total = pokemons.Length,
                Page = 1,
                PageSize = pokemons.Length,
                Species = name
            };

            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarExcel([FromBody] EnviarExcelRequest request)
        {
            try
            {
                var smtpConfig = _configuration.GetSection("Smtp");

                // Convertir base64 a bytes
                var bytes = Convert.FromBase64String(request.FileBase64);

                // Construir el correo
                var mensaje = new MimeMessage();
                mensaje.From.Add(MailboxAddress.Parse(smtpConfig["From"]));
                mensaje.To.Add(MailboxAddress.Parse(smtpConfig["To"]));
                mensaje.Subject = "Pokédex - Exportación Excel";

                var builder = new BodyBuilder
                {
                    TextBody = "Se adjunta la tabla de la Pokédex exportada desde la aplicación."
                };
                builder.Attachments.Add(request.FileName, bytes,
                    ContentType.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

                mensaje.Body = builder.ToMessageBody();

                // Enviar
                using var client = new SmtpClient();
                await client.ConnectAsync(smtpConfig["Host"],
                                        int.Parse(smtpConfig["Port"]!),
                                        SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpConfig["User"], smtpConfig["Password"]);
                await client.SendAsync(mensaje);
                await client.DisconnectAsync(true);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    error = ex.Message,
                    detail = ex.ToString()  // stack trace completo
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStats(int id)
        {
            var stats = await _pokeService.GetPokemonById(id);

            return Json(new {
                name    = stats.name,
                sprite  = stats.sprites?.front_default,
                types   = stats.types.Select(t => t.type.name).ToList(),
                stats   = stats.stats.Select(s => new {
                    name  = s.stat.name,
                    value = s.base_stat
                }).ToList()
            });
        }
    }
}
