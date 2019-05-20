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
            GameSessionDto GameSessionDto;
            Application application = new Application();
            int gameSessionId = 0;
            while (true)
            {
                while (gameSessionId == 0)
                {
                    gameSessionId = gameSession.GetGameSessionID();
                }
                Console.WriteLine("gamesession: " + gameSessionId);
                GameSessionDto = gameSession.GetGameSession(gameSessionId);
                if (gameSession.PlayersExist(GameSessionDto) == null)
                {
                    Console.WriteLine("Cancling gamesession.");
                    gameSession.CancelGameSession(GameSessionDto);
                    gameSessionId = 0;
                    continue;
                }

                while (GameSessionDto.EndTime >= DateTime.Now)
                {
                    if (GameSessionDto.StartTime <= DateTime.Now)
                        application.Run();
                    else
                    {
                        application.BuildPlayerList(GameSessionDto); //hämta playernames från db
                        application.CalibratePlayerList(); // hämta playerstats från api
                        application.BuildSquadList(GameSessionDto); //skapa squads utan stats               
                        application.CalibrateSquadList(); //updatera squads med stats tilldela trackers
                    }
                }
                gameSessionId = 0;
            }
        }
    }
}
