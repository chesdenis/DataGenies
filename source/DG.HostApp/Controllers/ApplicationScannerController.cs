using System.Linq;
using System.Text.Json;
using DG.Core.Model.Dto;
using DG.Core.Scanners;
using Microsoft.AspNetCore.Mvc;

namespace DG.HostApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationScannerController : ControllerBase
    {
        private readonly IApplicationScanner applicationScanner;

        public ApplicationScannerController(IApplicationScanner applicationScanner)
        {
            this.applicationScanner = applicationScanner;
        }

        [HttpGet]
        [Route("Scan")]
        public ActionResult<string> Scan()
        {
            var data = this.applicationScanner.Scan()
                .Select(s => new TypeDto()
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