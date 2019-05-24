using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using ApexTrackerConsoleApp.Models;
using System.Threading;

namespace ApexTrackerConsoleApp.Controllers
{
    public static class ApexController
    {
        public static async Task<PlayerDto> GetApexPlayerAPI(string name, string legendName)
        {
            PlayerDto player = new PlayerDto();
            legendName = legendName.ToLower();
            try
            {
                
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://public-api.tracker.gg/apex/v1/standard/profile/5/");
                    client.DefaultRequestHeaders.Add("TRN-API-Key", "6922b3b5-c27c-4088-9bf2-2642dd117657");

                    HttpResponseMessage res = await client.GetAsync(name);
                    if (res.IsSuccessStatusCode)
                    {
                        // Get the JSON data
                        string json = res.Content.ReadAsStringAsync()?.Result;

                        // Deserialize the data
                        ApexAPIResponse<ApexAPIData> response = JsonConvert.DeserializeObject<ApexAPIResponse<ApexAPIData>>(json);

                        player = new PlayerDto
                        {
                            Name = response.data.MetadataDto.UserName,
                            Level = response.data.MetadataDto.Level,
                            Icon = response.data.LegendsDto.FirstOrDefault() != null ? response.data.LegendsDto.FirstOrDefault().LegendMetadataDto.LegendName.ToLower() : "bangalore",
                            BGImage = response.data.LegendsDto.FirstOrDefault() != null ? response.data.LegendsDto.FirstOrDefault().LegendMetadataDto.BackgroundURL.ToString() : "https://trackercdn.com/cdn/apex.tracker.gg/legends/bangalore-concept-bg-small.jpg",
                            Kills = response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "Kills") != null ? response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "Kills").Value : 0,
                            Wins = response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "SeasonWins") != null ? response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "SeasonWins").Value : 0,
                            Top3 = response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "TimesPlacedtop3") != null ? response.data.LegendsDto.Find(x => x.LegendMetadataDto.LegendName.ToLower() == legendName).Stats.Find(x => x.MetadataDto.Key == "TimesPlacedtop3").Value : 0
                        };
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return player;
        }
        public static async Task<PlayerDto> GetApexPlayerOffsetsAPI(string name)
        {
            PlayerDto player = new PlayerDto();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://public-api.tracker.gg/apex/v1/standard/profile/5/");
                    client.DefaultRequestHeaders.Add("TRN-API-Key", "6922b3b5-c27c-4088-9bf2-2642dd117657");

                    HttpResponseMessage res = await client.GetAsync(name);
                    if (res.IsSuccessStatusCode)
                    {
                        // Get the JSON data
                        string json = res.Content.ReadAsStringAsync()?.Result;

                        // Deserialize the data
                        ApexAPIResponse<ApexAPIData> response = JsonConvert.DeserializeObject<ApexAPIResponse<ApexAPIData>>(json);

                        player = new PlayerDto
                        {
                            Name = response.data.MetadataDto.UserName,
                            Level = response.data.MetadataDto.Level,
                            Icon = response.data.LegendsDto.FirstOrDefault() != null ? response.data.LegendsDto.FirstOrDefault().LegendMetadataDto.LegendName.ToLower() : "bangalore",
                            BGImage = response.data.LegendsDto.FirstOrDefault() != null ? response.data.LegendsDto.FirstOrDefault().LegendMetadataDto.BackgroundURL.ToString() : "https://trackercdn.com/cdn/apex.tracker.gg/legends/bangalore-concept-bg-small.jpg",
                            Kills = response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "Kills") != null ? response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "Kills").Value : -1,
                            Wins = response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "SeasonWins") != null ? response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "SeasonWins").Value : -1,
                            Top3 = response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "TimesPlacedtop3") != null ? response.data.LegendsDto[0].Stats.Find(x => x.MetadataDto.Key == "TimesPlacedtop3").Value : -1
                        };
                    }
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
            }
            return player;
        }
    }
}