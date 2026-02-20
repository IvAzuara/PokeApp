using InjectedServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Models.Pokemon;
using MudBlazor;

namespace PokeApp.Components.Pages
{
    public partial class HomeBase : ComponentBase
    {

        [Inject] private IPokeService _pokeService { get; set; } = default!;

        protected PokemonList _pokemonList = new();
        protected IEnumerable<PokemonListItem>? _dsPokemonListItem;
        protected int _selected = 1;

        protected async Task<GridData<PokemonListItem>> LoadServerData(GridState<PokemonListItem> state)
        {
            int offset = state.Page * state.PageSize;
            int limit = state.PageSize;

            var response = await _pokeService.GetPokemon(offset, limit);

            return new GridData<PokemonListItem>
            {
                Items = response.results,
                TotalItems = response.count
            };
        }
    }
}