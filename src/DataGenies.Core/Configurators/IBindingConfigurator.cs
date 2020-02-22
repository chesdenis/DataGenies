using DataGenies.Core.Models;
using DataGenies.Core.Services;

namespace DataGenies.Core.Configurators
{
    public interface IBindingConfigurator
    {
        void ConfigureFor(int instanceId);

        void ConfigureForTemplateScope(int instanceId, string boundTemplateName, string routingKey = "#");
        
        void ConfigureForInstanceScope(int instanceId, string boundInstanceName, string routingKey = "#");
    }
}