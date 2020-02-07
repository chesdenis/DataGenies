using System.Threading.Tasks;

namespace DataGenies.Core.Orchestrators
{
    public interface IOrchestrator
    {
        Task PrepareTemplatePackage(int applicationInstanceId);
        Task Deploy(int applicationInstanceId);
        Task Start(int applicationInstanceId);
        Task Stop(int applicationInstanceId);
        Task Remove(int applicationInstanceId);
        Task Redeploy(int applicationInstanceId);
        Task Restart(int applicationInstanceId);
        Task RestartAll();
        Task RemoveAll();
    }
}