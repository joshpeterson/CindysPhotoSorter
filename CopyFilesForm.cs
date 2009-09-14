using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PhotoSorter
{
    public partial class CopyFilesForm : Form
    {
        public CopyFilesForm(ListView.SelectedListViewItemCollection selectedItems)
        {
            InitializeComponent();

            this.destinationDirectories = new List<DestinationDirectoryInformation>();
            this.selectedItems = selectedItems;

            this.CopyPhotosWorker = new BackgroundWorker();
            this.CopyPhotosWorker.WorkerReportsProgress = true;
            this.CopyPhotosWorker.DoWork += new DoWorkEventHandler(this.CopyImages);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.PhotoCopyComplete);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.CopyPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateProgress);
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

        private void CopyPhotosOnClick(object sender, EventArgs e)
        {
            int numPhotos = this.selectedItems.Count;
            if (numPhotos > 0)
            {
                List<string> photoFileNames = new List<string>();
                foreach (ListViewItem photo in this.selectedItems)
                {
                    photoFileNames.Add(photo.Tag.ToString());

                }

                CopyPhotoInformation copyInformation = new CopyPhotoInformation();
                copyInformation.PhotoFileNames = photoFileNames;
                copyInformation.DeleteSourcePhotoAfterCopy = this.DeleteCheckbox.Checked;
                copyInformation.NumberOfPhotos = numPhotos;

                this.CopyPhotosWorker.RunWorkerAsync(copyInformation);

                this.StatusStripLabel.Text = "Copying photos...";
            }
            else
            {
                this.StatusStripLabel.Text = "Select photos to copy.";
            }
        }

        private void CopyImages(object sender, DoWorkEventArgs e)
        {
            int fileNameCounter = 1;
            CopyPhotoInformation copyInformation = (CopyPhotoInformation)e.Argument;
            foreach (string fileName in copyInformation.PhotoFileNames)
            {
                foreach (DestinationDirectoryInformation directory in this.destinationDirectories)
                {
                    string newFileName = Path.Combine(directory.DestinationDirectoryName, string.Format("{0} {1}{2}", directory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfPhotos), Path.GetExtension(fileName)));
                    while (File.Exists(newFileName))
                    {
                        fileNameCounter++;
                        newFileName = Path.Combine(directory.DestinationDirectoryName, string.Format("{0} {1}{2}", directory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfPhotos), Path.GetExtension(fileName)));
                    }

                    File.Copy(fileName, newFileName);
                }

                if (copyInformation.DeleteSourcePhotoAfterCopy)
                {
                    File.Delete(fileName);
                }

                fileNameCounter++;
            }
        }

        private void PhotoCopyComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.StatusStripLabel.Text = "Photos copied successfully.";
            this.Close();
        }

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
        }

        private void ClearProgressBar(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressBar.Value = 0;
        }

        private string GetFileNameNumber(int fileNameCounter, int numPhotos)
        {
            return fileNameCounter.ToString();
        }

        struct CopyPhotoInformation
        {
            public List<string> PhotoFileNames;
            public int NumberOfPhotos;
            public bool DeleteSourcePhotoAfterCopy;
        }

        struct DestinationDirectoryInformation
        {
            public DestinationDirectoryInformation(string destinationDirectoryName)
            {
                this.destinationDirectoryName = Path.GetFullPath(destinationDirectoryName);
                this.destinationDirectoryDisplay = new Label();
                this.destinationDirectoryDisplay.Height = 1;
                this.destinationDirectoryDisplay.AutoSize = true;
                this.destinationDirectoryDisplay.Text = this.destinationDirectoryName;

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
                directoryPanel.Controls.Add(this.fileNamePrefix);
                directoryPanel.Controls.Add(this.destinationDirectoryDisplay);

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
        private BackgroundWorker CopyPhotosWorker;
        private ListView.SelectedListViewItemCollection selectedItems;
    }
}
