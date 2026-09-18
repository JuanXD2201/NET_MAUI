using PokedexMaui.Services;
using PokedexMaui.Models;

namespace PokedexMaui;

public partial class MainPage : ContentPage
{
    private readonly PokeApiService _pokeApiService;

    public MainPage(PokeApiService pokeApiService)
    {
        InitializeComponent();
        _pokeApiService = pokeApiService;
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchEntry.Text))
        {
            ShowError("Por favor, ingrese un nombre o número.");
            return;
        }

        // Bloquear UI y mostrar carga
        SearchEntry.IsEnabled = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        MessageLabel.IsVisible = false;
        ResultCard.IsVisible = false;

        try
        {
            var pokemon = await _pokeApiService.GetPokemonAsync(SearchEntry.Text);

            if (pokemon == null)
            {
                ShowError("Pokémon no encontrado. Verifique el dato ingresado.");
            }
            else
            {
                // Mapeo de datos a la interfaz
                NameLabel.Text = pokemon.Name.ToUpper();
                IdLabel.Text = $"#{pokemon.Id}";
                HeightLabel.Text = $"Altura: {pokemon.Height / 10.0} m";
                WeightLabel.Text = $"Peso: {pokemon.Weight / 10.0} kg";
                ExperienceLabel.Text = $"Exp Base: {pokemon.BaseExperience}";

                if (pokemon.Types != null && pokemon.Types.Any())
                {
                    var types = string.Join(", ", pokemon.Types.Select(t => t.Type.Name));
                    TypeLabel.Text = $"Tipo: {types}";
                }
                else
                {
                    TypeLabel.Text = "Tipo: N/A";
                }

                if (!string.IsNullOrWhiteSpace(pokemon.Sprites?.FrontDefault))
                {
                    PokemonImage.Source = ImageSource.FromUri(new Uri(pokemon.Sprites.FrontDefault));
                }

                ResultCard.IsVisible = true;
            }
        }
        catch (HttpRequestException)
        {
            ShowError("Error de red. Verifique su conexión a internet.");
        }
        catch (Exception)
        {
            ShowError("Ocurrió un error inesperado al procesar la solicitud.");
        }
        finally
        {
            // Restaurar estado de la UI
            SearchEntry.IsEnabled = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void ShowError(string message)
    {
        MessageLabel.Text = message;
        MessageLabel.IsVisible = true;
        ResultCard.IsVisible = false;
    }
}