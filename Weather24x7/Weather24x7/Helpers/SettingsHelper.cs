using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Weather24x7.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsHelper))]
namespace Weather24x7.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        public (string endpoint, string key) LoadSettings()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resName = assembly.GetManifestResourceNames()?.FirstOrDefault(res => res.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            using Stream file = assembly.GetManifestResourceStream(resName);
            using StreamReader sr = new StreamReader(file);

            string json = sr.ReadToEnd();
            JObject j = JObject.Parse(json);

            string endpoint = j.Value<string>("openWeatherAPIEndpoint");
            string key = j.Value<string>("openWeatherAPIKey");

            return (endpoint, key);
        }
    }
}
