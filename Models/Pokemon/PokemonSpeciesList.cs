using System;

namespace Models.Pokemon;

public class PokemonSpeciesList
{
    public int count { get; set; }
    public List<SpeciesItem> results { get; set; } = new();
}

public class SpeciesItem
{
    public string name { get; set; } = string.Empty;
    public string url  { get; set; } = string.Empty;
    public int Id => int.Parse(url.TrimEnd('/').Split('/').Last());
}

public class PokemonSpeciesDetail
{
    public string name { get; set; } = string.Empty;
    public List<PokemonVariety> varieties { get; set; } = new();
}

public class PokemonVariety
{
    public bool is_default { get; set; }
    public SpeciesItem pokemon { get; set; } = new();
}
