using System;
using System.Collections.Generic;
using System.Text;

namespace ApexTrackerConsoleApp.Helpers
{
    public class ApexResponseTranslator
    {
        public IDictionary<string, string> ApexResponseDict { get; set; }
        public ApexResponseTranslator()
        {
            ApexResponseDict = new Dictionary<string, string>();

            // Legends
            ApexResponseDict.Add(new KeyValuePair<string, string>("1111853120", "Caustic"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1409694078", "Lifeline"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1464849662", "Pathfinder"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("182221730", "Gibraltar"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("2045656322", "Mirage"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("725342087", "Bangalore"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("827049897", "Wraith"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("898565421", "Bloodhound"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("843405508", "Octane"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("187386164", "Wattson"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("80232848", "Crypto"));



            // _Legend Stats_
            //Caustic
            ApexResponseDict.Add(new KeyValuePair<string, string>("15753331", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1039353772", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("628023316", "Wins"));

            //Bangalore
            ApexResponseDict.Add(new KeyValuePair<string, string>("1814522143", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("486927953", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1872592848", "Wins"));
            
            //Bloodhound
            ApexResponseDict.Add(new KeyValuePair<string, string>("1049917798", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("59904350", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1413070446", "Wins"));

            //Gibraltar
            ApexResponseDict.Add(new KeyValuePair<string, string>("345008354", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("299529402", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1082828160", "Wins"));
            
            //Lifeline
            ApexResponseDict.Add(new KeyValuePair<string, string>("1509839340", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("692891435", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1740545206", "Wins"));
            
            //Mirage
            ApexResponseDict.Add(new KeyValuePair<string, string>("1730527550", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1996756430", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1500066309", "Wins"));

            //Pathfinder
            ApexResponseDict.Add(new KeyValuePair<string, string>("196161681", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("704679779", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1358938113", "Wins"));
            
            //Wraith
            ApexResponseDict.Add(new KeyValuePair<string, string>("1618935778", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1974771594", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("2023347527", "Wins"));
            
            //Octane
            ApexResponseDict.Add(new KeyValuePair<string, string>("303788636", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1853728991", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("290334183", "Wins"));

            //Wattson
            ApexResponseDict.Add(new KeyValuePair<string, string>("1449585426", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("1013733932", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("706925895", "Wins"));

            //Crypto
            ApexResponseDict.Add(new KeyValuePair<string, string>("1154431005", "Kills"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("767362001", "Top3"));
            ApexResponseDict.Add(new KeyValuePair<string, string>("276577930", "Wins"));



        }
    }
}
