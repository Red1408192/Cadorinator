using Cadorinator.Infrastructure;
using Cadorinator.ServiceContract.Settings;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Cadorinator.ServiceContract.Command.ConsoleComand;

namespace Cadorinator.CommandConsole
{
    class Program
    {
        private static ILogger _logger;

        static async Task Main(string[] args)
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
            var settings = await Settings.LoadAsync(_logger);
            Console.WriteLine();
            Console.WriteLine("Cadorinator v0.1.4");
            Console.WriteLine("Command Console");
            Console.WriteLine("Type 'help' to list all command");

            using(var context = new DbContextFactory(settings.FilePath, settings.MainDbName, _logger).Create())
            {
                while (true)
                {
                    var input = Console.ReadLine().Split(" ");
                    if (input[0] == Exit.Comand)
                    {
                        _logger.Information("exiting . . .");
                        break;
                    }
                    if (input[0] == Help.Comand) ComandList.ForEach(x => Console.WriteLine($"{x.Comand}{x.ListParam()} - {x.Description}"));
                    context.Films.ToList().ForEach(x => Console.WriteLine($"{x.FilmId}-{x.FilmName}"));
                }
            }
        }
    }
}
