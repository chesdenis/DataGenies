namespace DataGenies.Core.Models
{
    public class DropFolderOptions
    {
        public string Path { get; set; } = "C:\\GeniesDropFolder";

        public bool UseZippedPackages { get; set; } = false;
    }
}