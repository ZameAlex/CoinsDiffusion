using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Diffusion
{
    struct Transaction
    {
        public City receiver;
        public Dictionary<string, int> values;
    }
    
    public class Transition
    {
        public List<City> Cities { get; protected set; }
        public string[] Countries { get; set; }

        public Transition(List<City> cities,string[] countries)
        {
            Cities = cities;
            Countries = countries;
        }

        private bool CheckCountry(string country)
        {
            var countryCities = Cities.Where(t => t.Country == country);
            foreach(var item in countryCities)
            {
                foreach(var type in item.Coins)
                {
                    if (type.Value == 0)
                        return false;
                }
            }
            return true;
        }

        public Dictionary<string,int> CountDuration()
        {
            Dictionary<string, int> countriesDuration = new Dictionary<string, int>();
            foreach(var item in Cities)
            {
                foreach(var country in Countries)
                {
                    if (item.Country == country)
                        item.Coins.Add(country, 1000000);
                    else
                        item.Coins.Add(country, 0);
                    item.FillTransaction(country, 0);
                }
            }
            int count = 0;
            bool allCountriesCompleted = false;
            while(!allCountriesCompleted)
            {
                foreach(var city in Cities)
                {
                    city.PrepareTransaction();
                }
                foreach (var city in Cities)
                {
                    city.MakeTransaction();
                }
                allCountriesCompleted = true;
                foreach(var country in Countries)
                {
                    if (!CheckCountry(country))
                        allCountriesCompleted = false;
                    else
                    {
                        if(!countriesDuration.ContainsKey(country))
                        countriesDuration.Add(country, count+1);
                    }
                }
                count++;
            }
            return countriesDuration;
        }
    }
}
