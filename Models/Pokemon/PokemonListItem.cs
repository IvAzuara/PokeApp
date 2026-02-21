namespace Models.Pokemon
{
    /// <summary>
    /// Clase que representa un Pokémon en la lista de resultados de la API.
    /// </summary>
    public class PokemonListItem
    {
        public string name { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public int Id => int.Parse(url.TrimEnd('/').Split('/').Last());

        // Se llenan desde GetPokemonById
        public string SpriteUrl { get; set; } = string.Empty;
        public string Species   { get; set; } = string.Empty;
    }
}
