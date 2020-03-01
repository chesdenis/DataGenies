using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataGenies.Orchestrator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrchestratorController : ControllerBase, IOrchestrator
    {
        private readonly IOrchestrator _orchestrator;
        private readonly ILogger<OrchestratorController> _logger;

        public OrchestratorController(IOrchestrator orchestrator, ILogger<OrchestratorController> logger)
        {
            _orchestrator = orchestrator;
            _logger = logger;
        }
 
        [HttpGet]
        IQueryable<ApplicationInstanceEntity> IOrchestrator.GetDeployed()
        {
            return _orchestrator.GetDeployed();
        }

        [HttpGet]
        IQueryable<ApplicationInstanceEntity> IOrchestrator.GetStarted()
        {
            return _orchestrator.GetStarted();
        }

        [HttpGet]
        IQueryable<ApplicationInstanceEntity> IOrchestrator.GetStopped()
        {
            return _orchestrator.GetStopped();
        }

        [HttpPost]
        async Task IOrchestrator.PrepareTemplatePackage(int applicationInstanceId)
        {
            await _orchestrator.PrepareTemplatePackage(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Deploy(int applicationInstanceId)
        {
            await _orchestrator.Deploy(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Start(int applicationInstanceId)
        {
            await _orchestrator.Start(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Stop(int applicationInstanceId)
        {
            await _orchestrator.Stop(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Remove(int applicationInstanceId)
        {
            await _orchestrator.Remove(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Redeploy(int applicationInstanceId)
        {
            await _orchestrator.Redeploy(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.Restart(int applicationInstanceId)
        {
            await _orchestrator.Restart(applicationInstanceId);
        }

        [HttpPost]
        async Task IOrchestrator.RestartAll()
        {
            await _orchestrator.RestartAll();
        }

        [HttpPost]
        async Task IOrchestrator.RemoveAll()
        {
            await _orchestrator.RemoveAll();
        }
    }
}