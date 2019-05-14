using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp.Models
{
    public class Squad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Player> PlayerList { get; set; }
        public bool HasTop3Tracker { get; set; }
        public bool HasWinTracker { get; set; }

        public Squad(string name, int id)
        {
            this.Id = id;
            this.Name = name;
        }
        public void AddPlayers(List<Player> players)
        {
            PlayerList = players.Where(x => x.SquadId == Id).ToList();
        }
        public bool UpdateTrackers()
        {
            var hasWinTracker = PlayerList.FirstOrDefault(x => x.Wins > -1).HasWinTracker = true;
            var hasTop3Tracker = PlayerList.FirstOrDefault(x => x.Top3 > -1).HasTop3Tracker = true;

            if (!hasWinTracker || !hasTop3Tracker)
            {
                PlayerList.ForEach(x => x.Active = false);
                return false;
            }
            return true;
        }
    }
}
