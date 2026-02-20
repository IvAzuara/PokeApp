using Models.Pokemon;

namespace InjectedServices.Interfaces
{
    public interface IPokeService
    {
        Task<PokemonList> GetPokemon(int Ioffset, int Ilimit);
    }
}
