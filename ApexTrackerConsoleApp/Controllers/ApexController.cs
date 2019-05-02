using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using ApexTrackerConsoleApp.Models;

namespace ApexTrackerConsoleApp.Controllers
{
    public class ApexController
    {
        public async Task<PlayerDto> GetApexPlayerAPI(string name)
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
                            Name = response.data.Metadata.UserName,
                            Level = response.data.Metadata.Level,
                            Icon = response.data.Legends.FirstOrDefault() != null ? response.data.Legends.FirstOrDefault().LegendMetadata.LegendName.ToLower() : "bangalore",
                            BGImage = response.data.Legends.FirstOrDefault() != null ? response.data.Legends.FirstOrDefault().LegendMetadata.BackgroundURL.ToString() : "https://trackercdn.com/cdn/apex.tracker.gg/legends/bangalore-concept-bg-small.jpg",
                            Kills = response.data.Stats.Find(x => x.Metadata.Key == "Kills") != null ? response.data.Stats.Find(x => x.Metadata.Key == "Kills").Value : 0,
                            Wins = response.data.Stats.Find(x => x.Metadata.Key == "SeasonWins") != null ? response.data.Stats.Find(x => x.Metadata.Key == "SeasonWins").Value : 0,
                            Top3 = response.data.Stats.Find(x => x.Metadata.Key == "TimesPlacedtop3") != null ? response.data.Stats.Find(x => x.Metadata.Key == "TimesPlacedtop3").Value : 0
                        };                       
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return player;
        }
    }
}