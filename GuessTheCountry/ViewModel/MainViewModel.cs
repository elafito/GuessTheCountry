using BingMapsRESTService.Common.JSON;
using GalaSoft.MvvmLight;
using GuessTheCountry.Model;
using System;

namespace GuessTheCountry.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ICountryService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _countryName = string.Empty;

        /// <summary>
        /// Gets the CountryName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CountryName
        {
            get
            {
                return _countryName;
            }

            set
            {
                if (_countryName == value)
                {
                    return;
                }

                _countryName = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ICountryService dataService)
        {
            _dataService = dataService;

            string key = "At5gCPjlFX3X3_9vHk-_dCFUqFps-IKZMyh_mb7Al1CyS0fgzFRsETMF0S2Or6ou";
            string query = "47.64054,-122.12934";

            Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0}?key={1}", query, key));

            _dataService.GetResponse(geocodeRequest, (x) =>
            {
                CountryName = ((Location)x.ResourceSets[0].Resources[0]).Address.CountryRegion;;
            });
        }

    }
}