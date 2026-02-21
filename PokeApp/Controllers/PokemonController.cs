using InjectedServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using PokeApp.Models;
using Models.Pokemon;

namespace PokeApp.Controllers
{
    /// <summary>
    /// Controlador para mostrar la lista de Pokémon. Utiliza el servicio IPokeService para obtener los datos de la API de Pokémon.
    /// </summary>
    public class PokemonController : Controller
    {
        private readonly IPokeService _pokeService;

        public PokemonController(IPokeService pokeService)
        {
            _pokeService = pokeService;
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
    }
}
