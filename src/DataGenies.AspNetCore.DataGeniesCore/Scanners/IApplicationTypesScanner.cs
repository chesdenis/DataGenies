using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Models;

namespace DataGenies.AspNetCore.DataGeniesCore.Scanners
{
    public interface IApplicationTypesScanner
    {
        IEnumerable<ApplicationType> ScanTypes();
    }
}