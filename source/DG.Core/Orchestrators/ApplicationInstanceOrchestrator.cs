using System.Linq;
using DG.Core.Scanners;

namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Extensions;
    using DG.Core.Model.ClusterConfig;

    // public class ApplicationInstanceOrchestrator : IApplicationInstanceOrchestrator
    // {
    //     private readonly IApplicationScanner applicationScanner;
    //     private readonly IDictionary<string, object> inMemoryInstances =
    //         new Dictionary<string, object>();
    //
    //     public ApplicationInstanceOrchestrator(
    //         IApplicationScanner applicationScanner)
    //     {
    //         this.applicationScanner = applicationScanner;
    //     }
    //     
    //     public IDictionary<string, Type> PrepareInstancesDataToCreate(IEnumerable<ApplicationInstance> appInstances)
    //     {
    //         var instanesToCreate = new Dictionary<string, Type>();
    //         foreach (var instance in applicationInstances)
    //         {
    //             var instanceKey = ApplicationExtensions.ConstructUniqueId(instance.Type, instance.Name);
    //             var instanceType = this.applicationScanner.Scan().First(w =>
    //             {
    //                 var applicationInstances = appInstances as ApplicationInstance[] ?? appInstances.ToArray();
    //                 return applicationInstances.Any(ww => ww.Type == w.Name);
    //             });
    //             instanesToCreate.Add(instanceKey, instanceType);
    //         }
    //
    //         return instanesToCreate;
    //     }
    //
    //     public IDictionary<string, object> GetInMemoryInstancesData()
    //     {
    //         return this.inMemoryInstances;
    //     }
    // }
}
