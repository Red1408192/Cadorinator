using Cadorinator.Infrastructure;
using Cadorinator.ServiceContract.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Cadorinator.ServiceContract.Command.ConsoleComand;

namespace Cadorinator.CommandConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var settings = await Settings.LoadAsync();
            Console.WriteLine("Cadorinator v0.1.4");
            Console.WriteLine("Command Console");
            Console.WriteLine("Type 'help' to list all command");

            using(var context = new DbContextFactory(settings.FilePath, settings.MainDbName).Create())
            {
                while (true)
                {
                    var input = Console.ReadLine().Split(" ");
                    if (input[0] == Exit.Comand) break;
                    if (input[0] == Help.Comand) ComandList.ForEach(x => Console.WriteLine($"{x.Comand}{x.ListParam()} - {x.Description}"));
                    var test = context.Films.ToArray();
                }
            }
        }
    }
}
