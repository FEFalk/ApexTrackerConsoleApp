using Newtonsoft.Json;

namespace ApexTrackerConsoleApp.Models
{
    public class ChildrenMetadata
    {
        [JsonProperty("platformId")]
        public int PlatformId { get; set; }

        [JsonProperty("platformUserHandle")]
        public string UserName { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }
    }
}