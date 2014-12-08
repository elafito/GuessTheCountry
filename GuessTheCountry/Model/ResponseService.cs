using BingMapsRESTService.Common.JSON;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Device.Location;
using Windows.Devices.Geolocation;

namespace GuessTheCountry.Model
{
    /// <summary>
    /// Response class to manage REST Services.
    /// </summary>
    public class ResponseService : IResponseService
    {
        /// <summary>
        /// Key for using Bing Maps
        /// </summary>
        private const string key = "At5gCPjlFX3X3_9vHk-_dCFUqFps-IKZMyh_mb7Al1CyS0fgzFRsETMF0S2Or6ou";

        /// <summary>
        /// Dev url for Bing Maps REST Service
        /// </summary>
        private const string url = "http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?key={2}";

        /// <summary>
        /// Get the country name from the given coordinates.
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="callback">Callback</param>
        public void GetCountryName(double latitude, double longitude, Action<Response> callback)
        {
            // Build URI for the REST Service
            Uri geocodeRequest = new Uri(string.Format(url,latitude, longitude, key));

            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback != null)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(a.Result) as Response);
                }
            };

            //Get result
            wc.OpenReadAsync(geocodeRequest);
        }
    }
}