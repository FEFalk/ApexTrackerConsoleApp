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

        public GameSessionDto gameSession = new GameSessionDto();
        public GameSessionDto GetGameSession(int gameSessionId)
        {

            dbConnection.ConnectToDb(connection);
            dbConnection.SetCommandReadGameSessionFromDb();
            dbConnection.Items = new List<Item>();
            dbConnection.Items = dbConnection.ReadGameSessionFromDb(gameSessionId);

            gameSession = new GameSessionDto();
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
            dbConnection.SetCommandReadActiveGameSessionFromDb();
            gameSessionId = dbConnection.ReadActiveSessionFromDb();
            if (gameSessionId != 0)
                return gameSessionId;
            else
                return 0;
        }

        public List<Item> PlayersExist(GameSessionDto gameSession)
        {
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = new List<Item>();
            dbConnection.SetCommandReadPlayersFromDb();
            dbConnection.Items = dbConnection.ReadPlayersFromDb(gameSession);
            if (dbConnection.Items.Count > 0) return (dbConnection.Items);
            else return null;
        }
        public void CancelGameSession(GameSessionDto gameSession)
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
