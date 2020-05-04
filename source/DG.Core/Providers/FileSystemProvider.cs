using System.IO;

namespace DG.Core.Providers
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public string[] GetAssembliesLocations(string path) => Directory.GetFiles(path, "*.dll|*.exe", SearchOption.AllDirectories);
    }
}