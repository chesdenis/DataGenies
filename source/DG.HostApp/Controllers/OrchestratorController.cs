using System.Collections.Generic;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrchestratorController : ControllerBase
    {
        private readonly IApplicationOrchestrator applicationOrchestrator;

        public OrchestratorController(IApplicationOrchestrator applicationOrchestrator)
        {
            this.applicationOrchestrator = applicationOrchestrator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StateReport>> GetInstanceState(
            [FromQuery]string application,
            [FromQuery]string instanceName)
        {
            return new ActionResult<IEnumerable<StateReport>>(
                this.applicationOrchestrator.GetInstanceState(application, instanceName));
        }
    }
}