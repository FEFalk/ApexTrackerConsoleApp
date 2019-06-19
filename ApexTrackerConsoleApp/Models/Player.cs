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
            try
            {
                if (UserName == null)
                    return;
                DbConnection dbConnection = new DbConnection();
                if (LegendName == null)
                    LegendName = dbConnection.GetLegendNameFromDb(LegendId);
                
                var playerAPIResult = await ApexController.GetApexPlayerAPI(UserName, LegendName);

                if (playerAPIResult != null)
                {
                    if(Kills < (int)playerAPIResult.Kills || 
                       Top3 < (int)playerAPIResult.Top3 || 
                       Wins < (int)playerAPIResult.Wins)
                    {
                        //Deltavalues used for gamesessiondatalog so only the current update is logged.
                        Kills = (int)playerAPIResult.Kills - Kills;
                        Top3 = (int)playerAPIResult.Top3 - Top3;
                        Wins = (int)playerAPIResult.Wins - Wins;
                        dbConnection.ConnectToDb(Application.connection);
                        dbConnection.SetCommandInsertGameSessionDataLog();
                        dbConnection.UpdateGameSessionDataLog(this);
                    }
                    Kills = (int)playerAPIResult.Kills;
                    Top3 = (int)playerAPIResult.Top3;
                    Wins = (int)playerAPIResult.Wins;

                    dbConnection.ConnectToDb(Application.connection);
                    dbConnection.SetCommandUpdateGameSessionData();
                    dbConnection.UpdateGameSessionData(this);
                }
                else
                {
                    Console.WriteLine($"Player [" + UserName + "] not found!");
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        public void SetStatsFromAPI()
        {
            var playerAPIResult = ApexController.GetApexPlayerOffsetsAPI(UserName)?.Result;

            if (playerAPIResult != null)
            {
                this.OffsetKills = (int)playerAPIResult.Kills;
                this.OffsetTop3 = (int)playerAPIResult.Top3;
                this.OffsetWins = (int)playerAPIResult.Wins;
                this.LegendName = playerAPIResult.Icon;
            }
            else
            {
                Console.WriteLine($"Player [" + UserName + "] not found!");
                return;
            }

            DbConnection dbConnection = new DbConnection();
            dbConnection.ConnectToDb(Application.connection);
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
