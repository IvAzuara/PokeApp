using Models.Pokemon;
using Refit;

namespace InjectedServices.Interfaces.Refit
{
    /// <summary>
    /// Interfaz para la API de Pokémon, que define los métodos para interactuar con la API de Pokémon utilizando Refit.
    /// </summary>
    public interface IPokeAPI
    {
        /// <summary>
        /// Método para obtener la lista de Pokémon desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP.
        /// </summary>
        /// <param name="Ioffset"></param>
        /// <param name="Ilimit"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon?offset={Ioffset}&limit={Ilimit}")]
        Task<ApiResponse<PokemonList>> GetPokemon([AliasAs("Ioffset")] int Ioffset, [AliasAs("Ilimit")] int Ilimit);

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su ID desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon/{id}")]
        Task<ApiResponse<PokemonStats>> GetPokemonById(int id);

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su nombre desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon/{name}")]
        Task<ApiResponse<PokemonStats>> GetPokemonByName(string name);
    }
}
