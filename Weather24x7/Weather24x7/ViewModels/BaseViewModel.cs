using System.ComponentModel;
using Xamarin.CommunityToolkit.UI.Views;

namespace Weather24x7.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LayoutState CurrentLayoutState { get; set; }

        public BaseViewModel() { }
    }
}
