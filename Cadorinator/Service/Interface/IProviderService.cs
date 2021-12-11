using Cadorinator.Infrastructure.Entity;
using System.Threading.Tasks;

namespace Cadorinator.Service.Service.Interface
{
    public interface IProviderService
    {
        int ProviderSourceId { get; }
        Task RegisterSchedules(Provider source);
        Task SampleData(ProjectionsSchedule schedule);
    }
}
