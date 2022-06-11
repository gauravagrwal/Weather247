namespace Weather24x7.Helpers
{
    public interface ISettingsHelper
    {
        (string endpoint, string key) LoadSettings();
    }
}
