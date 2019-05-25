using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApexTrackerConsoleApp.Controllers;
using ApexTrackerConsoleApp.Models;

namespace ApexTrackerConsoleApp
{
    class Application
    {
        DbConnection dbConnection = new DbConnection();
        string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";

        public List<Player> playerList = new List<Player>();
        public List<Squad> squadList = new List<Squad>();
        public List<Squad> squadsWithoutTracker = new List<Squad>();
        public void Run(GameSessionDto GameSessionDto)
        {
            dbConnection.ConnectToDb(connection);

            if (playerList == null)
            {
                Console.WriteLine("Playerlist empty, updating players failed.");
                return;
            }
            foreach (Player player in playerList)
            {
                player.UpdateStatsFromAPI();
                // IMPORTANT! Wait 3 seconds between each request against apex.tracker.gg, or else we will get banned.
                Thread.Sleep(3000);

            }
        }
        public void BuildPlayerList(GameSessionDto gameSession)
        {
            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandReadPlayersFromDb();
            dbConnection.Items = dbConnection.ReadPlayersFromDb(gameSession);

            playerList = new List<Player>();
            if (dbConnection.Items == null)
            {
                Console.WriteLine("No items found in DB.");
                return;
            }
            foreach (Item item in dbConnection.Items)
            {
                if (item.Active)
                {
                    Player player = new Player(item);

                    if (item.LegendId > 0)
                    {
                        player.LegendName = dbConnection.GetLegendNameFromDb(item.LegendId);
                    }
                    playerList.Add(player);
                }

            }
        }
        public void CalibratePlayerList()
        {
            if (playerList == null)
            {
                Console.WriteLine("Playerlist is empty, calibration failed.");
                return;
            }
            foreach (Player player in playerList)
            {
                if(player.Active)
                    player.SetStatsFromAPI();
            }
        }
        public void BuildSquadList(GameSessionDto gameSession)
        {

            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandReadSquadsFromDb();
            dbConnection.SquadItems = dbConnection.ReadSquadsFromDb(gameSession);
            if (dbConnection.SquadItems == null)
            {
                Console.WriteLine("SquadItems from DB is empty, building squadlist failed.");
                return;
            }
            foreach (SquadItem squaditem in dbConnection.SquadItems)
            {
                Squad squad = new Squad(squaditem.Name, squaditem.Id);
                squad.AddPlayers(playerList);

                squadList.Add(squad);
            }
        }
        public void ValidateSquadSize()
        {
            List<Squad> squadsToRemove = new List<Squad>();
            foreach (Squad squad in squadList)
            {
                if (squad.PlayerList.Count < 3)
                {
                    Console.WriteLine("Squad " + squad.Name + " is incomplete with " + squad.PlayerList.Count + "/3 players. Kicking...");
                    squad.SetPlayersInactive();
                    squadsToRemove.Add(squad);
                }
            }
            foreach (Squad squad in squadsToRemove)
            {
                foreach(Player player in squad.PlayerList)
                {
                    playerList.Remove(player);
                }
                squadList.Remove(squad);
            }
        }
        public void ValidateSquadTrackers()
        {
            List<Squad> squadsToRemove = new List<Squad>();
            foreach (Squad squad in squadList)
            {
                Player playerWithWinsTracker = squad.PlayerList.FirstOrDefault(x => x.OffsetWins > -1);
                Player playerWithTop3Tracker = squad.PlayerList.FirstOrDefault(x => x.OffsetTop3 > -1);

                if (playerWithWinsTracker == null || playerWithTop3Tracker == null)
                {
                    Console.WriteLine("Squad " + squad.Name + " is missing a tracker. Kicking...");
                    squad.SetPlayersInactive();
                    squadsToRemove.Add(squad);
                }
            }
            foreach (Squad squad in squadsToRemove)
            {
                foreach (Player player in squad.PlayerList)
                {
                    playerList.Remove(player);
                }
                squadList.Remove(squad);
            }
        }
        public void UpdateSquadTrackers()
        {
            if (squadList == null)
            {
                Console.WriteLine("No Squads found, updating squadlist failed.");
                return;
            }
            foreach (Squad squad in squadList)
            {
                var success = squad.UpdateTrackers();

                if (!success)
                {
                    squadsWithoutTracker.Add(squad);
                }
            }
        }
    }
}
