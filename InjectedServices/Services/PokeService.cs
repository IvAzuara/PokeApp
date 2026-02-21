using InjectedServices.Interfaces;
using InjectedServices.Interfaces.Refit;
using Models.Pokemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectedServices.Services
{
    /// <summary>
    /// Clase que implementa el servicio de Pokémon, que interactúa con la API de Pokémon utilizando la interfaz IPokeAPI y Refit.
    /// </summary>
    public class PokeService : IPokeService
    {
        private IPokeAPI _pokeAPI;
        public PokeService(IPokeAPI _) => _pokeAPI = _;

        /// <summary>
        /// Método para obtener la lista de Pokémon desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP. Maneja errores y devuelve la lista de Pokémon o lanza una excepción si ocurre un error.
        /// </summary>
        /// <param name="Ioffset"></param>
        /// <param name="Ilimit"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PokemonList> GetPokemon(int Ioffset, int Ilimit)
        {
            var response = await _pokeAPI.GetPokemon(Ioffset, Ilimit);

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su ID desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP. Maneja errores y devuelve los detalles del Pokémon o lanza una excepción si ocurre un error.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PokemonStats> GetPokemonById(int id)
        {
            var response = await _pokeAPI.GetPokemonById(id);

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su nombre desde la API de Pokémon, utilizando Refit para realizar la solicitud HTTP. Maneja errores y devuelve los detalles del Pokémon o lanza una excepción si ocurre un error.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PokemonStats> GetPokemonByName(string name)
        {
            var response = await _pokeAPI.GetPokemonByName(name);

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }
    }
}
