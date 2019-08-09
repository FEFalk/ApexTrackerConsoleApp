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
        public const string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";
        public const string errorFilePath = @".\Error.txt";
        public List<Player> playerList = new List<Player>();
        public List<Squad> squadList = new List<Squad>();
        public List<Squad> squadsToRemove = new List<Squad>();
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
                Thread.Sleep(1000);
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
                    if (item.PlatformId > 0)
                    {
                        player.Platform = dbConnection.GetPlatformApexTrackerIdFromDb(item.PlatformId);
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
                if (player.Active)
                {
                    player.SetStatsFromAPI();
                    Thread.Sleep(1000);
                }
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
        public void RemoveIncorrectSquads()
        {
            if(squadsToRemove != null)
            {
                foreach (Squad squad in squadsToRemove)
                {
                    Console.WriteLine("removing squad: " + squad.Name);
                    foreach (Player player in squad.PlayerList)
                    {
                        playerList.Remove(player);
                    }
                    squad.SetPlayersInactive();
                    squadList.Remove(squad);
                }
                squadsToRemove = new List<Squad>();
            }
        }
        public void ValidateSquads()
        {
            if (squadList == null || squadList.Count <= 0)
            {
                Console.WriteLine("No Squads found, updating squadlist failed.");
                return;
            }
            foreach (Squad squad in squadList)
            {
                if (squad.PlayerList.Count < 3)
                {
                    if (!squadsToRemove.Contains(squad))
                        squadsToRemove.Add(squad);
                    Console.WriteLine("squad has less than 3 players: " + squad.Name);
                }
                else
                {
                    if (squad.CheckDuplicateLegends())
                    {
                        squadsToRemove.Add(squad);
                        Console.WriteLine("Squad has duplicate legends: " + squad.Name);
                    }
                    else
                    {
                        var success = squad.ValidateTrackers();
                        if (!success)
                            squadsToRemove.Add(squad);

                        else if (success && squadsToRemove.Contains(squad))
                            squadsToRemove.Remove(squad);
                    }
                }
            }
        }
    }
}
