using DataGenies.Core.Models;
using DataGenies.Core.Services;

namespace DataGenies.Core.Configurators
{
    public interface IBindingConfigurator
    {
        BindingNetwork ConfigureBindingNetworkFor(int applicationInstanceEntityId);
        void ConfigureBindings(BindingNetwork bindingNetwork);
    }
}