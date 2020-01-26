using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataGenies.AspNetCore.DataGeniesCore.Repositories
{
    public class FileSystemRepository : IFileSystemRepository
    {
        public IEnumerable<string> GetFilesInFolder(string path, string criteria)
        {
            return Directory.GetFiles(path, criteria, SearchOption.AllDirectories).ToList();
        }
    }
}