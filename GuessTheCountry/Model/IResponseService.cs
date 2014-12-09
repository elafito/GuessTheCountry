using BingMapsRESTService.Common.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GuessTheCountry.Model
{
    public interface IResponseService
    {
        void GetCountryName(double latitude, double longitude, Action<string> callback);
    }
}
