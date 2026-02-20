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
    public class PokeService : IPokeService
    {
        private IPokeAPI _pokeAPI;
        public PokeService(IPokeAPI _) => _pokeAPI = _;

        public async Task<PokemonList> GetPokemon(int Ioffset, int Ilimit)
        {
            var response = await _pokeAPI.GetPokemon(Ioffset, Ilimit);

            if (!response.IsSuccessStatusCode || response.Content is null)
                throw new Exception("Error consultando PokéAPI");

            return response.Content;
        }
    }
}
