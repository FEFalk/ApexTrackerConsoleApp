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

            while (gameSessionId == 0)
            {
                gameSessionId = gameSession.GetGameSessionID();
                Thread.Sleep(1000);
            }
            gameSessionModel = gameSession.GetGameSession(gameSessionId);

            getPlayerData.BuildPlayerList(gameSessionModel);
            getPlayerData.GetPlayerOffsets(gameSessionModel);
            getPlayerData.BuildPlayerList(gameSessionModel);
            getPlayerData.Run(gameSessionModel);            
        }
    }
}
