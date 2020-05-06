using System.IO;
using System.Linq;

namespace DG.Core.Providers
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public string[] GetAssembliesLocations(string path)
        {
            if (!Directory.Exists(path))
            {
                return new string[] { };
            }

            return Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories)
                .Union(Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories)).ToArray();
        }
    }
}