using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApexTrackerConsoleApp.Models
{
    public class LegendsDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("metadata")]
        public LegendMetadataDto LegendMetadataDto { get; set; }

        [JsonProperty("stats")]
        public List<Stats> Stats { get; set; }
    }
}