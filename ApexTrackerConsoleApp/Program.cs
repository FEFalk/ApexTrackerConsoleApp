using ApexTrackerConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApexTrackerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*Fetch gamesession*/
            GameSession gameSession = new GameSession();
            GameSessionModel gameSessionModel = new GameSessionModel();
            GetPlayerData getPlayerData = new GetPlayerData();

            gameSessionModel = gameSession.GetGameSession();
            getPlayerData.BuildPlayerList(gameSessionModel);
            getPlayerData.GetPlayerOffsets(gameSessionModel);
            getPlayerData.BuildPlayerList(gameSessionModel);
            getPlayerData.Run(gameSessionModel);
            
        }
    }
}
