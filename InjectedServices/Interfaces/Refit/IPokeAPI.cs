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
        /// Método para obtener la lista de Pokémon
        /// </summary>
        /// <param name="Ioffset"></param>
        /// <param name="Ilimit"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon?offset={Ioffset}&limit={Ilimit}")]
        Task<ApiResponse<PokemonList>> GetPokemon([AliasAs("Ioffset")] int Ioffset, [AliasAs("Ilimit")] int Ilimit);

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon/{id}")]
        Task<ApiResponse<PokemonStats>> GetPokemonById(int id);

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por nombre
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon/{name}")]
        Task<ApiResponse<PokemonStats>> GetPokemonByName(string name);

        /// <summary>
        /// Método para buscar especies de Pokémon
        /// </summary>
        /// <returns></returns>
        [Get("/api/v2/pokemon-species?offset=0&limit=-1")]
        Task<ApiResponse<PokemonSpeciesList>> SearchSpecies();

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su especie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Get("/api/v2/pokemon-species/{name}")]
        Task<ApiResponse<PokemonSpeciesDetail>> GetSpeciesByName(string name);

    }
}
