using System.Collections.Generic;

namespace Cadorinator.ServiceContract.Settings
{
    public interface ICadorinatorSettings
    {
        int DefaultDelay { get; set; }
        int Reactivity { get; set; }
        int PollerTimeSpan { get; set; }
        int SchedulerTimeSpan { get; set; }
        int MaxRequestOffset { get; set; }
        int[] SamplesRange { get; set; }
        string MainDbName { get; set; }
        string FilePath { get; set; }
        bool RemotePublish { get; set; }
        string GoogleDriveToken { get; set; }
        bool Backup { get; set; }
        int BackupFrequency { get; set; }
        bool AutoReport { get; set; }
        (byte, byte) AutoReportSchedule { get; set; }
        int LoggingLevel { get; set; }
    }

    public class CadorinatorSettings : ICadorinatorSettings
    {
        public int DefaultDelay { get; set; } = 600;
        public int Reactivity { get; set; } = 200;
        public int PollerTimeSpan { get; set; } = 20;
        public int LoggingLevel { get; set; } = 1;
        public int SchedulerTimeSpan { get; set; } = 60;
        public int MaxRequestOffset { get; set; } = 150;
        public int[] SamplesRange { get; set; } = new[] { 43200, 21600, 10800, 7200, 3600, 1800, 1200, 600, 300, -300 }; //12h, 6h, 3h, 2h, 1h, 30m, 20m, 10m, 5m, -5m
        public bool RemotePublish { get; set; } = false;
        public string GoogleDriveToken { get; set; } = "";
        public bool Backup { get; set; } = true;
        public int BackupFrequency { get; set; } = 1;
        public bool AutoReport { get; set; } = true;
        public (byte, byte) AutoReportSchedule { get; set; } = (6, 8);
        public string MainDbName { get; set; } = "Cadorinator.db";
        public string FilePath { get; set; } = "%AppData%\\Cadorinator\\";
    }
}
