using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class CoordinateInfo
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("lat")] public double Lat{ get; set; }
        [JsonPropertyName("lon")] public double Lon { get; set; }
        [JsonPropertyName("country")] public string Country { get; set; }
    }
}
