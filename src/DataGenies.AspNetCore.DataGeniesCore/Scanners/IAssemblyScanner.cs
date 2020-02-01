using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Models;

namespace DataGenies.AspNetCore.DataGeniesCore.Scanners
{
    public interface IAssemblyScanner
    {
        IEnumerable<ApplicationTemplateInfo> ScanApplicationTemplates(string assemblyFullPath);
    }
}