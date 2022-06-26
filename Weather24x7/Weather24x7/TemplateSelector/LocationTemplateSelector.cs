using Weather24x7.Models;
using Weather24x7.Views.Templates;
using Xamarin.Forms;

namespace Weather24x7.TemplateSelector
{
    public class LocationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedLocationTemplate { get; set; }
        public DataTemplate LocationTemplate { get; set; }
        public DataTemplate AddLocationTemplate { get; set; }

        public LocationTemplateSelector()
        {
            SelectedLocationTemplate = new DataTemplate(typeof(SelectedLocationTemplate));
            LocationTemplate = new DataTemplate(typeof(LocationTemplate));
            AddLocationTemplate = new DataTemplate(typeof(AddLocationTemplate));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item.GetType() == typeof(LocationModel))
            {
                var locationItem = item as LocationModel;
                if (string.IsNullOrEmpty(locationItem.Country) && string.IsNullOrEmpty(locationItem.Locality))
                {
                    return AddLocationTemplate;
                }
                else if (locationItem.IsSelected)
                {
                    return SelectedLocationTemplate;
                }
                else
                {
                    return LocationTemplate;
                }
            }
            return LocationTemplate;
        }
    }
}
