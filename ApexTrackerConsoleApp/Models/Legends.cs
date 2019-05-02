using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApexTrackerConsoleApp.Models
{
    public class Legends
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("metadata")]
        public LegendMetadata LegendMetadata { get; set; }

        [JsonProperty("stats")]
        public List<Stats> Stats { get; set; }
    }
}