using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class FiveDaysForecast
    {
        [JsonPropertyName("list")] public List<ThreeHourlyForecast>? List { get; set; }
    }

    internal class ThreeHourlyForecast
    {
        [JsonPropertyName("dt_txt")] public string? Date { get; set; }
        [JsonPropertyName("main")] public Main? Main { get; set; }
        [JsonPropertyName("wind")] public Wind? Wind { get; set; }
    }

    internal class Main
    {
        [JsonPropertyName("temp")] public double Temp { get; set; }
        [JsonPropertyName("humidity")] public double Humidity { get; set; }
        [JsonPropertyName("feels_like")] public double FeelsLike { get; set; }
    }

    internal class Wind
    {
        [JsonPropertyName("speed")] public double Speed { get; set; }
    }
}
