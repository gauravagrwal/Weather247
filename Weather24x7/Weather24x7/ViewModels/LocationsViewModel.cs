using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Weather24x7.Models;
using Weather24x7.Services;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Weather24x7.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        #region Private & Protected
        private readonly IGeoCoordsLocationService _geoCoordsLocationService;
        #endregion


        #region Properties
        public ObservableCollection<LocationModel> Locations { get; set; }
        #endregion


        #region Commands
        public Command AddLocationCommand { get; set; }
        public Command SelectLocationCommand { get; set; }
        public Command DeleteLocationCommand { get; set; }
        #endregion


        public LocationsViewModel()
        {
            _geoCoordsLocationService = DependencyService.Get<IGeoCoordsLocationService>();

            AddLocationCommand = new Command(AddLocationCommandHandler);
            SelectLocationCommand = new Command<string>(SelectLocationCommandHandler);
            DeleteLocationCommand = new Command<string>(DeleteLocationCommandHandler);

            Locations = new ObservableCollection<LocationModel>();
        }


        #region Command Handlers
        private void AddLocationCommandHandler()
        {
            throw new NotImplementedException();
        }

        private async void SelectLocationCommandHandler(string selectedLocality)
        {
            CurrentLayoutState = LayoutState.Loading;

            Locations.ForEach(l => l.IsSelected = false);
            Locations.First(l => l.Locality == selectedLocality).IsSelected = true;
            Locations.RemoveAt(Locations.Count - 1);

            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(Locations));
            await GetPlacemarkAndLocation();

            CurrentLayoutState = LayoutState.None;
        }

        private async void DeleteLocationCommandHandler(string selectedLocality)
        {
            CurrentLayoutState = LayoutState.Loading;

            if (Locations.Count > 2)
            {
                var item = Locations.First(l => l.Locality == selectedLocality);
                if (item.IsSelected)
                {
                    var index = Locations.IndexOf(item);
                    if (index < Locations.Count - 2)
                    {
                        Locations[index + 1].IsSelected = true;
                    }
                    else
                    {
                        Locations[0].IsSelected = true;
                    }
                }
                Locations.Remove(item);
                Locations.RemoveAt(Locations.Count - 1);

                await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(Locations));
                await GetPlacemarkAndLocation();
            }

            CurrentLayoutState = LayoutState.None;
        }
        #endregion


        #region Methods
        private async Task GetPlacemarkAndLocation()
        {
            try
            {
                Locations.Clear();

                var listLocJson = await SecureStorage.GetAsync("locations");
                var locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);

                locations.ForEach(l => Locations.Add(l));
                Locations.Add(new LocationModel());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        #endregion
    }
}
