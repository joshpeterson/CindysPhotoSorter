using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace PhotoSorter
{
    public partial class PhotoSorter : Form
    {
        public PhotoSorter()
        {
            InitializeComponent();
            this.destinationDirectories = new List<DestinationDirectoryInformation>();
            this.StatusStripLabel.Text = "Click Find Photos to select current photo location.";
        }

        private void SourceDirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            folderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(folderBrowser.SelectedPath))
            {
                this.PhotoDisplay.Clear();

                ImageList images = new ImageList();
                images.ImageSize = new Size(128, 96);
                images.ColorDepth = ColorDepth.Depth24Bit;
                this.PhotoDisplay.LargeImageList = images;

                int i = 0;
                foreach (string fileName in GetImageFilesInDirectory(folderBrowser.SelectedPath))
                {
                    Bitmap image = new Bitmap(fileName);
                    images.Images.Add(image);
                    ListViewItem item = new ListViewItem(Path.GetFileName(fileName));
                    item.ImageIndex = i;
                    item.Tag = fileName;
                    this.PhotoDisplay.Items.Add(item);
                    i++;
                }    
            }

            this.StatusStripLabel.Text = "Click Add Destination to select photo destinations.";
        }

        private void AddDirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            folderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(folderBrowser.SelectedPath))
            {
                DestinationDirectoryInformation destinationDirectory = new DestinationDirectoryInformation(folderBrowser.SelectedPath);
                this.destinationDirectories.Add(destinationDirectory);
                destinationDirectory.AddToPanel(this.DestinationDirectoriesPanel);
            }

            this.StatusStripLabel.Text = "Click Copy Photos to copy photos to each destination.";
        }

        private void RemoveAllDestinationsOnClick(object sender, EventArgs e)
        {
            this.DestinationDirectoriesPanel.Controls.Clear();
            this.destinationDirectories.Clear();

            this.StatusStripLabel.Text = "Click Add Destination to select photo destinations.";
        }

        private void CopyPhotosOnClick(object sender, EventArgs e)
        {
            int fileNameCounter = 1;
            int numPhotos = this.PhotoDisplay.SelectedItems.Count;
            if (numPhotos > 0)
            {
                this.StatusStripLabel.Text = "Copying photos...";
            }
            else
            {
                this.StatusStripLabel.Text = "Select photos to copy.";
            }

            foreach (ListViewItem photo in this.PhotoDisplay.SelectedItems)
            {
                string oldFileName = photo.Tag.ToString();
                foreach (DestinationDirectoryInformation directory in this.destinationDirectories)
                {
                    string newFileName = Path.Combine(directory.DestinationDirectoryName, string.Format("{0} {1}{2}", directory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, numPhotos), Path.GetExtension(oldFileName)));
                    while (File.Exists(newFileName))
                    {
                        fileNameCounter++;
                        newFileName = Path.Combine(directory.DestinationDirectoryName, string.Format("{0} {1}{2}", directory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, numPhotos), Path.GetExtension(oldFileName)));
                    }

                    File.Copy(oldFileName, newFileName);
                }

                if (this.DeleteCheckbox.Checked)
                {
                    File.Delete(oldFileName);
                }

                fileNameCounter++;
            }

            if (numPhotos > 0)
            {
                this.StatusStripLabel.Text = "Photos copied successfully.";
            }
        }

        private string GetFileNameNumber(int fileNameCounter, int numPhotos)
        {
            return fileNameCounter.ToString();
        }

        private IEnumerable<string> GetImageFilesInDirectory(string directoryName)
        {
            List<string> imageFileNames = new List<string>();
            foreach (string fileName in Directory.GetFiles(directoryName))
            {
                if (IsImageFile(fileName))
                {
                    imageFileNames.Add(fileName);
                }
            }
            return imageFileNames;
        }

        private bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension == ".jpg" || extension == ".png";
        }

        struct DestinationDirectoryInformation
        {
            public DestinationDirectoryInformation(string destinationDirectoryName)
            {
                this.destinationDirectoryName = Path.GetFullPath(destinationDirectoryName);
                this.destinationDirectoryDisplay = new Label();
                this.destinationDirectoryDisplay.Height = 1;
                this.destinationDirectoryDisplay.AutoSize = true;
                this.destinationDirectoryDisplay.Text = this.destinationDirectoryName.Substring(this.destinationDirectoryName.LastIndexOf(Path.DirectorySeparatorChar)+1);
                
                ToolTip fullDirectoryName = new ToolTip();
                fullDirectoryName.SetToolTip(this.destinationDirectoryDisplay, string.Format("Destination directory: {0}", this.destinationDirectoryName));

                this.fileNamePrefix = new TextBox();
                this.fileNamePrefix.Text = "File name prefix";
                this.fileNamePrefix.Click += new EventHandler(this.FileNamePrefixOnClick);
            }

            public void AddToPanel(Panel parent)
            {
                FlowLayoutPanel directoryPanel = new FlowLayoutPanel();
                directoryPanel.Height = 1;
                directoryPanel.AutoSize = true;
                directoryPanel.Controls.Add(this.destinationDirectoryDisplay);
                directoryPanel.Controls.Add(this.fileNamePrefix);

                parent.Controls.Add(directoryPanel);
            }

            public string FileNamePrefix
            {
                get
                {
                    return this.fileNamePrefix.Text;
                }
            }

            public string DestinationDirectoryName
            {
                get
                {
                    return this.destinationDirectoryName;
                }
            }

            private void FileNamePrefixOnClick(object sender, EventArgs e)
            {
                TextBox fileNamePrefix = sender as TextBox;
                fileNamePrefix.Text = string.Empty;
            }

            private string destinationDirectoryName;
            private Label destinationDirectoryDisplay;
            private TextBox fileNamePrefix;
        }

        private List<DestinationDirectoryInformation> destinationDirectories;
    }
}
