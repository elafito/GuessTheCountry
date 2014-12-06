using BingMapsRESTService.Common.JSON;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GuessTheCountry.Model
{
    public  class CountryService : ICountryService
    {
        public void GetResponse(Uri uri, Action<Response> callback)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback != null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(a.Result) as Response);
                }
            };
            wc.OpenReadAsync(uri);
        }

    }
}