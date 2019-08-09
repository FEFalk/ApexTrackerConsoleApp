using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using ApexTrackerConsoleApp.Models;
using System.Threading;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using ApexTrackerConsoleApp.Helpers;

namespace ApexTrackerConsoleApp.Controllers
{
    public static class ApexController
    {
        public static async Task<ApexUserInfoDTO> GetUserInfoByPlatformAndId(string platform, string userId)
        {
            Uri EndpointURL = new Uri("https://R5-pc.stryder.respawn.com/user.php?qt=user-getinfo&hardware="+ platform + "&uid=" + userId);
            try
            {
                var cookieContainer = new CookieContainer();
                HttpClientHandler httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };
                httpClientHandler.AllowAutoRedirect = false;

                using (HttpClient client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = EndpointURL;

                    var httpRequestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = EndpointURL,
                        Headers =
                        {
                            { "User-Agent", "Respawn HTTPS/1.0" }
                        }
                    };

                    HttpResponseMessage res = await client.SendAsync(httpRequestMessage);
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var contents = await res.Content.ReadAsStringAsync();

                        contents = contents.Insert(0, "{");
                        contents = contents.Insert(contents.Length, "}");

                        ApexAPIResponse<ApexUserInfoDTO> response = JsonConvert.DeserializeObject<ApexAPIResponse<ApexUserInfoDTO>>(contents);

                        return response.data;
                    }
                    else
                    {
                        Console.WriteLine("Could not get Player info.");
                    }
                }
            }

            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(Application.errorFilePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine(DateTime.Now.ToString() + ": ");
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Error: " + ex.Message);
                        writer.WriteLine("StackTrace: " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            return null;
        }




        public static async Task<PlayerDto> GetApexPlayerAPI(string userId, string legendName, int platform)
        {
            PlayerDto player = new PlayerDto();

            ApexUserInfoDTO response = await GetUserInfoByPlatformAndId("PC", userId);

            if (response != null)
            {
                int Kills = 0;
                int Top3 = 0;
                int Wins = 0;
                ApexResponseTranslator translator = new ApexResponseTranslator();
                string result;

                string LegendName = "";

                if (translator.ApexResponseDict.TryGetValue(response.LegendName, out result))
                    LegendName = result;

                if (LegendName != legendName)
                    return player;

                response.Tracker1Value = response.Tracker1Value == "2" ? "0" : response.Tracker1Value.Substring(0, response.Tracker1Value.Length - 2);
                response.Tracker2Value = response.Tracker2Value == "2" ? "0" : response.Tracker2Value.Substring(0, response.Tracker2Value.Length - 2);
                response.Tracker3Value = response.Tracker3Value == "2" ? "0" : response.Tracker3Value.Substring(0, response.Tracker3Value.Length - 2);



                if (translator.ApexResponseDict.TryGetValue(response.Tracker1Name, out result))
                {
                    if(result == "Kills") Kills = Int32.Parse(response.Tracker1Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker1Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker1Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.Tracker2Name, out result))
                {
                    if (result == "Kills") Kills = Int32.Parse(response.Tracker2Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker2Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker2Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.Tracker3Name, out result))
                {
                    if (result == "Kills") Kills = Int32.Parse(response.Tracker3Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker3Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker3Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.LegendName, out result))
                    LegendName = result;

                player = new PlayerDto
                {
                    Name = response.Name,
                    UserId = response.Uid,
                    Level = response.AccountLevel,
                    LegendName = LegendName,
                    Kills = Kills,
                    Wins = Wins,
                    Top3 = Top3,
                    RankScore = response.RankScore,
                    InMatch = response.PartyInMatch == 1 ? true : false
                };
            }
            return player;
        }
        public static async Task<PlayerDto> GetApexPlayerOffsetsAPI(string userId, int platform)
        {
            PlayerDto player = new PlayerDto();

            ApexUserInfoDTO response = await GetUserInfoByPlatformAndId("PC", userId);

            if (response != null)
            {
                response.Tracker1Value = response.Tracker1Value == "2" ? "0" : response.Tracker1Value.Substring(0, response.Tracker1Value.Length - 2);
                response.Tracker2Value = response.Tracker2Value == "2" ? "0" : response.Tracker2Value.Substring(0, response.Tracker2Value.Length - 2);
                response.Tracker3Value = response.Tracker3Value == "2" ? "0" : response.Tracker3Value.Substring(0, response.Tracker3Value.Length - 2);

                int Kills = -1;
                int Top3 = -1;
                int Wins = -1;
                string LegendName = "";
                ApexResponseTranslator translator = new ApexResponseTranslator();

                string result;
                if (translator.ApexResponseDict.TryGetValue(response.Tracker1Name, out result))
                {
                    if (result == "Kills") Kills = Int32.Parse(response.Tracker1Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker1Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker1Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.Tracker2Name, out result))
                {
                    if (result == "Kills") Kills = Int32.Parse(response.Tracker2Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker2Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker2Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.Tracker3Name, out result))
                {
                    if (result == "Kills") Kills = Int32.Parse(response.Tracker3Value);
                    else if (result == "Top3") Top3 = Int32.Parse(response.Tracker3Value);
                    else if (result == "Wins") Wins = Int32.Parse(response.Tracker3Value);
                }
                if (translator.ApexResponseDict.TryGetValue(response.LegendName, out result))
                    LegendName = result;

                player = new PlayerDto
                {
                    Name = response.Name,
                    UserId = response.Uid,
                    Level = response.AccountLevel,
                    LegendName = LegendName,
                    Kills = Kills,
                    Wins = Wins,
                    Top3 = Top3,
                    RankScore = response.RankScore,
                    InMatch = response.PartyInMatch == 1 ? true : false
                };
            }
            return player;
        }
    }
}