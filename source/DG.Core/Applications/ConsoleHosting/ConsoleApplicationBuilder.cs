namespace DG.Core.Orchestrators
{
    public class ConsoleApplicationBuilder : IApplicationBuilder
    {
        private const string HostingModelName = "Console";

        public void Build(ApplicationUniqueId applicationUniqueId, string propertiesAsJson = "{}")
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ApplicationUniqueId applicationUniqueId)
        {
            throw new System.NotImplementedException();
        }

        public bool CanExecute(string hostingModelName)
        {
            return hostingModelName == HostingModelName;
        }

        public void Scale(ApplicationUniqueId applicationUniqueId, int newInstanceCount)
        {
            throw new System.NotImplementedException();
        }
    }
}