using System;
using GuessTheCountry.Model;
using BingMapsRESTService.Common.JSON;
using System.Threading.Tasks;
using System.Device.Location;
using Windows.Devices.Geolocation;

namespace GuessTheCountry.Design
{
    public class DesignDataService : IResponseService
    {

        public void GetCountryName(double latitude, double longitude, Action<Response> callback)
        {
            throw new NotImplementedException();
        }
    }
}