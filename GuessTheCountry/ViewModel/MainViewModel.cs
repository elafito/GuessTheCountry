using BingMapsRESTService.Common.JSON;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GuessTheCountry.Model;
using System;
using Windows.Devices.Geolocation;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Device.Location;

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
        #region private variables

        //Set minimum and maximum values for latitude
        private const int minLatitude = -90;
        private const int maxLatitude = 90;

        //Set minimum and maximum values for longitude
        private const int minLongitude = -180;
        private const int maxLongitude = 180;

        private readonly IResponseService dataService;
        private GeoCoordinate coordinate;
        private ICommand sendAnswer;
        private string answer;
        private string status;
        private string countryName = string.Empty;

        #endregion

        #region public properties

        public GeoCoordinate Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; RaisePropertyChanged("Coordinate");        
            }
        }   

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged("Status");}
        }

        #endregion

        private void CheckAnswer(string answer)
        {
            if (answer.ToUpper().Trim() == countryName.ToUpper().Trim())
            {
                Status = "Correct!";
                Status = "Searching for location...";
                getCountryNameFromRandomCoordinates();
            }
            else
            {
                Status = "Incorrect!";
            }
        }

        #region private methods

        /// <summary>
        /// Get country name from the random coordinates
        /// </summary>
        private void getCountryNameFromRandomCoordinates()
        {
            //Get random values
            Coordinate = getRandomCoordinates();

            //Ask for country name
            dataService.GetCountryName(coordinate.Latitude, coordinate.Longitude, (x) =>
            {
                //If there is a value for country name, set the property value CountryName.
                if (x.ResourceSets[0].Resources.Length > 0)
                {
                    CountryName = ((Location)x.ResourceSets[0].Resources[0]).Address.CountryRegion;
                    Status = "Guess!";
                }
                else //If there is no country name (pushpin ended in the sea), search again
                {
                    getCountryNameFromRandomCoordinates();
                }
            });
        }

        /// <summary>
        /// Get Random Coordinates having on count defined min and max values.
        /// </summary>
        /// <returns>Random coordinates</returns>
        private GeoCoordinate getRandomCoordinates()
        {
            double latitude = Utils.GetRandomNumber(minLatitude, maxLatitude);
            double longitude = Utils.GetRandomNumber(minLongitude, maxLongitude);

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion

        #region public methods

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

                //RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IResponseService dataService)
        {
            this.dataService = dataService;

            getCountryNameFromRandomCoordinates();

            Status = "Searching for location...";
        }
        public ICommand SendAnswer
        {
            get
            {
                if (sendAnswer == null)
                    sendAnswer = new RelayCommand<string>(i => CheckAnswer(answer));
                return sendAnswer;
            }
        }

        #endregion
    }
}