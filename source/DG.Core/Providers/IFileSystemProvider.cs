namespace DG.Core.Providers
{
    public interface IFileSystemProvider
    {
        string[] GetAssembliesLocations(string path);
    }
}