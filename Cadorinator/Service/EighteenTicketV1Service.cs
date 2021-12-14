using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadorinator.Service.Service
{
    public class EighteenTicketV1Service : IProviderService
    {
        public int ProviderSourceId => 1;

        private readonly IDALService _cadorinatorService;
        private readonly ICadorinatorSettings _settings;

        public EighteenTicketV1Service(IDALService service, ICadorinatorSettings settings)
        {
            _cadorinatorService = service;
            _settings = settings;
        }

        public async Task RegisterSchedules(Provider source)
        {
            int outertries = 0;

            Request1:
                await Task.Delay(_settings.DefaultDelay * outertries);

                outertries++;
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/");
                if (htmlDoc == null) return;
                if (htmlDoc.ParsedText.Length < 80 && outertries < 10) goto Request1;

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'movie movie--preview release border border-light')]");

            foreach (var node in nodes)
            {
                var filmTitle = node.SelectSingleNode(".//*[contains(@class,'movie__title')]")?.InnerText.Replace("\n", "");
                if (filmTitle == null) continue;
                var timeSelector = node.SelectSingleNode(".//*[contains(@class,'time-select')]");
                if (timeSelector == null) continue;
                var filmId = Regex.Match(timeSelector.Attributes["id"]?.Value, "[0-9]{4,}(?!-)");
                if (filmId == null) continue;

                int innertries = 0;
                Request2:
                    innertries++;
                    await Task.Delay(_settings.DefaultDelay * innertries);
                    var innerPage = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/film/{filmId}");
                    if (innerPage == null) continue;
                    if(innerPage.ParsedText.Length < 80 && innertries < 10) goto Request2;

                var results = innerPage.DocumentNode.SelectNodes("//*[contains(@class,'film-projection smooth-scroller')]");
                if (results == null) continue;

                foreach(var innerNode in results)
                {
                    if (DateTime.TryParse(innerNode.Attributes["data-date"]?.Value, out var date))
                    {
                        var utcDate = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"), TimeZoneInfo.FindSystemTimeZoneById("UTC"));

                        var film = await _cadorinatorService.UpselectFilm(filmTitle, date.ToUniversalTime());
                        var projectionId = innerNode.Attributes["data-id"]?.Value;
                        var schedule = new ProjectionsSchedule()
                        {
                            FilmId = film.FilmId,
                            ProjectionTimestamp = utcDate,
                            ProviderId = source.ProviderId,
                            SourceEndpoint = $"https://{source.ProviderDomain}/seats/{ projectionId }?caller_id=0",
                            ThreaterId = 1
                        };

                        await _cadorinatorService.AddSchedule(schedule);
                    }
                }
            }
        }

        public async Task SampleData(ProjectionsSchedule schedule, int secondsETA)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(schedule.SourceEndpoint),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("UserAgent", "Cadorin");
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("Host", schedule.Provider.ProviderDomain);

                int tries = 0;
                Request:
                    await Task.Delay(_settings.DefaultDelay * tries);
                    var response = await client.SendAsync(request);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK && tries < 10) goto Request;

                var json = await response?.Content?.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TheaterSample>(json);
                if (result == null) return;

                await _cadorinatorService.LoadSample(schedule.ProjectionsScheduleId, result.Bought.Count, result.Locked.Count, result.Reserved.Count, result.Quarantined.Count, result.Total, secondsETA);
            }
        }
    }
}
