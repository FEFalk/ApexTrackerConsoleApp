using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApexTrackerConsoleApp.Controllers;
using ApexTrackerConsoleApp.Models;

namespace ApexTrackerConsoleApp
{
    class GetPlayerData
    {
        DbConnection dbConnection = new DbConnection();
        string connection = @"Server=.\SQLEXPRESS;Database=ApexTrackerDb;Trusted_Connection=True";

        public List<GameSessionDataModel> gameSessionDataList = new List<GameSessionDataModel>();
        PlayerDto player = new PlayerDto();
        public void Run(GameSessionModel gameSession)
        {
            foreach (GameSessionDataModel player in gameSessionDataList)
            {
                var playerDataApexAPI = GetPlayer(player.Player.Name);
                Thread.Sleep(3000);
                player.Kills = (int)playerDataApexAPI.Result.Kills - player.OffsetKills;
                player.Top3 = (int)playerDataApexAPI.Result.Top3 - player.OffsetTop3;
                player.Wins = (int)playerDataApexAPI.Result.Wins - player.OffsetWins;               

                Console.WriteLine(player.Player.Name);
                Console.WriteLine("kills: " + player.Kills);
                Console.WriteLine("Top3 : " + player.Top3);
                Console.WriteLine("Wins : " + player.Wins);
            }
            UpdatePlayerStats(gameSession);
        }
        public void BuildPlayerList(GameSessionModel gameSession)
        {
            string sqlcommand = "SELECT UserName, PlayerId, GameSessionId, Kills, Top3, Wins, OffsetKills, OffsetTop3, OffsetWins " +
                     "FROM [ApexTrackerDb].[dbo].[GameSessionData], dbo.Player " +
                     "where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid " +
                     "and [dbo].[GameSessionData].[PlayerId] = dbo.Player.Id ";
            dbConnection.ConnectToDb(connection);
            dbConnection.Items = dbConnection.ReadPlayersFromDb(sqlcommand, gameSession);
            gameSessionDataList = new List<GameSessionDataModel>();
            foreach (Item item in dbConnection.Items)
            {
                GameSessionDataModel gameSessionData = new GameSessionDataModel();
                PlayerDto player = new PlayerDto();
                player.Name = item.UserName;
                gameSessionData.Player = player;
                gameSessionData.PlayerId = item.PlayerId;
                gameSessionData.GameSessionId = item.GameSessionId;
                gameSessionData.Kills = item.Kills;
                gameSessionData.Top3 = item.Top3;
                gameSessionData.Wins = item.Wins;
                gameSessionData.OffsetKills = item.OffsetKills;
                gameSessionData.OffsetTop3 = item.OffsetTop3;
                gameSessionData.OffsetWins = item.OffsetWins;
                this.gameSessionDataList.Add(gameSessionData);
            }
        }
        public void GetPlayerOffsets(GameSessionModel gameSession)
        {
            dbConnection.ConnectToDb(connection);
            foreach (GameSessionDataModel gameSessionData in gameSessionDataList)
            {
                var playerDataApexAPI = GetPlayer(gameSessionData.Player.Name);
                Thread.Sleep(3000);
                StringBuilder command = new StringBuilder();
                command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
                command.Append(" SET OffsetKills = @kills,");
                command.Append(" OffsetTop3 = @top3,");
                command.Append(" OffsetWins = @wins");
                command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
                command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");

                dbConnection.WriteToDb(command.ToString(), playerDataApexAPI, gameSessionData, gameSession);
            }
        }

        public void UpdatePlayerStats(GameSessionModel gameSession)
        {
            dbConnection.ConnectToDb(connection);
            foreach (GameSessionDataModel gameSessionData in gameSessionDataList)
            {
                var playerDataApexAPI = GetPlayer(gameSessionData.Player.Name);
                Thread.Sleep(3000);
                StringBuilder command = new StringBuilder();
                command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
                command.Append(" SET Kills = @kills,");
                command.Append(" Top3 = @top3,");
                command.Append(" Wins = @wins");
                command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
                command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");

                dbConnection.WriteToDb(command.ToString(), playerDataApexAPI, gameSessionData, gameSession);
            }
        }

        public async Task<PlayerDto> GetPlayer(string playerName)
        {
            ApexController apexController = new ApexController();
            this.player = await apexController.GetApexPlayerAPI(playerName);
            return this.player;
        }
    }
}
