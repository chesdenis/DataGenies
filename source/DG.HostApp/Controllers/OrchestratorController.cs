using System.Collections.Generic;
using System.Text.Json;
using DG.Core.Extensions;
using DG.Core.Model.Dto;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using DG.Core.Readers;
using DG.HostApp.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route(OrchestratorControllerRoutes.Root)]
    [ApiController]
    public class OrchestratorController : ControllerBase
    {
        private readonly IApplicationOrchestrator applicationOrchestrator;
        private readonly IApplicationInstanceSettingsReader applicationInstanceSettingsReader;

        public OrchestratorController(
            IApplicationOrchestrator applicationOrchestrator,
            IApplicationInstanceSettingsReader applicationInstanceSettingsReader)
        {
            this.applicationOrchestrator = applicationOrchestrator;
            this.applicationInstanceSettingsReader = applicationInstanceSettingsReader;
        }

        [HttpGet]
        [Route(OrchestratorControllerRoutes.GetInstanceState)]
        public ActionResult<IEnumerable<StateReport>> GetInstanceState(
            [FromQuery]string application,
            [FromQuery]string instanceName)
        {
            return new ActionResult<IEnumerable<StateReport>>(
                this.applicationOrchestrator.GetInstanceState(application, instanceName));
        }

        [HttpGet]
        [Route(OrchestratorControllerRoutes.GetInstanceSettings)]
        public ActionResult<string> GetInstanceSettings(
          [FromQuery]string appType,
          [FromQuery]string instanceName)
        {
            var settings = this.applicationInstanceSettingsReader.GetSettings(appType, instanceName);
            var list = new List<PropertyDTO>();
            var props = settings?.GetType().GetProperties();
            if (props != null)
            {
                foreach (var prop in props)
                {
                    settings.GetPropertyDTOsToList(prop, list);
                }
            }

            return this.Ok(JsonSerializer.Serialize(list));
        }
    }
}