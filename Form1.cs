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

            this.items = new List<ListViewItem>();
            
            this.FindPhotosWorker = new BackgroundWorker();
            this.FindPhotosWorker.WorkerReportsProgress = true;
            this.FindPhotosWorker.DoWork += new DoWorkEventHandler(this.GetImagesInDirectory);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SetUpListView);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.FindPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateProgress);

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

        private void CopyPhotosOnClick(object sender, EventArgs e)
        {
            CopyFilesForm copyFiles = new CopyFilesForm(this.PhotoDisplay.SelectedItems);
            copyFiles.ShowDialog();
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
                using (Bitmap image = new Bitmap(fileName))
                {
                    Image.GetThumbnailImageAbort dummy = null;
                    images.Images.Add(image.GetThumbnailImage(128, 96, dummy, IntPtr.Zero));
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

        private ImageList images;
        private List<ListViewItem> items;
        private BackgroundWorker FindPhotosWorker;
    }
}