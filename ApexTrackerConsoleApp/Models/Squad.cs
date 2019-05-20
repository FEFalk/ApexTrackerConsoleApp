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

        string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";
        public Squad(string name, int id)
        {
            this.Id = id;
            this.Name = name;
        }
        public void AddPlayers(List<Player> players)
        {
            PlayerList = players.Where(x => x.SquadId == Id).ToList();
        }
        public void SetPlayersInactive()
        {
            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandSetGameSessionData();

            //Console.WriteLine("Setting players to inactive");
            PlayerList.ForEach(x => x.Active = false);

            PlayerList.ForEach(x => dbConnection.UpdateGameSessionData(x));
        }
        public bool UpdateTrackers()
        {
            bool success = true;
            Player playerWithWinsTracker = PlayerList.FirstOrDefault(x => x.OffsetWins > -1);
            Player playerWithTop3Tracker = PlayerList.FirstOrDefault(x => x.OffsetTop3 > -1);
            
            if (playerWithTop3Tracker != null)
            {
                playerWithTop3Tracker.HasTop3Tracker = true;
            }
            if(playerWithWinsTracker != null)
            {
                playerWithWinsTracker.HasWinTracker = true;
            }

            if (playerWithWinsTracker == null || playerWithTop3Tracker == null)
            {
                SetPlayersInactive();
                success = false;
            }
            return success;
        }
    }
}
