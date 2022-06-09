using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Weather24x7.Services
{
    public interface IGeoCoordsLocationService
    {
        Task<Placemark> GetCurrentLocation(double latitude, double longitude);
        Task<Location> GetCurrentLocationCoordinates();
    }
}
