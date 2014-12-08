using BingMapsRESTService.Common.JSON;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GuessTheCountry.Model;
using System;
using System.Device.Location;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private const int minLatitude = -90;
        private const int maxLatitude = 90;

        private const int minLongitude = -180;
        private const int maxLongitude = 180;


        private readonly ICountryService _dataService;
        private ICommand sendAnswer;
        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string answer;



        private GeoCoordinate coordinate;

        public GeoCoordinate Coordinate
        {
            get { return coordinate; }
            set { 
                
                coordinate = value;


            RaisePropertyChanged("Coordinate");
            
            }
        }

        private readonly GeoCoordinate seattleGeoCoordinate = new GeoCoordinate(47.60097, -122.3331);
     

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged("Status");}
        }

        private string countryName = string.Empty;

        /// <summary>
        /// Gets the CountryName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                if (countryName == value)
                {
                    return;
                }

                countryName = value;

                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ICountryService dataService)
        {
            _dataService = dataService;

            setGeoCoordinate();

            Status = "Searching for location...";

        }

        private ICommand _addNumberCommand;

        public ICommand SendAnswer
        {
            get
            {
                if (_addNumberCommand == null)
                    _addNumberCommand = new RelayCommand<string>(i => CheckAnswer(answer));
                return _addNumberCommand;
            }
        }

        public void CheckAnswer(string answer)
        {
            if (answer.ToUpper() == countryName.ToUpper())
            {
                Status = "Correct!";
                Status = "Searching for location...";
                setGeoCoordinate();
            }
            else
            {
                Status = "Incorrect!";
            }
        }

        private GeoCoordinate getRandomCoordinates()
        {
            double latitude = Utils.GetRandomNumber(minLatitude, maxLatitude);
            double longitude = Utils.GetRandomNumber(minLongitude, maxLongitude);

            return new GeoCoordinate(latitude, longitude);
        }

        private void setGeoCoordinate()

        {
            string key = "At5gCPjlFX3X3_9vHk-_dCFUqFps-IKZMyh_mb7Al1CyS0fgzFRsETMF0S2Or6ou";

            Coordinate = getRandomCoordinates();

            Uri geocodeRequest = new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?key={2}", Coordinate.Latitude, Coordinate.Longitude, key));

            _dataService.GetResponse(geocodeRequest, (x) =>
            {
                if (x.ResourceSets[0].Resources.Length > 0)
                {
                    CountryName = ((Location)x.ResourceSets[0].Resources[0]).Address.CountryRegion;
                    Status = "Guess!";
                }
                else
                {
                    setGeoCoordinate();
                }

            });

        }

    }
}