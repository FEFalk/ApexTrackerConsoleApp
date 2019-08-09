using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class PlayerDto
    {
        public int Kills { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string OriginId { get; set; }
        public int Wins { get; set; }
        public int Top3 { get; set; }
        public int Level { get; set; }
        public string LegendName { get; set; }
        public bool InMatch { get; set; }
        public int RankScore { get; set; }

    }
}
