using Models.Pokemon;

namespace InjectedServices.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de Pokémon, que define los métodos para interactuar con la API de Pokémon.
    /// </summary>
    public interface IPokeService
    {
        /// <summary>
        /// Método para obtener la lista de Pokémon desde la API de Pokémon.
        /// </summary>
        /// <param name="Ioffset"></param>
        /// <param name="Ilimit"></param>
        /// <returns></returns>
        Task<PokemonList> GetPokemon(int Ioffset, int Ilimit);
        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su ID desde la API de Pokémon.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PokemonStats> GetPokemonById(int id);
        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su nombre desde la API de Pokémon.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<PokemonStats> GetPokemonByName(string name);
    }
}
