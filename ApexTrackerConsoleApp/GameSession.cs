using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApexTrackerConsoleApp
{
    class GameSession
    {
        DbConnection dbConnection = new DbConnection();
        string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";

        public GameSessionModel gameSession = new GameSessionModel();
        public GameSessionModel GetGameSession(int gameSessionId)
        {
            string sqlcommand = "SELECT Id, StartTime, EndTime, MaxPlayers, Canceled " +
                                "FROM [ApexTrackerDb].[dbo].[GameSession] " +
                                "where [dbo].[GameSession].[Id] = @gamesessionid " +
                                "and [dbo].[GameSession].[Canceled] = 0";
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = new List<Item>();
            dbConnection.Items = dbConnection.ReadGameSessionFromDb(sqlcommand, gameSessionId);

            gameSession = new GameSessionModel();
            foreach (Item item in dbConnection.Items)
            {
                gameSession.Id = item.GameSessionId;
                gameSession.StartTime = item.StartTime;
                gameSession.EndTime = item.EndTime;
                gameSession.MaxPlayers = item.MaxPlayers;
            }
            return gameSession;
        }
        public int GetGameSessionID()
        {
            int gameSessionId;
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = new List<Item>();
            string sqlcommand = "SELECT Id, StartTime, EndTime, MaxPlayers, Canceled " +
                    "FROM [ApexTrackerDb].[dbo].[GameSession] " +
                    "where  GETDATE() >=  DATEADD(MINUTE, -5, [dbo].[GameSession].[StartTime]) " + 
                    "and GETDATE () <= [dbo].[GameSession].[EndTime] " +
                    "and [dbo].[GameSession].[Canceled] = 0";
            gameSessionId = dbConnection.ReadActiveSessionFromDb(sqlcommand);
            if (gameSessionId != 0)
                return gameSessionId;
            else
                return 0;
        }

        public List<Item> PlayersExist(GameSessionModel gameSession)
        {
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = new List<Item>();
            string sqlcommand = "SELECT UserName, PlayerId, GameSessionId, LegendId, Kills, Top3, Wins, OffsetKills, OffsetTop3, OffsetWins " +
                     "FROM [ApexTrackerDb].[dbo].[GameSessionData], dbo.Player " +
                     "where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid " +
                     "and [dbo].[GameSessionData].[PlayerId] = dbo.Player.Id ";

            dbConnection.Items = dbConnection.ReadPlayersFromDb(sqlcommand, gameSession);
            if (dbConnection.Items.Count > 0) return (dbConnection.Items);
            else return null;
        }
        public void CancelGameSession(GameSessionModel gameSession)
        {
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = new List<Item>();
            StringBuilder command = new StringBuilder();
            command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSession]");
            command.Append(" SET Canceled = @canceled");
            command.Append(" where [dbo].[GameSession].[Id] = @gamesessionid");

            dbConnection.WriteCancelToDb(command.ToString(), gameSession);
        }
    }
}
