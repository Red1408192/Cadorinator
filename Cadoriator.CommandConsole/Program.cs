using System;
using System.Linq;
using static Cadorinator.ServiceContract.Command.ConsoleComand;

namespace Cadorinator.CommandConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cadorinator v0.1.4");
            Console.WriteLine("Command Console");
            Console.WriteLine("Type 'help' to list all command");

            while (true)
            {
                var input = Console.ReadLine().Split(" ");
                if (input[0] == Exit.Comand) break;
                if (input[0] == Help.Comand) ComandList.ForEach(x => Console.WriteLine($"{x.Comand}{x.ListParam()} - {x.Description}"));
            }
        }
    }
}
