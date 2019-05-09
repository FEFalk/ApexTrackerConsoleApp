using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class Stats
    {
        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("value")]
        public float Value { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("metadata")]
        public MetadataDto MetadataDto { get; set; }
    }
}
