using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuessTheCountry.Model
{
    public class Country
    {
        public Country(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName
        {
            get;
            private set;
        }
    }
}
