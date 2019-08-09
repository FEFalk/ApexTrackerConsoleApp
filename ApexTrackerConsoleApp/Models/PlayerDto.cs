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
        public float Kills { get; set; }
        public string Name { get; set; }
        public float Wins { get; set; }
        public float Top3 { get; set; }
        public string Icon { get; set; }
        public string BGImage { get; set; }
        public int Level { get; set; }
    }
}
