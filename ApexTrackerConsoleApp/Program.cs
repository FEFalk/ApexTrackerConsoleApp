using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace ApexTrackerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GameSession gameSession = new GameSession();
            GameSessionModel gameSessionModel;
            GetPlayerData getPlayerData = new GetPlayerData();
            int gameSessionId = 0;
            while (true)
            {
                while (gameSessionId == 0)
                {
                    gameSessionId = gameSession.GetGameSessionID();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("gamesession: " + gameSessionId);
                gameSessionModel = gameSession.GetGameSession(gameSessionId);
                if (gameSession.PlayersExist(gameSessionModel) == null)
                {
                    Console.WriteLine("Cancling gamesession.");
                    gameSession.CancelGameSession(gameSessionModel);
                    gameSessionId = 0;
                    continue;
                }

                getPlayerData.BuildPlayerList(gameSessionModel);
                getPlayerData.SetPlayerOffsets();
                getPlayerData.BuildPlayerList(gameSessionModel);

                while (gameSessionModel.EndTime >= DateTime.Now)
                {
                    if (gameSessionModel.StartTime <= DateTime.Now)
                        getPlayerData.Run();
                }
                gameSessionId = 0;
            }
        }
    }
}
