using Cadorinator.Infrastructure.Entity;
using Cadorinator.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure.Interface
{
    public interface ICadorinatorService
    {
        Task<IList<Operation>> CheckSchedulesAsync(TimeSpan timeSpan, List<Operation> operations);
        Task<IList<Operation>> CollectSchedulesAsync();
        Task<IList<Operation>> TakeSample(ProjectionsSchedule schedule, int secondsEta);
    }
}