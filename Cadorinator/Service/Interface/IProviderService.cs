using Cadorinator.Infrastructure.Entity;
using System.Threading.Tasks;

namespace Cadorinator.Service.Service.Interface
{
    public interface IProviderService
    {
        int ProviderSourceId { get; }
        Task<bool> RegisterSchedules(Provider source);
        Task<bool> SampleData(ProjectionsSchedule schedule, int secondsETA);
    }
}
