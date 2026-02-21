using System;

namespace Models.Pokemon;

public class PokemonStats
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public Sprites sprites { get; set; } = new();
    public Species species { get; set; } = new();
    public List<PokemonStat> stats { get; set; } = new();
    public List<PokemonType> types { get; set; } = new();
}

/// <summary>
/// Clases auxiliares para representar los sprites y la especie de un Pokémon
/// </summary>
public class Sprites
{
    public string front_default { get; set; } = string.Empty;
}

public class Species
{
    public string name { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
}

/// <summary>
/// Clases para mostrar las estadisticas en un popup
/// </summary>
public class PokemonStat
{
    public int base_stat { get; set; }
    public StatInfo stat { get; set; } = new();
}

public class StatInfo
{
    public string name { get; set; } = string.Empty;
}

public class PokemonType
{
    public TypeInfo type { get; set; } = new();
}

public class TypeInfo
{
    public string name { get; set; } = string.Empty;
}
