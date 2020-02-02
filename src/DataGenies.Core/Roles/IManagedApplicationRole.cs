namespace DataGenies.Core.Roles
{
    public interface IManagedApplicationRole : IRestartable
    {
        IRestartable GetRootComponent();
    }
}