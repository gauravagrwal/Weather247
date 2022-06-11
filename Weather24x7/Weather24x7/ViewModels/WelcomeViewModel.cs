using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather24x7.Helpers;
using Weather24x7.Models;
using Weather24x7.Services;
using Weather24x7.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather24x7.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        #region Commands
        public ICommand GetCurrentLocationCommand { get; set; }
        #endregion

        #region Properties
        public string CurrentLocation { get; set; }
        #endregion

        #region Helpers & Services
        private IGeoCoordsLocationService _geoCoordsLocationService;
        private ISettingsHelper _settingsHelper;
        #endregion

        public WelcomeViewModel()
        {
            _geoCoordsLocationService = DependencyService.Get<IGeoCoordsLocationService>();
            _settingsHelper = DependencyService.Get<ISettingsHelper>();

            GetCurrentLocationCommand = new Command(GetCurrentLocationCommandHandler);
        }

        #region Command Handlers
        private async void GetCurrentLocationCommandHandler()
        {
            try
            {
                var location = await _geoCoordsLocationService.GetCurrentLocationCoordinates();
                if (location != null)
                {
                    var placemark = await _geoCoordsLocationService.GetCurrentLocation(location.Latitude, location.Longitude);
                    await SaveLocation(location, placemark);
                    await LoadandSaveSettings();
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        #endregion

        #region Methods
        private async Task SaveLocation(Location locationCoords, Placemark placemark)
        {
            var location = new LocationModel
            {
                Latitude = locationCoords.Latitude,
                Longitude = locationCoords.Longitude,
                Locality = placemark.Locality,
                State = placemark.AdminArea,
                Country = placemark.CountryName,
                CountryCode = placemark.CountryCode,
                IsSelected = true
            };

            var listOfLocations = new List<LocationModel>();
            listOfLocations.Add(location);

            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(listOfLocations));
        }
        
        private async Task LoadandSaveSettings()
        {
            var (endpoint, key) = _settingsHelper.LoadSettings();
            await SecureStorage.SetAsync("weatherAPIendpoint", endpoint);
            await SecureStorage.SetAsync("weatherAPIkey", key);
        }
        #endregion
    }
}
