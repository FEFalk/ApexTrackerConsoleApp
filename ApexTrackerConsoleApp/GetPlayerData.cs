using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApexTrackerConsoleApp.Controllers;
using ApexTrackerConsoleApp.Models;

namespace ApexTrackerConsoleApp
{
    class GetPlayerData
    {
        GameSessionData playerDataOffsets = new GameSessionData();
        PlayerDto player = new PlayerDto();
        bool isCalibrated = true;
        public void Run(List<GameSessionData> playerList)
        {
            foreach (GameSessionData player in playerList)
            {
                if (!isCalibrated)
                     playerDataOffsets = GetPlayerOffsets(player);

                var playerDataApexAPI = getPlayer(player.Player.Name);
                Thread.Sleep(3000);
                player.Kills = (int)playerDataApexAPI.Result.Kills - playerDataOffsets.OffsetKills;
                player.Top3 = (int)playerDataApexAPI.Result.Top3 - playerDataOffsets.OffsetTop3;
                player.Wins = (int)playerDataApexAPI.Result.Wins - playerDataOffsets.OffsetWins;
                //save to database

                Console.WriteLine(player.Player.Name);
                Console.WriteLine("kills: " + player.Kills);
                Console.WriteLine("Top3 : " + player.Top3);
                Console.WriteLine("Wins : " + player.Wins);
            }
        }

        public GameSessionData GetPlayerOffsets(GameSessionData gameSessionData)
        {
            var playerDataApexAPI = getPlayer(gameSessionData.Player.Name);
            Thread.Sleep(3000);
            if (playerDataApexAPI.Result.Kills > 0) gameSessionData.OffsetKills = (int)playerDataApexAPI.Result.Kills - 1;
            if (playerDataApexAPI.Result.Top3 > 0) gameSessionData.OffsetTop3 = (int)playerDataApexAPI.Result.Top3 - 1;
            if (playerDataApexAPI.Result.Wins > 0) gameSessionData.OffsetWins = (int)playerDataApexAPI.Result.Wins - 1;
            return gameSessionData;
        }

        public async Task<PlayerDto> getPlayer(string playerName)
        {
            ApexController apexController = new ApexController();
            this.player = await apexController.GetApexPlayerAPI(playerName);
            return this.player;
        }
    }
}
