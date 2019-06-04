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
        public void SetPlayersInactive()
        {
            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(Application.connection);
            dbConnection.SetCommandSetGameSessionData();

            //Console.WriteLine("Setting players to inactive");
            PlayerList.ForEach(x => x.Active = false);

            PlayerList.ForEach(x => dbConnection.UpdateGameSessionData(x));
        }
        public void SetSquadPlayerTracker(Player playerWithTracker)
        {
            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(Application.connection);
            dbConnection.SetCommandSetGameSessionData();
            dbConnection.UpdateGameSessionData(playerWithTracker);
        }
        public bool CheckDuplicateLegends()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (PlayerList[i].PlayerId != PlayerList[j].PlayerId && PlayerList[i].LegendId == PlayerList[j].LegendId)
                        return true;
                }
            }
            return false;
        }
        public bool ValidateTrackers()
        {
            bool success = true;
            Player playerWithWinsTracker = PlayerList.FirstOrDefault(x => x.OffsetWins > -1);
            Player playerWithTop3Tracker = PlayerList.FirstOrDefault(x => x.OffsetTop3 > -1);

            if (playerWithTop3Tracker != null && !playerWithTop3Tracker.HasTop3Tracker)
            {
                playerWithTop3Tracker.HasTop3Tracker = true;
                SetSquadPlayerTracker(playerWithTop3Tracker);
            }
            if (playerWithWinsTracker != null && !playerWithWinsTracker.HasWinTracker)
            {
                playerWithWinsTracker.HasWinTracker = true;
                SetSquadPlayerTracker(playerWithWinsTracker);
            }

            return success;
        }
    }
}
