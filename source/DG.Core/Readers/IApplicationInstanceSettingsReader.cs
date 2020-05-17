using System.Collections.Generic;

namespace DG.Core.Readers
{
    public interface IApplicationInstanceSettingsReader
    {
        IEnumerable<T> GetSharedSettings<T>(string application, string instanceName);

        IEnumerable<object> GetSharedSettings(string application, string instanceName);

        T GetSettings<T>(string application, string instanceName);
        
        object GetSettings(string application, string instanceName);
    }
    
    // public IEnumerable<T> GetSharedProperties<T>(string application, string instanceName)
    // {
    // throw new NotImplementedException();
    // }
    //
    // public T GetProperties<T>(string application, string instanceName)
    // {
    // throw new NotImplementedException();
    // }

    // public IEnumerable<PropertyInfo> GetSettingsProperties(string application, string instanceName)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public object GetProperties(string application, string instanceName)
    // {
    //     var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
    //     return this.inMemoryInstances[uniqueId]
    //         .First()
    //         .GetType()
    //         .GetProperties()
    //         .First(f => f.GetCustomAttributes(typeof(PropertiesAttribute)).Any())
    //         .GetValue(this.inMemoryInstances[uniqueId].First());
    // }
}