using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApexTrackerConsoleApp.Models
{
    public class ApexUserInfoDTO
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("hardware")]
        public string Hardware { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("matches")]
        public int Matches { get; set; }

        [JsonProperty("banReason")]
        public string BanReason { get; set; }

        [JsonProperty("banSeconds")]
        public string BanSeconds { get; set; }

        [JsonProperty("eliteStreak")]
        public int EliteStreak { get; set; }

        [JsonProperty("rankScore")]
        public int RankScore { get; set; }

        [JsonProperty("charVer")]
        public string CharVer { get; set; }

        [JsonProperty("charIdx")]
        public string CharIdx { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("cdata0")]
        public string Cdata0 { get; set; }

        [JsonProperty("cdata1")]
        public string Cdata1 { get; set; }

        [JsonProperty("cdata2")]
        public string LegendName { get; set; }

        [JsonProperty("cdata3")]
        public string Cdata3 { get; set; }

        [JsonProperty("cdata4")]
        public string Cdata4 { get; set; }

        [JsonProperty("cdata5")]
        public string Cdata5 { get; set; }

        [JsonProperty("cdata6")]
        public string Cdata6 { get; set; }

        [JsonProperty("cdata7")]
        public string Cdata7 { get; set; }

        [JsonProperty("cdata8")]
        public string Cdata8 { get; set; }

        [JsonProperty("cdata9")]
        public string Cdata9 { get; set; }

        [JsonProperty("cdata10")]
        public string Cdata10 { get; set; }

        [JsonProperty("cdata11")]
        public string Cdata11 { get; set; }

        [JsonProperty("cdata12")]
        public string Tracker1Name { get; set; }

        [JsonProperty("cdata13")]
        public string Tracker1Value { get; set; }

        [JsonProperty("cdata14")]
        public string Tracker2Name { get; set; }

        [JsonProperty("cdata15")]
        public string Tracker2Value { get; set; }

        [JsonProperty("cdata16")]
        public string Tracker3Name { get; set; }

        [JsonProperty("cdata17")]
        public string Tracker3Value { get; set; }

        [JsonProperty("cdata18")]
        public string Cdata18 { get; set; }

        [JsonProperty("cdata19")]
        public string Cdata19 { get; set; }

        [JsonProperty("cdata20")]
        public string Cdata20 { get; set; }

        [JsonProperty("cdata21")]
        public string Cdata21 { get; set; }

        [JsonProperty("cdata22")]
        public string Cdata22 { get; set; }

        [JsonProperty("cdata23")]
        public int AccountLevel { get; set; }

        [JsonProperty("cdata24")]
        public string Cdata24 { get; set; }

        [JsonProperty("cdata25")]
        public string Cdata25 { get; set; }

        [JsonProperty("cdata26")]
        public string Cdata26 { get; set; }

        [JsonProperty("cdata27")]
        public string Cdata27 { get; set; }

        [JsonProperty("cdata28")]
        public string Cdata28 { get; set; }

        [JsonProperty("cdata29")]
        public string Cdata29 { get; set; }

        [JsonProperty("cdata30")]
        public string Cdata30 { get; set; }

        [JsonProperty("cdata31")]
        public string Cdata31 { get; set; }

        [JsonProperty("online")]
        public string Online { get; set; }

        [JsonProperty("joinable")]
        public string Joinable { get; set; }

        [JsonProperty("partyFull")]
        public string PartyFull { get; set; }

        [JsonProperty("partyInMatch")]
        public int PartyInMatch { get; set; }

        [JsonProperty("timeSinceServerChange")]
        public string TimeSinceServerChange { get; set; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }
    }
}
