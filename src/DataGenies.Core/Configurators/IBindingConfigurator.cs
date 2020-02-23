using DataGenies.Core.Models;

namespace DataGenies.Core.Configurators
{
    public interface IBindingConfigurator
    {
        BindingNetwork ConfigureBindingNetworkFor(int applicationInstanceEntityId);

        void ConfigureBindings(BindingNetwork bindingNetwork);
    }
}