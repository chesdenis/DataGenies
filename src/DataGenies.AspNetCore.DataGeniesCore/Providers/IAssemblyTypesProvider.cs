using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Models;

namespace DataGenies.AspNetCore.DataGeniesCore.Providers
{
    public interface IAssemblyTypesProvider
    {
        IEnumerable<ApplicationTypeInsideAssembly> GetApplicationTypes(string assemblyFullPath);
    }
}