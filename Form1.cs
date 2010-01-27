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
            this.FindPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateFindPhotosProgress);

            this.destinationDirectory = null;

            this.CopyPhotosWorker = new BackgroundWorker();
            this.CopyPhotosWorker.WorkerReportsProgress = true;
            this.CopyPhotosWorker.DoWork += new DoWorkEventHandler(this.CopyImages);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.PhotoCopyComplete);
            this.CopyPhotosWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.CopyPhotosWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateCopyPhotosProgress);

            this.selectPhotoDisplayDelay = new Timer();
            this.selectPhotoDisplayDelay.Interval = 500;
            this.selectPhotoDisplayDelay.Tick += new EventHandler(this.OnPhotoDisplayDelayTimerComplete);

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

        private void OnPhotoDisplayDelayTimerComplete(object sender, EventArgs e)
        {
            this.PhotoDisplay.Select();
            this.selectPhotoDisplayDelay.Stop();
        }

        private void CopyPhotosOnClick(object sender, EventArgs e)
        {
            if (this.PhotoDisplay.SelectedItems.Count == 0)
            {
                this.StatusStripLabel.Text = "Please select at least one photo to copy.";
                return;
            }

            if (this.destinationDirectory == null)
            {
                this.StatusStripLabel.Text = "Please add a destination directory.";
                return;
            }

            if (string.IsNullOrEmpty(destinationDirectory.FileNamePrefix))
            {
                this.StatusStripLabel.Text = "Please select a file name prefix for the destinations directory.";
                return;
            }

            this.PhotoDisplay.Select();

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

        private void UpdateFindPhotosProgress(object sender, ProgressChangedEventArgs e)
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

        private void AddDirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            folderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(folderBrowser.SelectedPath))
            {
                if (this.destinationDirectory != null)
                {
                    this.destinationDirectory.RemoveFromPanel(this.DestinationDirectoriesPanel);   
                }

                this.destinationDirectory = new DestinationDirectoryInformation(folderBrowser.SelectedPath, this.selectPhotoDisplayDelay);
                destinationDirectory.AddToPanel(this.DestinationDirectoriesPanel);
            }

            this.PhotoDisplay.Select();

            this.StatusStripLabel.Text = "Click Copy Photos to copy photos to the destination directory.";
        }

        private void DeleteCheckboxOnCheckChanged(object sender, EventArgs e)
        {
            this.PhotoDisplay.Select();
        }

        private void CopyImages(object sender, DoWorkEventArgs e)
        {
            int highestPercentageReached = 0;
            int fileNameCounter = 1;
            int i = 0;
            CopyPhotoInformation copyInformation = (CopyPhotoInformation)e.Argument;
            foreach (string fileName in copyInformation.PhotoFileNames)
            {
                string newFileName = Path.Combine(this.destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", this.destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfPhotos), Path.GetExtension(fileName)));
                while (File.Exists(newFileName))
                {
                    fileNameCounter++;
                    newFileName = Path.Combine(this.destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", this.destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfPhotos), Path.GetExtension(fileName)));
                }

                File.Copy(fileName, newFileName);

                if (copyInformation.DeleteSourcePhotoAfterCopy)
                {
                    File.Delete(fileName);
                    this.OnPhotoDeleted(fileName);
                }

                int percentComplete = (int)((float)i / (float)copyInformation.NumberOfPhotos * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    this.CopyPhotosWorker.ReportProgress(percentComplete);
                }

                fileNameCounter++;
                i++;
            }
        }

        private void PhotoCopyComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            RemoveDeletedPhotos();

            this.PhotoDisplay.Sort();

            if (this.DeleteCheckbox.Checked)
            {
                this.DeleteCheckbox.Checked = false;
            }

            this.StatusStripLabel.Text = "Photos copied successfully.";
        }

        private void UpdateCopyPhotosProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
        }

        private string GetFileNameNumber(int fileNameCounter, int numPhotos)
        {
            if (fileNameCounter < 100)
            {
                return fileNameCounter.ToString().PadLeft(3, '0');
            }

            return fileNameCounter.ToString();
        }

        struct CopyPhotoInformation
        {
            public List<string> PhotoFileNames;
            public int NumberOfPhotos;
            public bool DeleteSourcePhotoAfterCopy;
        }

        class DestinationDirectoryInformation
        {
            public DestinationDirectoryInformation(string destinationDirectoryName, Timer keyPressDelay)
            {
                this.keyPressDelay = keyPressDelay;

                this.destinationDirectoryName = Path.GetFullPath(destinationDirectoryName);

                this.destinationDirectoryDisplay = new Label();
                this.destinationDirectoryDisplay.Height = 1;
                this.destinationDirectoryDisplay.AutoSize = true;
                this.destinationDirectoryDisplay.Text = this.destinationDirectoryName;
                this.destinationDirectoryDisplay.Padding = new Padding(0, 6, 0, 0);

                this.fileNamePrefixLabel = new Label();
                this.fileNamePrefixLabel.Height = 1;
                this.fileNamePrefixLabel.AutoSize = true;
                this.fileNamePrefixLabel.Text = "File name:";
                this.fileNamePrefixLabel.Padding = new Padding(0, 6, 0, 0);

                this.fileNamePrefix = new TextBox();

                this.fileNamePrefix.KeyPress += new KeyPressEventHandler(this.FileNamePrefixOnKeyPress);
            }

            public void AddToPanel(FlowLayoutPanel parent)
            {
                parent.Height = 1;
                parent.AutoSize = true;

                parent.Controls.Add(this.destinationDirectoryDisplay);
                parent.Controls.Add(this.fileNamePrefixLabel);
                parent.Controls.Add(this.fileNamePrefix);
            }

            public void RemoveFromPanel(FlowLayoutPanel parent)
            {
                parent.Controls.Remove(this.fileNamePrefix);
                parent.Controls.Remove(this.fileNamePrefixLabel);
                parent.Controls.Remove(this.destinationDirectoryDisplay);
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

            private void FileNamePrefixOnKeyPress(object sender, EventArgs e)
            {
                this.keyPressDelay.Start();
            }

            private string destinationDirectoryName;
            private Label destinationDirectoryDisplay;
            private Label fileNamePrefixLabel;
            private TextBox fileNamePrefix;
            private Timer keyPressDelay;
        }

        private ImageList images;
        private List<ListViewItem> items;
        private BackgroundWorker FindPhotosWorker;
        private List<string> photosDeletedDuringCopy;
        private DestinationDirectoryInformation destinationDirectory;
        private BackgroundWorker CopyPhotosWorker;
        private Timer selectPhotoDisplayDelay;
    }
}