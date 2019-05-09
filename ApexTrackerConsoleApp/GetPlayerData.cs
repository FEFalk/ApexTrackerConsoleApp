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
        public void Run()
        {
            foreach (GameSessionDataModel player in gameSessionDataList)
            {
                string legendName = dbConnection.GetLegendNameFromDb(player.LegendId);
                var playerDataApexAPI = GetPlayer(player.Player.Name, legendName);
                Thread.Sleep(3000);
                UpdatePlayerStats(player, playerDataApexAPI);
                
                //Console.WriteLine(player.Player.Name);
                //Console.WriteLine("kills: " + player.Kills);
                //Console.WriteLine("Top3 : " + player.Top3);
                //Console.WriteLine("Wins : " + player.Wins);
            }
            
        }
        public void UpdatePlayerStats(GameSessionDataModel player, Task<PlayerDto> playerDataApexAPI)
        {
            dbConnection.ConnectToDb(connection);

            StringBuilder command = new StringBuilder();
            command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
            command.Append(" SET Kills = @kills,");
            command.Append(" Top3 = @top3,");
            command.Append(" Wins = @wins");
            command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
            command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");
            command.Append(" and [dbo].[GameSessionData].[LegendId] = @legendid");

            dbConnection.WriteToDb(command.ToString(), player, playerDataApexAPI);
        }
        public void SetPlayerOffsets()
        {
            dbConnection.ConnectToDb(connection);
            foreach (GameSessionDataModel gameSessionData in gameSessionDataList)
            {
                var playerDataApexAPI = GetPlayerOffsets(gameSessionData.Player.Name);
                gameSessionData.LegendId = dbConnection.GetLegendIdFromDb(playerDataApexAPI.Result.Icon);
                Thread.Sleep(3000);
                StringBuilder command = new StringBuilder();
                command.Append("UPDATE [ApexTrackerDb].[dbo].[GameSessionData]");
                command.Append(" SET OffsetKills = @kills,");
                command.Append(" OffsetTop3 = @top3,");
                command.Append(" OffsetWins = @wins,");
                command.Append(" LegendId = @legendId"); //(select Id from [dbo].[Legend] where Name = @legend)
                command.Append(" where [dbo].[GameSessionData].[GameSessionId] = @gamesessionid");
                command.Append(" and [dbo].[GameSessionData].[PlayerId] = @playerid");

                dbConnection.WriteToDb(command.ToString(), gameSessionData, playerDataApexAPI);
            }
        }
        public void BuildPlayerList(GameSessionModel gameSession)
        {
            string sqlcommand = "SELECT UserName, PlayerId, GameSessionId, LegendId, Kills, Top3, Wins, OffsetKills, OffsetTop3, OffsetWins " +
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
                gameSessionData.LegendId = item.LegendId;
                gameSessionData.Kills = item.Kills;
                gameSessionData.Top3 = item.Top3;
                gameSessionData.Wins = item.Wins;
                gameSessionData.OffsetKills = item.OffsetKills;
                gameSessionData.OffsetTop3 = item.OffsetTop3;
                gameSessionData.OffsetWins = item.OffsetWins;
                gameSessionDataList.Add(gameSessionData);
            }
        }

        public async Task<PlayerDto> GetPlayer(string playerName, string legendName)
        {
            ApexController apexController = new ApexController();
            this.player = await apexController.GetApexPlayerAPI(playerName, legendName);
            return this.player;
        }
        public async Task<PlayerDto> GetPlayerOffsets(string playerName)
        {
            ApexController apexController = new ApexController();
            this.player = await apexController.GetApexPlayerOffsetsAPI(playerName);
            return this.player;
        }
    }
}
