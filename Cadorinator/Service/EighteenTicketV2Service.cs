using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Service.Helper;
using Cadorinator.Service.Model;
using Cadorinator.Service.Service.Interface;
using Cadorinator.ServiceContract.Settings;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cadorinator.Service.Service
{

    public class EighteenTicketV2Service : IProviderService
    {
        public int ProviderSourceId => 2;

        private readonly IDALService _cadorinatorService;
        private readonly ICadorinatorSettings _settings;

        public EighteenTicketV2Service(IDALService service, ICadorinatorSettings settings)
        {
            _cadorinatorService = service;
            _settings = settings;
        }

        public async Task RegisterSchedules(Provider source)
        {
            int tries = 0;
            OuterRequest:
                await Task.Delay(_settings.DefaultDelay * tries);
                tries++;
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/");
                if (htmlDoc == null) return;
                if (htmlDoc.ParsedText.Length < 80 && tries < 10) goto OuterRequest;


            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'m18-film-projection-block container col-md-12')]");
            if (nodes == null) return;

            foreach (var node in nodes)
            {
                var filmTitle = node.SelectSingleNode(".//h4")?.InnerText;
                if (filmTitle == null) continue;
                var innerNodes = node.SelectNodes(".//*[contains(@class,'m18-home-projection')]");
                if (innerNodes == null) continue;
                foreach(var panel in innerNodes)
                {
                    var innerHtml = panel.Attributes["href"]?.Value;
                    if (innerHtml == null) continue;


                    if (long.TryParse(panel.Attributes["data-time"]?.Value, out var unixTime))
                    {
                        var scheduletime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).ToUniversalTime();
                        var film = await _cadorinatorService.UpselectFilm(filmTitle, scheduletime);

                        var schedule = new ProjectionsSchedule()
                        {
                            FilmId = film.FilmId,
                            ProjectionTimestamp = scheduletime.UtcDateTime,
                            ProviderId = source.ProviderId,
                            SourceEndpoint = innerHtml,
                            ThreaterId = 1
                        };

                        await _cadorinatorService.AddSchedule(schedule);
                    }
                }
            }
        }

        public async Task SampleData(ProjectionsSchedule schedule, int secondsETA)
        {
            int tries = 0;
            Request:
                await Task.Delay(_settings.DefaultDelay * tries);
                tries++;
                var filmPage = await new HtmlWeb().LoadFromWebAsync(schedule.SourceEndpoint);
                if (filmPage.ParsedText.Length < 80 && tries < 10) goto Request;

            var nodes = filmPage.DocumentNode.SelectNodes("//*[contains(@class,'label label-success film-projection m18-label-pj smooth-scroller')]");
            if (nodes == null) return;

            var node = nodes.FirstOrDefault(x => DatetimeHelper.ParseScheduleDate(x.Attributes["data-date"]?.Value).UtcDateTime == schedule.ProjectionTimestamp);
            if (node == null) return;

            using (var client = new HttpClient())
            {
                var baseAddress = $"https://{schedule.Provider.ProviderDomain}/";
                var projectionId = node.Attributes["data-id"].Value;

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(baseAddress + $"seats/{projectionId}?caller_id=0"),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("UserAgent", "Cadorin");
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("Host", schedule.Provider.ProviderDomain);

                int InnerTries = 0;

                InnerRequest:
                    await Task.Delay(_settings.DefaultDelay * InnerTries);
                    InnerTries++;
                    var response = await client.SendAsync(request);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK && tries < 10) goto InnerRequest;

                var json = await response?.Content?.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TheaterSample>(json);
                if (result == null) return;

                await _cadorinatorService.LoadSample(schedule.ProjectionsScheduleId, result.Bought.Count, result.Locked.Count, result.Reserved.Count, result.Quarantined.Count, result.Total, secondsETA);
            }
        }
    }
}
