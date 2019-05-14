using ApexTrackerConsoleApp.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ApexTrackerConsoleApp.Models
{
    public class Player
    {
        public string UserName;
        public string PlayerId;
        public int SquadId;
        public int GameSessionId;
        public int LegendId;
        public string LegendName;
        public int Kills;
        public int Top3;
        public int Wins;
        public int OffsetKills;
        public int OffsetTop3;
        public int OffsetWins;
        public bool HasTop3Tracker;
        public bool HasWinTracker;
        public bool Active;

        string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";

        public Player(Item item)
        {
            UserName = item.UserName;
            PlayerId = item.PlayerId;
            SquadId = item.SquadId;
            GameSessionId = item.GameSessionId;
            LegendId = item.LegendId;
            LegendName = item.LegendName;
            Kills = item.Kills;
            Top3 = item.Top3;
            Wins = item.Wins;
            OffsetKills = item.OffsetKills;
            OffsetTop3 = item.OffsetTop3;
            OffsetWins = item.OffsetWins;
            HasTop3Tracker = item.HasTop3Tracker;
            HasWinTracker = item.HasWinTracker;
            Active = item.Active;
        }

        public async void UpdateStatsFromAPI()
        {
            var playerAPIResult = await ApexController.GetApexPlayerAPI(UserName, LegendName);
            
            if(playerAPIResult != null)
            {
                Kills = (int)playerAPIResult.Kills;
                Top3 = (int)playerAPIResult.Top3;
                Wins = (int)playerAPIResult.Wins;   
            }
            else
            {
                Console.WriteLine($"Player [" + UserName + "] not found!");
                return;
            }

            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandUpdateGameSessionData();
            dbConnection.UpdateGameSessionData(this);

        }

        public async void SetStatsFromAPI()
        {
            var playerAPIResult = await ApexController.GetApexPlayerOffsetsAPI(UserName);

            if (playerAPIResult != null)
            {
                OffsetKills = (int)playerAPIResult.Kills;
                OffsetTop3 = (int)playerAPIResult.Top3;
                OffsetWins = (int)playerAPIResult.Wins;
                LegendName = playerAPIResult.Icon;
            }
            else
            {
                Console.WriteLine($"Player [" + UserName + "] not found!");
                return;
            }

            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandSetGameSessionData();
            dbConnection.UpdateGameSessionData(this);
        }

        public void UpdateTrackers(bool winTracker, bool top3Tracker)
        {
            HasWinTracker = winTracker;
            HasTop3Tracker = top3Tracker;
        }
    }
}
