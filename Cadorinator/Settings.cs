using Cadorinator.ServiceContract.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cadorinator.Service
{
    public static class Settings
    {
        private static string _name = "appsettings.json";

        public static async Task<ICadorinatorSettings> LoadAsync(string fileName = null)
        {
            _name = fileName ?? _name;
            ICadorinatorSettings settings;
            if (File.Exists($"./{_name}"))
            {
                using(var str = new StreamReader($"./{_name}"))
                {
                    var raw = await str.ReadToEndAsync();
                    str.Close();
                    settings = JsonConvert.DeserializeObject<CadorinatorSettings>(raw);
                    if(settings != null)
                    {
                        return settings;
                    }
                }
            }
            Console.WriteLine("[WARNING] Settings file not found - Default settings will be created instead");
            Console.WriteLine("[WARNING] Please update the settings file, no provider have been defined");
            await SaveAsync(new CadorinatorSettings());
            throw new Exception("Missing appsettings");
        }

        public static async Task SaveAsync(this CadorinatorSettings settings)
        {
            await File.WriteAllTextAsync($"./{_name}", JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}
