using System.Text.Json;
using DG.Core.ConfigManagers;
using DG.Core.Model.ClusterConfig;
using DG.HostApp.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route(ClusterConfigManagerRoutes.Root)]
    [ApiController]
    public class ClusterConfigManagerController : ControllerBase
    {
        private readonly IClusterConfigManager clusterConfigManager;
        
        public ClusterConfigManagerController(IClusterConfigManager clusterConfigManager)
        {
            this.clusterConfigManager = clusterConfigManager;
        }

        [HttpGet]
        [Route(ClusterConfigManagerRoutes.GetConfig)]
        public ActionResult<string> GetConfig()
        {
            return this.Ok(JsonSerializer.Serialize(
                this.clusterConfigManager.GetConfig(),
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                }));
        }

        [HttpPost]
        [Route(ClusterConfigManagerRoutes.WriteConfig)]
        public ActionResult WriteConfig([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfig(clusterConfig);
            return this.Ok();
        }

        [HttpPost]
        [Route(ClusterConfigManagerRoutes.WriteClusterDefinition)]
        public ActionResult WriteClusterDefinition([FromBody] ClusterDefinition clusterDefinition)
        {
            this.clusterConfigManager.WriteClusterDefinition(clusterDefinition);
            return this.Ok();
        }
    }
}