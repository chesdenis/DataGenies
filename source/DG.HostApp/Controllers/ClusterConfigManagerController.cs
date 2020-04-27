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
        public ActionResult<string> GetClusterConfig()
        {
            return this.Ok(JsonSerializer.Serialize(this.clusterConfigManager.GetConfig()));
        }

        [HttpPost]
        public ActionResult WriteConfig([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfig(clusterConfig);
            return this.Ok();
        }
        
        [HttpPost]
        public ActionResult WriteClusterDefinition([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfig(clusterConfig);
            return this.Ok();
        }
        
        [HttpPost]
        public ActionResult SyncConfigsAcrossHosts()
        {
            this.clusterConfigManager.SyncConfigsAcrossHosts();
            return this.Ok();
        }
    }
}