using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Models.Contexts;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryDataGeniesDataContext : IDataGeniesDataContext
    {
        public InMemoryDbSet<ApplicationType> ApplicationTypes { get; }  
        
        public InMemoryDbSet<ApplicationInstance> ApplicationInstances { get; }  
        
        public InMemoryDbSet<Binding> Bindings { get; }

        public InMemoryDataGeniesDataContext()
        {
            this.ApplicationTypes = new InMemoryDbSet<ApplicationType>();
            this.ApplicationInstances = new InMemoryDbSet<ApplicationInstance>();
            this.Bindings = new InMemoryDbSet<Binding>();
        }

        IQueryable<ApplicationType> IDataGeniesDataContext.ApplicationTypes => ApplicationTypes;

        IQueryable<ApplicationInstance> IDataGeniesDataContext.ApplicationInstances => ApplicationInstances;

        IQueryable<Binding> IDataGeniesDataContext.Bindings => Bindings;

        public void SeedTestData()
        {
            this.ApplicationTypes.Add(new ApplicationType()
            {
                TypeId = 1, TypeVersion = "2019.1.1", TypeName = "SimpleScrape", ConfigTemplateJson = "{asdfsadgas}"
            });
        }
    }
}