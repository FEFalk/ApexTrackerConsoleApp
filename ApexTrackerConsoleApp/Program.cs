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
                    Thread.Sleep(1000);
                }
                Console.WriteLine("gamesession: " + gameSessionId);
                GameSessionDto = gameSession.GetGameSession(gameSessionId);
                if (gameSession.PlayersExist(GameSessionDto) == null)
                {
                    Console.WriteLine("Canceling gamesession.");
                    gameSession.CancelGameSession(GameSessionDto);
                    gameSessionId = 0;
                    continue;
                }

                while (GameSessionDto.EndTime >= DateTime.Now && !GameSessionDto.Canceled)
                {
                    if (GameSessionDto.StartTime <= DateTime.Now)
                    {
                        // If console app started in the middle of a gamesession => initialize the playerlist
                        if (application.playerList.Count == 0)
                            application.BuildPlayerList(GameSessionDto); //hämta playernames från db
                        application.Run(GameSessionDto);
                    }
                    else
                    {
                        if (application.playerList.Count == 0)
                        {
                            application.BuildPlayerList(GameSessionDto); //hämta playernames från db
                            application.BuildSquadList(GameSessionDto); //skapa squads med tillhörande spelare (utan trackervalidering)     
                        }

                        application.CalibratePlayerList(); // hämta playerstats från api
                        application.ValidateSquads(); //updatera squads trackers i programmet och db
                        Console.WriteLine("squads: " + application.squadList.Count);
                        if(GameSessionDto.StartTime.AddMinutes(-2) <= DateTime.Now)
                        {
                            if(application.squadsToRemove.Count > 0)
                                application.RemoveIncorrectSquads(); //kicka alla squads som inte har en wins- och en top3 tracker
                        }

                    }
                    // Wait 1 sec so that we don't spam the database with buildplayerlist etc when playerlist is empty
                    Thread.Sleep(1000);
                    // Check if gamesession has been canceled, so that we are not stuck in loop until endtime passed.
                    GameSessionDto = gameSession.GetGameSession(GameSessionDto.Id);
                    if (GameSessionDto.Canceled)
                        Console.WriteLine("GameSession has canceled. Searching for new Active GameSession...");
                }
                gameSessionId = 0;
            }
        }
    }
}
