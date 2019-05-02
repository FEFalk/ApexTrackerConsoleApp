using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApexTrackerConsoleApp
{
    public class Item
    {
        public string UserName { get; set; }
        public int Kills { get; set; }
        public int OffsetKills { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Databasanrop select player in gamesession and gamesessiondata
            //databasanropet fyller datan

            //private List<Item> GetView(string view)
            //{
            List<Item> Items = new List<Item>();

            var connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";

            // Creates a SQL connection
            var conn = new SqlConnection(connection);
            conn.Open();

            // Creates a SQL command
            var command = new SqlCommand("SELECT UserName, Kills, Top3, Wins, OffsetKills, OffsetTop3, OffsetWins " +
                                         "FROM [ApexTrackerDb].[dbo].[GameSessionData], dbo.Player " +
                                         "where [dbo].[GameSessionData].[GameSessionId] = 1 " +
                                         "and [dbo].[GameSessionData].[PlayerId] = dbo.Player.Id ", conn);
            // Loads the query results into the table
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Items.Add(new Item
                    {                       
                        UserName = reader[0].ToString(),
                        Kills = Int32.Parse(reader[1].ToString()),
                        OffsetKills = Int32.Parse(reader[4].ToString())
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                        
            }
            conn.Close();

            List<GameSessionData> gameSessionDataList = new List<GameSessionData>();
            var getPlayerData = new GetPlayerData();

            foreach (Item item in Items)
            {
                GameSessionData gameSessionData = new GameSessionData();
                PlayerDto player = new PlayerDto();
                player.Name = item.UserName;
                gameSessionData.Player = player;
                gameSessionData.Kills = item.Kills;
                gameSessionData.OffsetKills = item.OffsetKills;
                gameSessionDataList.Add(gameSessionData);
            }

            getPlayerData.Run(gameSessionDataList);

            Console.WriteLine("\nPress any key to exit!\n");
            Console.ReadKey();
        }
    }
}
