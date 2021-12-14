using Cadorinator.Infrastructure.Entity;
using Cadorinator.Model;
using Cadorinator.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure.Interface
{
    public interface ICadorinatorService
    {
        Task<bool> CheckSchedulesAsync(TimeSpan timeSpan, OperationSchedule operationSchedule);
        Task<bool> CollectSchedulesAsync(OperationSchedule operationSchedule, long? providerId = null);
        Task<bool> TakeSample(ProjectionsSchedule schedule, int secondsEta);
    }
}