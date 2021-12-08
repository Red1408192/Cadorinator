using Cadorinator.ServiceContract.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadorinator.ServiceContract.Settings
{
    public static class Settings
    {
        private static string _name = "appsettings.json";
        private static string _path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Cadorinator\\";

        public static async Task<ICadorinatorSettings> LoadAsync(string fileName = null)
        {
            _name = fileName ?? _name;
            ICadorinatorSettings settings;
            if (File.Exists(_path + _name))
            {
                using(var str = new StreamReader(_path + _name))
                {
                    var raw = await str.ReadToEndAsync();
                    str.Close();
                    settings = JsonConvert.DeserializeObject<CadorinatorSettings>(raw);
                    if(settings != null)
                    {
                        settings.FilePath = Regex.Replace(settings.FilePath, @"%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                        return settings;
                    }
                }
            }
            Console.WriteLine("[WARNING] Settings file not found - Default settings will be created instead");
            Console.WriteLine("[WARNING] Please update the settings file, no provider have been defined");
            await SaveAsync(new CadorinatorSettings());
            return await LoadAsync();
        }

        public static async Task SaveAsync(this CadorinatorSettings settings)
        {
            await File.WriteAllTextAsync(_path + _name, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}
