using System;

namespace Models.Pokemon;

public class PokemonStats
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public Sprites sprites { get; set; } = new();
    public Species species { get; set; } = new();
}

/// <summary>
/// Clases auxiliares para representar los sprites y la especie de un Pokémon. Estas clases se utilizan dentro de PokemonStats para organizar mejor la información obtenida de la API de Pokémon.
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