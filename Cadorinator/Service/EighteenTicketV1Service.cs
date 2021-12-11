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
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/");
            if (htmlDoc == null) return;

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'movie movie--preview release border border-light')]");

            foreach (var node in nodes)
            {
                var filmTitle = node.SelectSingleNode(".//*[contains(@class,'movie__title')]")?.InnerText.Replace("\n", "");
                if (filmTitle == null) continue;
                var timeSelector = node.SelectSingleNode(".//*[contains(@class,'time-select')]");
                if (timeSelector == null) continue;
                var filmId = Regex.Match(timeSelector.Attributes["id"]?.Value, "[0-9]{4,}(?!-)");
                if (filmId == null) continue;

                await Task.Delay(_settings.DefaultDelay);
                var innerPage = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/film/{filmId}");
                var results = innerPage.DocumentNode.SelectNodes("//*[contains(@class,'film-projection smooth-scroller')]");

                foreach(var innerNode in results)
                {
                    if (DateTime.TryParse(innerNode.Attributes["data-date"]?.Value, out var date))
                    {
                        var film = await _cadorinatorService.UpselectFilm(filmTitle, date.ToUniversalTime());
                        var projectionId = innerNode.Attributes["data-id"]?.Value;
                        var schedule = new ProjectionsSchedule()
                        {
                            FilmId = film.FilmId,
                            ProjectionTimestamp = date.ToUniversalTime(),
                            ProviderId = source.ProviderId,
                            SourceEndpoint = $"https://{source.ProviderDomain}seats/{ projectionId }?caller_id=0",
                            ThreaterId = 1
                        };

                        await _cadorinatorService.AddSchedule(schedule);
                    }
                }
            }
        }

        public async Task SampleData(ProjectionsSchedule schedule)
        {
            using (var client = new HttpClient())
            {
                var baseAddress = $"https://{schedule.Provider.ProviderDomain}/";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(schedule.SourceEndpoint),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("UserAgent", "Cadorin");
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("Host", schedule.Provider.ProviderDomain);

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TheaterSample>(json);

                await _cadorinatorService.LoadSample(schedule.ProjectionsScheduleId, result.Bought.Count, result.Locked.Count, result.Reserved.Count, result.Quarantined.Count, result.Total);
            }
        }
    }
}
