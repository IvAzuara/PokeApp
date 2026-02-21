namespace Models.Pokemon
{
    /// <summary>
    /// Clase que representa la respuesta de la API de Pokémon para la lista de Pokémon, que incluye el conteo total, los enlaces a la siguiente y anterior página, y la lista de resultados con los Pokémon.
    /// </summary>
    public class PokemonList
    {
        public int count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<PokemonListItem> results { get; set; } = new();
    }
}
