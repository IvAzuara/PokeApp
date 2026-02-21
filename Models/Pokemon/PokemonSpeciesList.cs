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
}
