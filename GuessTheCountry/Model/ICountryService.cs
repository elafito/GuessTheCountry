using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuessTheCountry.Model
{
    public interface ICountryService
    {
        void GetCountryName(Action<Country, Exception> callback);
    }
}
