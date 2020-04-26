namespace DG.HostApp.Controllers
{
    using DG.Core.ConfigManagers;
    using DG.Core.Models;
    using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<string> GetClusterConfigAsJson()
        {
            return this.Ok(this.clusterConfigManager.GetConfigAsJson());
        }

        [HttpPost]
        public ActionResult PostClusterConfig([FromBody] ClusterConfig clusterConfig)
        {
            this.clusterConfigManager.WriteConfig(clusterConfig);
            return this.Ok();
        }
    }
}