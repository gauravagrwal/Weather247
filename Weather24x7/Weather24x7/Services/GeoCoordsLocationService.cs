using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weather24x7.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(GeoCoordsLocationService))]
namespace Weather24x7.Services
{
    public class GeoCoordsLocationService : IGeoCoordsLocationService
    {
        private CancellationTokenSource _cts;

        public async Task<Location> GetCurrentLocationCoordinates()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cts = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(request, _cts.Token);

                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Debug.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Debug.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Debug.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine(ex);
            }
            return null;
        }

        public async Task<Placemark> GetCurrentLocation(double latitude, double longitude)
        {
            try
            {
                IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                Placemark placemark = placemarks.FirstOrDefault();
                if (placemark != null)
                {
                    return placemark;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
    }
}
