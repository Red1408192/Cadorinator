using Cadorinator.Infrastructure;
using Cadorinator.Infrastructure.Entity;
using Cadorinator.Service.Helper;
using Cadorinator.Service.Model;
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
    public class EighteenTicketV2Service
    {
        private readonly ICadorinatorService cadService;

        public EighteenTicketV2Service(ICadorinatorService service)
        {
            cadService = service;
        }

        public async Task RegisterSchedules(Provider source)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync($"https://{source.ProviderDomain}/");
            if (htmlDoc == null) return;

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'m18-film-projection-block container col-md-12')]");

            foreach (var node in nodes)
            {
                var filmTitle = node.SelectSingleNode(".//h4")?.InnerText;
                if (filmTitle == null) continue;
                var innerNode = node.SelectNodes(".//*[contains(@class,'m18-home-projection')]")?.FirstOrDefault ();
                if (innerNode == null) continue;
                var innerHtml = innerNode.Attributes["href"]?.Value;
                if (innerHtml == null) continue;


                if (long.TryParse(innerNode.Attributes["data-time"]?.Value, out var unixTime))
                {
                    var scheduletime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).ToUniversalTime();
                    var film = await cadService.UpselectFilm(filmTitle, scheduletime.DateTime);

                    var schedule = new ProjectionsSchedule()
                    {
                        FilmId = film.FilmId,
                        ProjectionTimestamp = scheduletime.DateTime,
                        ProviderId = source.ProviderId,
                        SourceEndpoint = innerHtml,
                        ThreaterId = 1
                    };

                    await cadService.AddSchedule(schedule);
                }
            }
        }

        public async Task SampleData(ProjectionsSchedule schedule)
        {
            var filmPage = await new HtmlWeb().LoadFromWebAsync(schedule.SourceEndpoint);
            var nodes = filmPage.DocumentNode.SelectNodes("//*[contains(@class,'label label-success film-projection m18-label-pj smooth-scroller')]");

            var node = nodes.FirstOrDefault(x => DatetimeHelper.ParseItalianDate(x.Attributes["data-date"]?.Value).UtcDateTime == schedule.ProjectionTimestamp);
            
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

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TheaterSeats>(json);

                await cadService.LoadSample(schedule.ProjectionsScheduleId, result.Bought.Count, result.Locked.Count, result.Reserved.Count, result.Quarantined.Count, result.Total);
            }
        }
    }
}
