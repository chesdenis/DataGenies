using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Repositories
{
    public interface IFileSystemRepository
    {
        IEnumerable<string> GetFilesInFolder(string path, string criteria);
    }
}