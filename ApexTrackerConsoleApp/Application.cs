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
        public void Run()
        {
            dbConnection.ConnectToDb(connection);
            
            if(playerList == null)
            {
                Console.WriteLine("Playerlist empty, updating players failed.");
                return;
            }
            foreach (Player player in playerList)
            {
                player.LegendName = dbConnection.GetLegendNameFromDb(player.LegendId);
                player.UpdateStatsFromAPI();
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
                Player player = new Player(item);              
                playerList.Add(player);
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
                player.SetStatsFromAPI();
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
        public void CalibrateSquadList()
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
