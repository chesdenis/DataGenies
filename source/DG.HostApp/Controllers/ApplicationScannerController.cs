using System.Linq;
using System.Text.Json;
using DG.Core.Model.Dto;
using DG.Core.Scanners;
using DG.HostApp.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route(ApplicationScannerControllerRoutes.Root)]
    [ApiController]
    public class ApplicationScannerController : ControllerBase
    {
        private readonly IApplicationTypesScanner applicationTypesScanner;

        public ApplicationScannerController(IApplicationTypesScanner applicationTypesScanner)
        {
            this.applicationTypesScanner = applicationTypesScanner;
        }

        [HttpGet]
        [Route(ApplicationScannerControllerRoutes.Scan)]
        public ActionResult<string> Scan()
        {
            var data = this.applicationTypesScanner.Scan()
                .Select(s => new ApplicationDto()
                {
                    Name = s.Name,
                    FullName = s.FullName,
                    AssemblyFullName = s.Assembly.FullName,
                    Version = s.Assembly.GetName().Version?.ToString(),
                }).ToList();

            return this.Ok(JsonSerializer.Serialize(data));
        }
    }
}