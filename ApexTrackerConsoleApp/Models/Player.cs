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
        public string OriginId;
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
        public int Platform;
        public bool InMatch;
        public int RankScore;

        public Player(Item item)
        {
            UserName = item.UserName;
            PlayerId = item.PlayerId;
            OriginId = item.OriginId;
            SquadId = item.SquadId;
            GameSessionId = item.GameSessionId;
            LegendId = item.LegendId;
            LegendName = item.LegendName;
            Kills = item.OffsetKills;
            Top3 = item.OffsetTop3;
            Wins = item.OffsetWins;
            OffsetKills = item.OffsetKills;
            OffsetTop3 = item.OffsetTop3;
            OffsetWins = item.OffsetWins;
            HasTop3Tracker = item.HasTop3Tracker;
            HasWinTracker = item.HasWinTracker;
            Active = item.Active;
            Platform = item.PlatformApexTrackerId;
            InMatch = item.InMatch;
            RankScore = item.RankScore;
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
                
                var playerAPIResult = await ApexController.GetApexPlayerAPI(OriginId, LegendName, Platform);

                if (playerAPIResult != null)
                {
                    if(Kills < playerAPIResult.Kills || 
                       (HasTop3Tracker && Top3 < playerAPIResult.Top3) || 
                       (HasWinTracker && Wins < playerAPIResult.Wins) ||
                       (InMatch && !playerAPIResult.InMatch))
                    {
                        //Deltavalues used for gamesessiondatalog so only the current update is logged.
                        Kills = playerAPIResult.Kills - Kills;
                        Top3 = playerAPIResult.Top3 - Top3;
                        Wins = playerAPIResult.Wins - Wins;
                        InMatch = playerAPIResult.InMatch;
                        dbConnection.ConnectToDb(Application.connection);
                        dbConnection.SetCommandInsertGameSessionDataLog();
                        dbConnection.UpdateGameSessionDataLog(this);
                    }
                    Kills = playerAPIResult.Kills;
                    Top3 = playerAPIResult.Top3;
                    Wins = playerAPIResult.Wins;

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
            var playerAPIResult = ApexController.GetApexPlayerOffsetsAPI(OriginId, Platform)?.Result;

            if (playerAPIResult != null)
            {
                this.Kills = (int)playerAPIResult.Kills;
                this.Top3 = (int)playerAPIResult.Top3;
                this.Wins = (int)playerAPIResult.Wins;
                this.OffsetKills = (int)playerAPIResult.Kills;
                this.OffsetTop3 = (int)playerAPIResult.Top3;
                this.OffsetWins = (int)playerAPIResult.Wins;
                this.LegendName = playerAPIResult.LegendName;
                this.InMatch = playerAPIResult.InMatch;
                this.RankScore = playerAPIResult.RankScore;
                
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
