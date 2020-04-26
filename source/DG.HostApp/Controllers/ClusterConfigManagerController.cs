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
            return this.Ok(this.clusterConfigManager.GetConfigAsJson());
        }

        [HttpPost]
        public ActionResult PostClusterConfig([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfigAsJson(null);
            return this.Ok();
        }
    }
}