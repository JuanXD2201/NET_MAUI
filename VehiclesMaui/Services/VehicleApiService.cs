using System.Net.Http.Json;
using VehiclesMaui.Models;

namespace VehiclesMaui.Services;

public class VehicleApiService
{
    private readonly HttpClient _httpClient;

    public VehicleApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VehicleMake>> GetAllMakesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<VehicleMakeResponse>(
                "https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json",
                cancellationToken);
            return response?.Results ?? [];
        }
        catch
        {
            return [];
        }
    }
}