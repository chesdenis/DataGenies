using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Models;

namespace DataGenies.Core.Orchestrators
{
    public interface IOrchestrator
    {
        IQueryable<ApplicationInstanceEntity> GetDeployed();
        IQueryable<ApplicationInstanceEntity> GetStarted();
        IQueryable<ApplicationInstanceEntity> GetStopped();
        
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