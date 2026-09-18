using VehiclesMaui.Services;
using VehiclesMaui.Models;

namespace VehiclesMaui;

public partial class MainPage : ContentPage
{
    private readonly VehicleApiService _vehicleApiService;
    private List<VehicleMake> _allMakes = [];

    public MainPage(VehicleApiService vehicleApiService)
    {
        InitializeComponent();
        _vehicleApiService = vehicleApiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_allMakes.Count == 0)
        {
            _allMakes = await _vehicleApiService.GetAllMakesAsync();
            VehiclesCollection.ItemsSource = _allMakes;

            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            VehiclesCollection.IsVisible = true;
        }
    }

    private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            VehiclesCollection.ItemsSource = _allMakes;
        }
        else
        {
            VehiclesCollection.ItemsSource = _allMakes
                .Where(v => v.MakeName.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}