using System.Threading.Tasks;

namespace WebApplication1.DependencyInject.Services
{
    public interface IJobEngine
    {
        Task StartJob();
    }
}
