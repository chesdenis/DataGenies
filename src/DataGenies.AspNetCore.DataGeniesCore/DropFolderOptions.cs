namespace DataGenies.AspNetCore.DataGeniesCore
{
    public class DropFolderOptions
    {
        public string Path  { get; set; } = "C:\\GeniesDropFolder";

        public bool UseZippedPackages { get; set; } = false;
    }
}