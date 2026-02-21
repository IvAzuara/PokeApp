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
        /// Método para obtener la lista de Pokémon 
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
        /// Método para obtener los detalles de un Pokémon específico por su ID 
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
        /// Método para obtener los detalles de un Pokémon específico por su nombre 
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

        /// <summary>
        /// Método para buscar especies de Pokémon por nombre 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PokemonSpeciesList> SearchSpecies()
        {
            var response = await _pokeAPI.SearchSpecies();

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }

        /// <summary>
        /// Método para obtener los detalles de un Pokémon específico por su especie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PokemonSpeciesDetail> GetSpeciesByName(string name)
        {
            var response = await _pokeAPI.GetSpeciesByName(name);

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }
    }
}
