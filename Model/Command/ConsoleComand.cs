using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cadorinator.ServiceContract.CadorinatorResources;

namespace Cadorinator.ServiceContract.Command
{
    public class ConsoleComand
    {
        public string Comand { get; private set; }
        public int Id { get; private set; }
        public string Description { get; private set; }
        public (Type, string)[] Params { get; private set; }

        public ConsoleComand(int id, string comand, string description, (Type, string)[] param = default)
        {
            Id = id;
            Comand = comand;
            Params = param;
            Description = description;
            ComandList.Add(this);
        }

        public string ListParam() => this.Params?.Aggregate("", (a, b) => a + $"[{b.Item1.Name}, {b.Item2}]");

        public static List<ConsoleComand> ComandList { get; } = new List<ConsoleComand>();

        public static ConsoleComand Exit = new ConsoleComand(0, CommandName_Exit_Command, CommandName_Exit_Description);

        public static ConsoleComand Help = new ConsoleComand(1, CommandName_Help_Command, CommandName_Help_Description);

        public static ConsoleComand Restart = new ConsoleComand(2, CommandName_Restart_Command,CommandName_Restart_Description, new[]
        {
            (typeof(int), CommandName_Restart_Param_waitTime)
        });

        public static ConsoleComand SetDelay = new ConsoleComand(3, CommandName_SetDelay_Command, CommandName_SetDelay_Description, new[]
        {
            (typeof(int), CommandName_SetDelay_Param_delayTime)
        });

        public static ConsoleComand SetSamplesRange = new ConsoleComand(4, CommandName_SetSamplesRange_Command, CommandName_SetSampleRange_Description, new[]
        {
            (typeof(int[]), CommandName_SetSampleRange_Param_delayTime)
        });

        public static ConsoleComand ToogleRemotePublish = new ConsoleComand(5, CommandName_ToogleRemotePublish_Command, CommandName_ToogleRemotePublish_Description);

        public static ConsoleComand SetGoogleDriveToken = new ConsoleComand(6, CommandName_SetGoogleDriveToken_Command, CommandName_SetGoogleDriveToken_Description, new[]
        {
            (typeof(string), CommandName_SetGoogleDriveToken_Param_token)
        });

        public static ConsoleComand ToogleBackup = new ConsoleComand(7, CommandName_ToogleBackup_Command, CommandName_ToogleBackup_Description);

        public static ConsoleComand SetBackupFrequency = new ConsoleComand(8, CommandName_SetBackupFrequency_Command, CommandName_SetBackupFrequency_Description, new[]
        {
            (typeof(byte), CommandName_SetBackupFrequency_Param_dayOfTheWeek),
            (typeof(byte), CommandName_SetBackupFrequency_Param_timeOfTheDay)
        });

        public static ConsoleComand PublishReport = new ConsoleComand(9, CommandName_PublishReport_Command, CommandName_PublishReport_Description, new[]
        {
            (typeof(DateTime), CommandName_PublishReport_Param_from),
            (typeof(DateTime), CommandName_PublishReport_Param_to),
            (typeof(long), CommandName_PublishReport_Param_cityId),
            (typeof(long), CommandName_PublishReport_Param_filmId),
            (typeof(bool), CommandName_PublishReport_Param_mode),
        });

        public static ConsoleComand List = new ConsoleComand(10, CommandName_ListFilm_Command, CommandName_ListFilm_Description, new[]
{
            (typeof(DateTime), CommandName_ListFilm_Param_since)
        });
    }
}
