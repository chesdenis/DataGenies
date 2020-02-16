namespace DataGenies.Core.Containers
{
    public interface IContainer
    {
        void Register<T>(object instance)
            where T : class;

        void Register<T>(object instance, string name)
            where T : class;

        T Resolve<T>();
        
        T Resolve<T>(string name);
    }
}