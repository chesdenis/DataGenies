namespace DG.Core.Orchestrators
{
    public interface IApplicationBuilder
    {
        void Build(ApplicationUniqueId applicationUniqueId, string propertiesAsJson);

        void Scale(ApplicationUniqueId applicationUniqueId, int newInstanceCount);

        void Remove(ApplicationUniqueId applicationUniqueId);
    }
}