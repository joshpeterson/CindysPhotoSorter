using System.IO;
using System.Windows.Forms;
using System;

namespace PhotoSorter
{
    class DestinationDirectoryInformation
    {
        public DestinationDirectoryInformation(string destinationDirectoryName, string fileNamePrefix)
        {
            this.destinationDirectoryName = Path.GetFullPath(destinationDirectoryName);
            this.fileNamePrefix = fileNamePrefix;

            this.destinationDirectoryDisplay = new Label();
            this.destinationDirectoryDisplay.Height = 1;
            this.destinationDirectoryDisplay.AutoSize = true;
            this.destinationDirectoryDisplay.Text = string.Format("Prefix: {0} - Directory: {1}", fileNamePrefix, this.destinationDirectoryName);
            this.destinationDirectoryDisplay.Padding = new Padding(0, 6, 0, 0);
        }

        public void AddToPanel(FlowLayoutPanel parent)
        {
            parent.Height = 1;
            parent.AutoSize = true;

            parent.Controls.Add(this.destinationDirectoryDisplay);
        }

        public void RemoveFromPanel(FlowLayoutPanel parent)
        {
            parent.Controls.Remove(this.destinationDirectoryDisplay);
        }

        public string FileNamePrefix
        {
            get
            {
                return this.fileNamePrefix;
            }
        }

        public string DestinationDirectoryName
        {
            get
            {
                return this.destinationDirectoryName;
            }
        }

        private string destinationDirectoryName;
        private string fileNamePrefix;
        private Label destinationDirectoryDisplay;
    }
}