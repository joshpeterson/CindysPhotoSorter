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
                //using (Bitmap image = new Bitmap(fileName))
                //{
                    //Image.GetThumbnailImageAbort dummy = null;
                    ImageData imageData = this.GetImageData(fileName);
                    images.Images.Add(imageData.Thumbnail);
                    ListViewItem item = new ListViewItem(string.Format("{0}\n{1}", Path.GetFileName(fileName), imageData.DateTaken != DateTime.MinValue ? imageData.DateTaken.ToString() : string.Empty));
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
                //}
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
	        // Last parameter tells GDI+ not the load the actual image data
                using (Image img = Image.FromStream(fs, false, false))
                {
                    // GDI+ throws an error if we try to read a property when the image
                    // doesn't have that property. Check to make sure the thumbnail property
                    // item exists.
                    bool thumbnailPropertyFound = false;
                    bool dateTakenPropertyFound = false;
                    for (int i = 0; i < img.PropertyIdList.Length; i++)
                    {
                        if (img.PropertyIdList[i] == 0x501B)
                        {
                            thumbnailPropertyFound = true;
                            if (dateTakenPropertyFound == true)
                            {
                                break;
                            }
                        }
                        if (img.PropertyIdList[i] == 306)
                        {
                            dateTakenPropertyFound = true;
                            if (thumbnailPropertyFound == true)
                            {
                                break;
                            }
                        }
                    }

                    if (!thumbnailPropertyFound)
                    {
                        // Do slow image thumbnail here
                        throw new Exception("problem");
                    }

                    PropertyItem p = img.GetPropertyItem(0x501B);

                    // The image data is in the form of a byte array. Write all 
                    // the bytes to a stream and create a new image from that stream
                    byte[] imageBytes = p.Value;
                    MemoryStream stream = new MemoryStream(imageBytes.Length);
                    stream.Write(imageBytes, 0, imageBytes.Length);

                    ImageData imageData;

                    imageData.Thumbnail = Image.FromStream(stream);
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

            this.StatusStripLabel.Text = "Select photos to copy.";
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
                PropertyItem propertyItem = image.GetPropertyItem(0x501B);

                byte[] imageBytes = propertyItem.Value;
                MemoryStream stream = new MemoryStream(imageBytes.Length);
                stream.Write(imageBytes, 0, imageBytes.Length);

                return Image.FromStream(stream);
            }
            catch (ArgumentException)
            {
                using (Bitmap fullImage = new Bitmap(fileName))
                {
                    Image.GetThumbnailImageAbort dummy = null;
                }
            }
        }

        private DateTime GetImageDateTaken(Image image)
        {
            try
            {
                PropertyItem propertyItem = image.GetPropertyItem(306);

                string dateTaken = Encoding.UTF8.GetString(propertyItem.Value).Trim();
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