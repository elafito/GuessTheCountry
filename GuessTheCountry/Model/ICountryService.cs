using BingMapsRESTService.Common.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheCountry.Model
{
    public interface ICountryService
    {
        void GetResponse(Uri uri, Action<Response> callback);
    }
}
