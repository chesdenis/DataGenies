using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IAssemblyScanner
    {
        IEnumerable<ApplicationTemplateEntity> ScanApplicationTemplates(string assemblyFullPath);

        IEnumerable<BehaviourTemplateEntity> ScanBehaviourTemplates(string assemblyFullPath);
    }
}