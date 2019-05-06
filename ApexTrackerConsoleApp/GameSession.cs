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
            string sqlcommand = "SELECT Id, StartTime, EndTime, MaxPlayers " +
                                "FROM [ApexTrackerDb].[dbo].[GameSession] " +
                                "where [dbo].[GameSession].[Id] = @gamesessionid ";
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
            string sqlcommand = "SELECT Id, StartTime, EndTime, MaxPlayers " +
                    "FROM [ApexTrackerDb].[dbo].[GameSession] " +
                    "where  GETDATE() >= [dbo].[GameSession].[StartTime] " + 
                    "and GETDATE () <= [dbo].[GameSession].[EndTime] ";
            gameSessionId = dbConnection.ReadActiveSessionFromDb(sqlcommand);
            if (gameSessionId != 0)
                return gameSessionId;
            else
                return 0;
        }
    }
}
