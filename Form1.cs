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
using System.Drawing.Imaging;

namespace PhotoSorter
{
    public partial class PhotoSorter : Form
    {
        public PhotoSorter()
        {
            InitializeComponent();

            this.items = new List<ListViewItem>();
            this.photosDeletedDuringCopy = new List<string>();
            
            this.FindPhotosWorker = new BackgroundWorker();
            this.FindPhotosWorker.WorkerReportsProgress = true;
            this.FindPhotosWorker.DoWork += new DoWorkEventHandler(this.GetImagesInDirectory);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SetUpListView);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.FindPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SortPhotoDisplay);
            this.FindPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateProgress);

            this.StatusStripLabel.Text = "Click Find Photos to select current photo location.";
        }

        public void OnPhotoDeleted(string fileName)
        {
            this.photosDeletedDuringCopy.Add(fileName);
        }

        private void SourceDirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
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
            if (this.PhotoDisplay.SelectedItems.Count == 0)
            {
                this.StatusStripLabel.Text = "Please select at least one photo to copy.";
                return;
            }

            this.PhotoDisplay.Select();

            CopyFilesForm copyFiles = new CopyFilesForm(this.PhotoDisplay.SelectedItems);
            copyFiles.PhotoDeleted += this.OnPhotoDeleted;
            copyFiles.ShowDialog();
            copyFiles.PhotoDeleted -= this.OnPhotoDeleted;

            RemoveDeletedPhotos();

            this.PhotoDisplay.Sort();

            this.StatusStripLabel.Text = "Photos copied successfully.";
        }

        private void OnSelectionedIndexChanged(object sender, EventArgs e)
        {
            this.StatusStripLabel.Text = string.Format("{0} of {1} photos selected.", this.PhotoDisplay.SelectedItems.Count, this.PhotoDisplay.Items.Count);
        }

        private void RemoveDeletedPhotos()
        {
            foreach (string fileName in this.photosDeletedDuringCopy)
            {
                foreach (ListViewItem item in this.PhotoDisplay.Items)
                {
                    if ((string)item.Tag == fileName)
                    {
                        this.PhotoDisplay.Items.Remove(item);
                    }
                }
            }
            this.photosDeletedDuringCopy.Clear();
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
                ImageData imageData = this.GetImageData(fileName);
                images.Images.Add(imageData.Thumbnail);
                ListViewItem item = new ListViewItem(string.Format("{0}\n{1}", Path.GetFileName(fileName), imageData.DateTaken != DateTime.MinValue ? imageData.DateTaken.ToString() : "Unknown date"));
                item.ImageIndex = i;
                item.Tag = fileName;
                this.items.Add(item);
                i++;

                int percentComplete = (int)((float)i / (float)numImages * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    this.FindPhotosWorker.ReportProgress(percentComplete, fileName);
                }
            }
        }

        private struct ImageData
        {
            public Image Thumbnail;
            public DateTime DateTaken;
        }

        private ImageData GetImageData(string path)
        {
	        using (FileStream fs = File.OpenRead (path))
            {
                using (Image img = Image.FromStream(fs, false, false))
                {
                    ImageData imageData;

                    imageData.Thumbnail = this.GetImageThumbnail(img, path);
                    imageData.DateTaken = this.GetImageDateTaken(img);

                    return imageData;
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

            if (this.PhotoDisplay.Items.Count == 0)
            {
                this.StatusStripLabel.Text = "No photos found";
            }
            else if (this.PhotoDisplay.Items.Count == 1)
            {
                this.StatusStripLabel.Text = string.Format("{0} photo found.", this.PhotoDisplay.Items.Count);
            }
            else
            {
                this.StatusStripLabel.Text = string.Format("{0} photos found.  Select photos to copy.", this.PhotoDisplay.Items.Count);
            }
        }

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
            this.StatusStripLabel.Text = string.Format("Processed photo {0}", Path.GetFullPath((string)e.UserState));
        }

        private void ClearProgressBar(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressBar.Value = 0;
        }

        private void SortPhotoDisplay(object sender, RunWorkerCompletedEventArgs e)
        {
            this.PhotoDisplay.Sort();
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

        private Image GetImageThumbnail(Image image, string fileName)
        {
            try
            {
                PropertyItem thumbnailProperty = image.GetPropertyItem(0x501B);

                byte[] imageBytes = thumbnailProperty.Value;
                MemoryStream stream = new MemoryStream(imageBytes.Length);
                stream.Write(imageBytes, 0, imageBytes.Length);

                return Image.FromStream(stream);
            }
            catch (ArgumentException)
            {
                // We can't get the thumbnail resource, so load the entire photo and generate
                // the thumbnail.  This is pretty slow.
                using (Bitmap fullImage = new Bitmap(fileName))
                {
                    Image.GetThumbnailImageAbort dummy = null;
                    return image.GetThumbnailImage(128, 96, dummy, IntPtr.Zero);
                }
            }
        }

        private DateTime GetImageDateTaken(Image image)
        {
            try
            {
                PropertyItem dateTakenProperty = image.GetPropertyItem(306);

                string dateTaken = Encoding.UTF8.GetString(dateTakenProperty.Value).Trim();
                string secondHalf = dateTaken.Substring(dateTaken.IndexOf(" "), (dateTaken.Length - dateTaken.IndexOf(" ")));
                string firstHalf = dateTaken.Substring(0, 10);
                firstHalf = firstHalf.Replace(":", "-");
                dateTaken = firstHalf + secondHalf;

                return DateTime.Parse(dateTaken);
            }
            catch (ArgumentException)
            {
                return DateTime.MinValue;
            }
        }

        private ImageList images;
        private List<ListViewItem> items;
        private BackgroundWorker FindPhotosWorker;
        private List<string> photosDeletedDuringCopy;
    }
}