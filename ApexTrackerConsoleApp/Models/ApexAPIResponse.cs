using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class ApexAPIResponse<T>
    {
        /// <summary>
        /// The data returned from the operation.
        /// </summary>
        [JsonProperty("userInfo")]
        public T data { get; set; }
    }
}
