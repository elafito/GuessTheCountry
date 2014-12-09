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
using System.Windows.Threading;
using System.Windows;

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
        private GeoCoordinate pushPinCoordinate;
        private GeoCoordinate mapCenterCoordinate;
        private ICommand sendAnswer;
        private string answer;
        private string status;
        private string countryName = string.Empty;
        private DispatcherTimer dispatcherTimer;
        private bool appIsEnabled = true;
        private Visibility barVisibility;

        #endregion

        #region public properties

        /// <summary>
        /// Coordinates for the map pushpin
        /// </summary>
        public GeoCoordinate PushPinCoordinate
        {
            get { return pushPinCoordinate; }
            set
            {
                pushPinCoordinate = value; RaisePropertyChanged("PushPinCoordinate");        
            }
        }

        /// <summary>
        /// Coordinates for the map center
        /// </summary>
        public GeoCoordinate MapCenterCoordinate
        {
            get { return mapCenterCoordinate; }
            set
            {
                mapCenterCoordinate = value; RaisePropertyChanged("MapCenterCoordinate");
            }
        }   

        /// <summary>
        /// User answer
        /// </summary>
        public string Answer
        {
            get { return answer; }
            set { answer = value; RaisePropertyChanged("Answer"); }
        }

        /// <summary>
        /// Status message
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged("Status");}
        }

        /// <summary>
        /// Enable/Disable UI elements
        /// </summary>
        public bool AppIsEnabled
        {
            get { return appIsEnabled; }
            set { appIsEnabled = value; RaisePropertyChanged("AppIsEnabled");}
        }

        /// <summary>
        /// Show/Hide progressbar
        /// </summary>
        public Visibility BarVisibility
        {
            get { return barVisibility; }
            set { barVisibility = value; ; RaisePropertyChanged("BarVisibility"); }
        }

        /// <summary>
        /// Country name of pushpin location
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
            }
        }

        #endregion

        /// <summary>
        /// Gets the CountryName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        private void CheckAnswer(string answer)
        {
            try
            {
                // If answer is correct search for new location
                if (answer.ToUpper().Trim() == countryName.ToUpper().Trim())
                {
                    Status = "Correct!";
                    AppIsEnabled = false;
                    dispatcherTimer = new DispatcherTimer();
                    dispatcherTimer.Interval = TimeSpan.FromMilliseconds(2000);
                    dispatcherTimer.Tick += (s, e) =>
                        {
                            dispatcherTimer.Stop();
                            getCountryNameFromRandomCoordinates();
                            searchMode(true);
                        };
                    dispatcherTimer.Start();
                }
                else
                {
                    Status = "Incorrect!";
                }
            }
            catch (Exception)
            {
                //TODO LOG ERROR AND/OR SEND TO DEVELOPER 
            }
        }

        #region public methods

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IResponseService dataService)
        {
            try
            {
                this.dataService = dataService;
                searchMode(true);
                MapCenterCoordinate = new GeoCoordinate(Utils.GetRandomNumber(minLatitude, maxLatitude), Utils.GetRandomNumber(minLongitude, maxLongitude));
                getCountryNameFromRandomCoordinates();

            }
            catch (Exception)
            {
                //TODO LOG ERROR AND/OR SEND TO DEVELOPER 
            }
        }

        /// <summary>
        /// Checks answer
        /// </summary>
        public ICommand SendAnswer
        {
            get
            {
                if (sendAnswer == null)
                {
                    //Check if answer is correct
                    sendAnswer = new RelayCommand<string>(i => CheckAnswer(answer));
                }

                return sendAnswer;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Get country name from the random coordinates
        /// </summary>
        private void getCountryNameFromRandomCoordinates()
        {
            //Get random values
            PushPinCoordinate = getRandomCoordinates();

            //Ask for country name
            dataService.GetCountryName(pushPinCoordinate.Latitude, pushPinCoordinate.Longitude, (countryNameResponse) =>
            {
                //If there is a value for country name, set the property value CountryName.
                if (countryNameResponse != String.Empty)
                {
                    MapCenterCoordinate = PushPinCoordinate;
                    CountryName = countryNameResponse;
                    searchMode(false);

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

        /// <summary>
        /// Make changes in the UI depending if the app is searching for a new location
        /// </summary>
        /// <param name="activate">Activate/Deactivate search mode</param>
        private void searchMode (bool activate)
        {
            if (activate)
            {
                Status = "Searching...";
                AppIsEnabled = false;
                BarVisibility = Visibility.Visible;
            }
            else
            {
                Status = "Guess!";
                AppIsEnabled = true;
                Answer = string.Empty;
                BarVisibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}