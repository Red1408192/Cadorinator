using Cadorinator.ServiceContract.Settings;
using Newtonsoft.Json;
using Serilog;
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

        public static async Task<ICadorinatorSettings> LoadAsync(ILogger serilog, string fileName = null)
        {
            try
            {
                _name = fileName ?? _name;
                ICadorinatorSettings settings;
                Directory.CreateDirectory(_path);

                if (File.Exists(_path + _name))
                {
                    serilog.Information($"{DateTime.Now} - settings file found");
                    using (var str = new StreamReader(_path + _name))
                    {
                        var raw = await str.ReadToEndAsync();
                        str.Close();
                        settings = JsonConvert.DeserializeObject<CadorinatorSettings>(raw);
                        if (settings != null)
                        {
                            settings.FilePath = Regex.Replace(settings.FilePath, @"%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                            serilog.Information($"{DateTime.Now} - settings loading completed");

                            return settings;
                        }
                    }
                }
                serilog.Warning($"{DateTime.Now} - [WARNING] Settings file not found - Default settings will be created instead");
                serilog.Warning($"{DateTime.Now} - [WARNING] Please update the settings file, no provider have been defined");
                await SaveAsync(new CadorinatorSettings());
                return await LoadAsync(serilog);
            }
            catch(Exception ex)
            {
                serilog.Error($"{DateTime.Now} - [EX] {ex.Message}");
                return default;
            }
        }

        public static async Task SaveAsync(this CadorinatorSettings settings)
        {
            await File.WriteAllTextAsync(_path + _name, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}
