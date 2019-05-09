using Newtonsoft.Json;

namespace ApexTrackerConsoleApp.Models
{
    public class LegendMetadataDto
    {
        [JsonProperty("legend_name")]
        public string LegendName { get; set; }

        [JsonProperty("icon")]
        public string IconURL { get; set; }

        [JsonProperty("bgimage")]
        public string BackgroundURL { get; set; }
    }
}