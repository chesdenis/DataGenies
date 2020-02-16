using DataGenies.Core.Models;
using DataGenies.Core.Services;

namespace DataGenies.Core.Configurators
{
    public interface IBindingConfigurator
    {
        void ConfigureFor(IManagedService managedService, ApplicationInstanceEntity applicationInstanceEntity);
    }
}