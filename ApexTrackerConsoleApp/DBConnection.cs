using ApexTrackerConsoleApp.Controllers;
using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApexTrackerConsoleApp
{
    public class Item
    {
        public string UserName { get; set; }
        public string PlayerId { get; set; }
        public int SquadId { get; set; }
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
        public bool Active { get; set; }

    }
    public class SquadItem
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
    class DbConnection
    {
        public SqlConnection conn;

        public List<Item> Items = new List<Item>();
        public List<SquadItem> SquadItems = new List<SquadItem>();
        StringBuilder command = new StringBuilder();
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public void WriteCancelToDb(string command, GameSessionDto gameSession)
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }

        public void SetCommandUpdateGameSessionData()
        {
            command.Clear();
            command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
            command.Append(" SET Kills = @kills,");
            command.Append(" Top3 = @top3,");
            command.Append(" Wins = @wins");
            command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
            command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");
            command.Append(" and [dbo].[GameSessionData].[LegendId] = @legendid");
        }
        public void SetCommandSetGameSessionData()
        {
            command.Clear();
            command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
            command.Append(" SET OffsetKills = @offsetkills,");
            command.Append(" OffsetTop3 = @offsettop3,");
            command.Append(" OffsetWins = @offsetwins,");
            command.Append(" LegendId = @legendId,");
            command.Append(" HasTop3Tracker = @hastop3tracker,");
            command.Append(" HasWinTracker = @haswintracker,");
            command.Append(" Active = @active");
            command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
            command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");
        }
        public void UpdateGameSessionData(Player player)
        {
            if(player.LegendName != null)
                player.LegendId = GetLegendIdFromDb(player.LegendName);
            try
            {
                conn.Open();
                var sqlcommand = new SqlCommand(command.ToString(), conn);
                sqlcommand.CommandText = command.ToString();
                sqlcommand.Parameters.AddWithValue("@kills", player.Kills);
                sqlcommand.Parameters.AddWithValue("@top3", player.Top3);
                sqlcommand.Parameters.AddWithValue("@wins", player.Wins);
                sqlcommand.Parameters.AddWithValue("@offsetkills", player.OffsetKills);
                sqlcommand.Parameters.AddWithValue("@offsettop3", player.OffsetTop3);
                sqlcommand.Parameters.AddWithValue("@offsetwins", player.OffsetWins);
                sqlcommand.Parameters.AddWithValue("@legendid", player.LegendId);
                sqlcommand.Parameters.AddWithValue("@hastop3tracker", player.HasTop3Tracker);
                sqlcommand.Parameters.AddWithValue("@active", player.Active);
                sqlcommand.Parameters.AddWithValue("@haswintracker", player.HasWinTracker);
                sqlcommand.Parameters.AddWithValue("@gamesessionid", player.GameSessionId);
                sqlcommand.Parameters.AddWithValue("@playerid", player.PlayerId);

                var writer = sqlcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }

        public void SetCommandReadPlayersFromDb()
        {
            command.Clear();
            command.Append("SELECT UserName, PlayerId, SquadId, GameSessionId, LegendId, Kills, Top3, Wins, OffsetKills, OffsetTop3, OffsetWins, HasTop3Tracker, HasWinTracker, Active");
            command.Append(" FROM [ApexTrackerDb].[dbo].[GameSessionData], dbo.Player");
            command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
            command.Append(" and [dbo].[GameSessionData].[PlayerId] = dbo.Player.Id");
        }
        public List<Item> ReadPlayersFromDb(GameSessionDto gameSession)
        {          
            try
            {
                //if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command.ToString(), conn);
                sqlcommand.Parameters.AddWithValue("@gamesessionid", gameSession.Id);
                var reader = sqlcommand.ExecuteReader();
                Items = new List<Item>();
                while (reader.Read())
                {
                    Items.Add(new Item
                    { 
                        UserName = reader[0].ToString(),
                        PlayerId = reader[1].ToString(),
                        SquadId = Int32.Parse(reader[2].ToString()),
                        GameSessionId = Int32.Parse(reader[3].ToString()),
                        LegendId = Int32.Parse(reader[4].ToString()),
                        Kills = Int32.Parse(reader[5].ToString()),
                        Top3 = Int32.Parse(reader[6].ToString()),
                        Wins = Int32.Parse(reader[7].ToString()),
                        OffsetKills = Int32.Parse(reader[8].ToString()),
                        OffsetTop3 = Int32.Parse(reader[9].ToString()),
                        OffsetWins = Int32.Parse(reader[10].ToString()),
                        HasTop3Tracker = Boolean.Parse(reader[11].ToString()),
                        HasWinTracker = Boolean.Parse(reader[12].ToString()),
                        Active = Boolean.Parse(reader[13].ToString())
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
            return Items;
        }

        public void SetCommandReadSquadsFromDb()
        {
            command.Clear();
            command.Append("SELECT SquadId, Name");
            command.Append(" FROM [ApexTrackerDb].[dbo].[GameSessionData], [dbo].[Player], [dbo].[Squad]");
            command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
            command.Append(" and [dbo].[GameSessionData].[PlayerId] = dbo.Player.Id");
            command.Append(" and [dbo].[Squad].[Id] = [dbo].[Player].[SquadId]");
            command.Append(" group by SquadId, Name");
        }
        public List<SquadItem> ReadSquadsFromDb(GameSessionDto gameSession)
        {
            try
            {
                //if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command.ToString(), conn);
                sqlcommand.Parameters.AddWithValue("@gamesessionid", gameSession.Id);
                var reader = sqlcommand.ExecuteReader();
                SquadItems = new List<SquadItem>();
                while (reader.Read())
                {
                    SquadItems.Add(new SquadItem
                    {
                        Id = Int32.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),
                    });
                }
                reader.Dispose();
                reader.Close();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
            return SquadItems;
        }
        public void SetCommandReadGameSessionFromDb()
        {
            command.Clear();
            command.Append("SELECT Id, StartTime, EndTime, MaxPlayers, Canceled");
            command.Append(" FROM [ApexTrackerDb].[dbo].[GameSession]");
            command.Append(" where [dbo].[GameSession].[Id] = @gamesessionid");
        }
        public List<Item> ReadGameSessionFromDb(int gameSessionId)
        {
            try
            {
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command.ToString(), conn);
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            finally
            {
                conn.Close();
            }
            return Items;
        }
        public void SetCommandReadActiveGameSessionFromDb()
        {
            command.Clear();
            command.Append("SELECT Id, StartTime, EndTime, MaxPlayers, Canceled");
            command.Append(" FROM [ApexTrackerDb].[dbo].[GameSession]");
            command.Append(" where  GETDATE() >=  DATEADD(MINUTE, -5, [dbo].[GameSession].[StartTime])");
            command.Append(" and GETDATE () <= [dbo].[GameSession].[EndTime]");
            command.Append(" and [dbo].[GameSession].[Canceled] = 0");
        }
        public int ReadActiveSessionFromDb()
        {
            try
            {
                //TODO: if open dont open               
                conn.Open();
                var sqlcommand = new SqlCommand(command.ToString(), conn);
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
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
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
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
