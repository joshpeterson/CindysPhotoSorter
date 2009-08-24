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
            this.items = new List<ListViewItem>();
            
            this.FindPhotosWorker = new BackgroundWorker();
            this.FindPhotosWorker.WorkerReportsProgress = true;
            this.FindPhotosWorker.DoWork += new DoWorkEventHandler(this.GetImagesInDirectory);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SetUpListView);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.FindPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateProgress);

            this.CopyPhotosWorker = new BackgroundWorker();
            this.CopyPhotosWorker.WorkerReportsProgress = true;
            this.CopyPhotosWorker.DoWork += new DoWorkEventHandler(this.CopyImages);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.PhotoCopyComplete);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.CopyPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateProgress);

            this.StatusStripLabel.Text = "Click Find Photos to select current photo location.";
        }

        private void SourceDirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            folderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(folderBrowser.SelectedPath))
            {
                this.images = new ImageList();
                this.items.Clear();
                this.PhotoDisplay.Clear();

                this.FindPhotosWorker.RunWorkerAsync(folderBrowser.SelectedPath);

                this.StatusStripLabel.Text = string.Format("Finding photos in {0}...", folderBrowser.SelectedPath);
            }
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
            int numPhotos = this.PhotoDisplay.SelectedItems.Count;
            if (numPhotos > 0)
            {
                List<string> photoFileNames = new List<string>();
                foreach (ListViewItem photo in this.PhotoDisplay.SelectedItems)
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
        }

        private void GetImagesInDirectory(object sender, DoWorkEventArgs e)
        {
            this.images.ImageSize = new Size(128, 96);
            this.images.ColorDepth = ColorDepth.Depth24Bit;

            string directoryName = (string)e.Argument;
            int i = 0;
            int highestPercentageReached = 0;
            IEnumerable<string> imagesInDirectory = GetImageFilesInDirectory(directoryName);
            int numImages = imagesInDirectory.Count();
            foreach (string fileName in imagesInDirectory)
            {
                Bitmap image = new Bitmap(fileName);
                images.Images.Add(image);
                ListViewItem item = new ListViewItem(Path.GetFileName(fileName));
                item.ImageIndex = i;
                item.Tag = fileName;
                this.items.Add(item);
                i++;

                int percentComplete = (int)((float)i / (float)numImages * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    this.FindPhotosWorker.ReportProgress(percentComplete);
                }
            }
        }

        private void SetUpListView(object sender, RunWorkerCompletedEventArgs e)
        {
            this.PhotoDisplay.LargeImageList = this.images;
            foreach (ListViewItem item in this.items)
            {
                this.PhotoDisplay.Items.Add(item);
            }

            this.StatusStripLabel.Text = "Click Add Destination to select photo destinations.";
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
        private ImageList images;
        private List<ListViewItem> items;
        private BackgroundWorker FindPhotosWorker;
        private BackgroundWorker CopyPhotosWorker;
    }
}