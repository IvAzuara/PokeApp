namespace Models.Pokemon
{
    public class PokemonList
    {
        public int count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<PokemonListItem> results { get; set; } = new();
    }
}
