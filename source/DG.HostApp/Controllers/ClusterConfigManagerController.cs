using System.Text.Json;
using DG.Core.ConfigManagers;
using DG.Core.Model.ClusterConfig;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClusterConfigManagerController : ControllerBase
    {
        private readonly IClusterConfigManager clusterConfigManager;

        public ClusterConfigManagerController(IClusterConfigManager clusterConfigManager)
        {
            this.clusterConfigManager = clusterConfigManager;
        }

        [HttpGet]
        [Route("GetConfig")]
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
        [Route("WriteConfig")]
        public ActionResult WriteConfig([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfig(clusterConfig);
            return this.Ok();
        }
        
        [HttpPost]
        [Route("WriteClusterDefinition")]
        public ActionResult WriteClusterDefinition([FromBody] ClusterDefinition clusterDefinition)
        {
            this.clusterConfigManager.WriteClusterDefinition(clusterDefinition);
            return this.Ok();
        }
        
        [HttpPost]
        [Route("SyncClusterDefinitionAcrossHosts")]
        public ActionResult SyncClusterDefinitionAcrossHosts()
        {
            this.clusterConfigManager.SyncClusterDefinitionAcrossHosts();
            return this.Ok();
        }
    }
}