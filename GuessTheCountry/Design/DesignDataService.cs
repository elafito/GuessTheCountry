using System;
using GuessTheCountry.Model;

namespace GuessTheCountry.Design
{
    public class DesignDataService : ICountryService
    {
        public void GetCountryName(Action<Country, Exception> callback)
        {
            // Use this to create design time data

            var item = new Country("Welcome to MVVM Light [design]");
            callback(item, null);
        }
    }
}