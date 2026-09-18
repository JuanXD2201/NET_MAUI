using System.Text.Json.Serialization;

namespace VehiclesMaui.Models;

public class VehicleMakeResponse
{
    [JsonPropertyName("Results")]
    public List<VehicleMake> Results { get; set; } = [];
}

public class VehicleMake
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Make_Name")]
    public string MakeName { get; set; } = string.Empty;
}