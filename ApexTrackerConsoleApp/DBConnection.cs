using ApexTrackerConsoleApp.Controllers;
using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp
{
    public class Item
    {
        public string UserName { get; set; }
        public string PlayerId { get; set; }
        public int GameSessionId { get; set; }
        public int LegendId { get; set; }
        public string LegendName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxPlayers { get; set; }
        public bool Canceled { get; set; }
        public int Kills { get; set; }
        public int Top3 { get; set; }
        public int Wins { get; set; }
        public int OffsetKills { get; set; }
        public int OffsetTop3 { get; set; }
        public int OffsetWins { get; set; }
        public bool HasTop3Tracker { get; set; }
        public bool HasWinTracker { get; set; }
    }
    class DbConnection
    {
        public SqlConnection conn;

        public List<Item> Items = new List<Item>();
        public DbConnection()
        {
            conn = new SqlConnection();
        }
        public void ConnectToDb(string connection)
        {    
            // Creates a SQL connection
            conn = new SqlConnection(connection);
        }
        
        public void WriteToDb(string command)
        {        
            try
            {
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                var writer = sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void WriteToDb(string command, GameSessionDataModel gameSessionData, Task<PlayerDto> playerDataApexAPI)
        {
            try
            {
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.CommandText = command;
                sqlcommand.Parameters.AddWithValue("@kills", playerDataApexAPI.Result.Kills);
                sqlcommand.Parameters.AddWithValue("@top3", playerDataApexAPI.Result.Top3);
                sqlcommand.Parameters.AddWithValue("@wins", playerDataApexAPI.Result.Wins);
                sqlcommand.Parameters.AddWithValue("@legendid", gameSessionData.LegendId);
                sqlcommand.Parameters.AddWithValue("@gamesessionid", gameSessionData.GameSessionId);
                sqlcommand.Parameters.AddWithValue("@playerid", gameSessionData.PlayerId);

                var writer = sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void WriteCancelToDb(string command, GameSessionModel gameSession)
        {
            try
            {
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.CommandText = command;
                sqlcommand.Parameters.AddWithValue("@gamesessionid", gameSession.Id);
                sqlcommand.Parameters.AddWithValue("@canceled", true);

                var writer = sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public List<Item> ReadPlayersFromDb(string command, GameSessionModel gameSession)
        {          
            try
            {
                //if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.Parameters.AddWithValue("@gamesessionid", gameSession.Id);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                    Items.Add(new Item
                    { 
                        UserName = reader[0].ToString(),
                        PlayerId = reader[1].ToString(),
                        GameSessionId = Int32.Parse(reader[2].ToString()),
                        LegendId = Int32.Parse(reader[3].ToString()),
                        Kills = Int32.Parse(reader[4].ToString()),
                        Top3 = Int32.Parse(reader[5].ToString()),
                        Wins = Int32.Parse(reader[6].ToString()),
                        OffsetKills = Int32.Parse(reader[7].ToString()),
                        OffsetTop3 = Int32.Parse(reader[8].ToString()),
                        OffsetWins = Int32.Parse(reader[9].ToString()),
                        HasTop3Tracker = Boolean.Parse(reader[10].ToString()),
                        HasWinTracker = Boolean.Parse(reader[11].ToString())
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Items;
        }
        public List<Item> ReadGameSessionFromDb(string command, int gameSessionId)
        {
            try
            {
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.Parameters.AddWithValue("gamesessionid", Int32.Parse(gameSessionId.ToString()));
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                    Items.Add(new Item
                    {
                        GameSessionId = Int32.Parse(reader[0].ToString()),
                        StartTime = DateTime.Parse(reader[1].ToString()),
                        EndTime = DateTime.Parse(reader[2].ToString()),
                        MaxPlayers = Int32.Parse(reader[3].ToString()),
                        Canceled = Boolean.Parse(reader[4].ToString()),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Items;
        }
        public int ReadActiveSessionFromDb(string command)
        {
            try
            {
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {                    
                    Items.Add(new Item
                    {
                        GameSessionId = Int32.Parse(reader[0].ToString()),
                        StartTime = DateTime.Parse(reader[1].ToString()),
                        EndTime = DateTime.Parse(reader[2].ToString()),
                        MaxPlayers = Int32.Parse(reader[3].ToString()),
                        Canceled = Boolean.Parse(reader[4].ToString()),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if (Items.Count > 0)
                return Items[0].GameSessionId;
            else
                return 0;
        }
        public List<Item> ReadLegendFromDb(string command, string legendname)
        {
            try
            {
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.Parameters.AddWithValue("@legendname", legendname);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                    Items.Add(new Item
                    {
                        LegendId = Int32.Parse(reader[0].ToString()),
                        LegendName = reader[1].ToString(),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if (Items.Count > 0)
                return Items;
            else
                return null;
        }
        public string GetLegendNameFromDb(int legendId)
        {
            try
            {
                string command = "SELECT * " +
                                 "FROM [ApexTrackerDb].[dbo].[Legend] " +
                                 "where [dbo].[Legend].[id] = @legendid";
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.Parameters.AddWithValue("@legendid", legendId);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                    Items.Add(new Item
                    {
                        LegendId = Int32.Parse(reader[0].ToString()),
                        LegendName = reader[1].ToString(),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if (Items.Count > 0)
                return Items[0].LegendName;
            else
                return null;
        }
        public int GetLegendIdFromDb(string legendName)
        {
            try
            {
                string command = "SELECT * " +
                                 "FROM [ApexTrackerDb].[dbo].[Legend] " +
                                 "where [dbo].[Legend].[Name] = @legendname";
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command, conn);
                sqlcommand.Parameters.AddWithValue("@legendname", legendName);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                     Items.Add(new Item
                    {
                        LegendId = Int32.Parse(reader[0].ToString()),
                        LegendName = reader[1].ToString(),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if (Items.Count > 0)
                return Items[0].LegendId;
            else
                return 0;
        }
    }
}
