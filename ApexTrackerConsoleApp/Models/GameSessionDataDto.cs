using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class GameSessionDataDto
    {
        [Required]
        public string PlayerId { get; set; }
        public int SquadId { get; set; }

        [Required]
        public int GameSessionId { get; set; }

        public int LegendId { get; set; }

        [Required]
        public PlayerDto Player { get; set; }

        [Required]
        public GameSessionDto GameSessionDto { get; set; }

        public int Kills { get; set; }

        public int Top3 { get; set; }

        public int Wins { get; set; }

        public int OffsetKills { get; set; }

        public int OffsetTop3 { get; set; }

        public int OffsetWins { get; set; }

        public bool HasTop3Tracker { get; set; }

        public bool HasWinTracker { get; set; }

        public bool Active { get; set; }


    }
}
