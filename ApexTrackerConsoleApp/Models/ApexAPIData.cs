using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class ApexAPIData
    {
        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("children")]
        public List<LegendsDto> LegendsDto { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("metadata")]
        public ChildrenMetadataDto MetadataDto { get; set; }


        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("stats")]
        public List<Stats> Stats { get; set; }



    }
}
