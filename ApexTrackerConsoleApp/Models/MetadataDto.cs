using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class MetadataDto
    {
        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
