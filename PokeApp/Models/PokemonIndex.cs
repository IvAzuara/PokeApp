using Models.Pokemon;

namespace PokeApp.Models;
/// <summary>
/// ViewModel para la vista de índice de Pokémon. Contiene la lista de Pokémon, información de paginación y estados de los botones de navegación.
/// </summary>
public class PokemonIndex
{
    public IEnumerable<PokemonListItem> Pokemons { get; set; } = Array.Empty<PokemonListItem>();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
    public int WindowSize { get; set; } = 5;
    public string? Species { get; set; }
    public IEnumerable<string> AvailableSpecies =>
        Pokemons.Select(p => p.Species)
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct()
                .OrderBy(s => s);


    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

    // Variables para la paginación
    public int WindowStart => Math.Max(1, Page - WindowSize);
    public int WindowEnd => Math.Min(TotalPages, Page + WindowSize);
    public int[] PageSizeOptions { get; set; } = { 10, 20, 50, 100 };

    // Estados de los botones
    public bool IsFirstPage => Page == 1;
    public bool IsLastPage => Page == TotalPages;

    // Mostrar los 3 puntitos de la paginación
    public bool ShowStartEllipsis => WindowStart > 2;
    public bool ShowEndEllipsis => WindowEnd < TotalPages - 1;
    public bool ShowFirstPage => WindowStart > 1;
    public bool ShowLastPage => WindowEnd < TotalPages;
}