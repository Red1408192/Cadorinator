using Cadorinator.Infrastructure;
using Cadorinator.ServiceContract.Settings;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cadorinator.Service
{
    public static class ProvidersHelper
    {
        private static readonly string filename = "Providers.csv";
        public static async Task SyncAsync(IDALService dalService, ICadorinatorSettings cadorinatorSettings)
        {
            if(FileSystem.FileExists(cadorinatorSettings.FilePath + filename))
            {
                using (TextFieldParser parser = new TextFieldParser(cadorinatorSettings.FilePath + filename))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(cadorinatorSettings.CSVSeparator);
                    parser.ReadFields(); //Remove the first raw
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Length < 4) continue;
                        await dalService.UpselectProvider(fields[0], fields[1], fields[2], fields[3]);
                    }
                }
            }
            else
            {
                var providers = await dalService.ListProvidersFullAsync();
                var file = new StringBuilder();
                file.AppendJoin(cadorinatorSettings.CSVSeparator, "ProviderDomain", "ProviderName", "ProviderSource", "City");
                file.AppendLine();
                foreach (var prov in providers)
                {
                    file.AppendJoin(cadorinatorSettings.CSVSeparator, prov.ProviderDomain, prov.ProviderName, prov.ProviderSourceNavigation.ProviderSourceName, prov.CityNavigation.CityName);
                    file.AppendLine();
                }
                File.WriteAllText(cadorinatorSettings.FilePath + filename, file.ToString());
            }
        }
    }
}
