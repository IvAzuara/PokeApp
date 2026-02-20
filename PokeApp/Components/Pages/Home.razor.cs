using InjectedServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Models.Pokemon;

namespace PokeApp.Components.Pages
{
    public partial class HomeBase : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            await CargarPokemons();
        }

        [Inject] private IPokeService _pokeService { get; set; } = default!;

        protected PokemonList _pokemonList = new();

        protected IEnumerable<PokemonListItem>? _dsPokemonListItem;

        protected async Task CargarPokemons()
        {
            try
            {
                var response = await _pokeService.GetPokemon(20, 20);
                _dsPokemonListItem = response.results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}