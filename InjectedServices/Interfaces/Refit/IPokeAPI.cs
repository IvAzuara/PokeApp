using Models.Pokemon;
using Refit;

namespace InjectedServices.Interfaces.Refit
{
    public interface IPokeAPI
    {
        [Get("/api/v2/pokemon?offset={Ioffset}&limit={Ilimit}")]
        Task<ApiResponse<PokemonList>> GetPokemon(int Ioffset, int Ilimit);
    }
}
