using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IAssemblyScanner
    {
        IEnumerable<ApplicationTemplateInfo> ScanApplicationTemplates(string assemblyFullPath);

        IEnumerable<BehaviourInfo> ScanBehaviours(string assemblyFullPath);
    }
}