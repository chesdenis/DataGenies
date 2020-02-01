using System.Collections.Generic;

namespace DataGenies.Core.Repositories
{
    public interface IFileSystemRepository
    {
        IEnumerable<string> GetFilesInFolder(string path, string criteria);
    }
}