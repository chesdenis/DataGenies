using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorInputFile;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Pages
{
    public partial class ClusterConfigManager
    {
        private const int MaxFileSize = 1 * 1024 * 1024; // 1MB
        private const string DefaultStatus = "Drop a text file here to view it, or click to choose a file";
        private const string LoadingStatus = "Loading...";
        private ClusterConfig clusterConfig;
        private string rawConfigAsJson = "{}";
        private bool fileUploadView = false;
        private string fileToBigStatus = $"That's too big. Max size: {MaxFileSize} bytes.";
        private string status = DefaultStatus;
        private bool rawView = false;

        protected override async Task OnInitializedAsync()
        {
            this.rawConfigAsJson = await this.clusterConfigPageService.ReadConfig(this.currentHost);
            this.SetFormView();
        }

        private async Task ViewFile(IFileListEntry[] files)
        {
            string fileTextContents;

            var file = files.FirstOrDefault();
            if (file == null)
            {
                return;
            }
            else if (file.Size > MaxFileSize)
            {
                this.status = this.fileToBigStatus;
            }
            else
            {
                this.status = LoadingStatus;

                using (var reader = new StreamReader(file.Data))
                {
                    fileTextContents = await reader.ReadToEndAsync();
                }

                this.status = DefaultStatus;
                this.rawConfigAsJson = fileTextContents;

                this.rawView = true;
                this.fileUploadView = false;
            }
        }

        private void SetRawView()
        {
            this.rawConfigAsJson = JsonSerializer.Serialize(this.clusterConfig, new JsonSerializerOptions() { WriteIndented = true });
            this.rawView = true;
        }

        private void SetFormView()
        {
            this.clusterConfig = JsonSerializer.Deserialize<ClusterConfig>(this.rawConfigAsJson);
            this.rawView = false;
        }

        private void ToggleFileUploadView()
        {
            this.fileUploadView = !this.fileUploadView;
        }
    }
}
