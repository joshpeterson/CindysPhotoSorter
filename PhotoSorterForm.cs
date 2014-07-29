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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PhotoSorter
{
    public partial class PhotoSorterForm : Form
    {
        #region Private members

        private ImageList listViewImageList;
        private List<ListViewItem> listViewItems;
        private BackgroundWorker findItemsWorker;
        private BackgroundWorker copyItemsWorker;
        private List<string> itemsDeletedDuringCopy;
        private List<DestinationDirectoryInformation> destinationDirectories;
        private string lastSelectedFindItemsPath;
        private string lastCopyItemsPath;

        #endregion

        #region Constructor

        public PhotoSorterForm()
        {
            InitializeComponent();

            this.listViewItems = new List<ListViewItem>();
            this.itemsDeletedDuringCopy = new List<string>();
            
            this.findItemsWorker = new BackgroundWorker();
            this.findItemsWorker.WorkerReportsProgress = true;
            this.findItemsWorker.DoWork += new DoWorkEventHandler(this.GetItemsInDirectory);
            this.findItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SetUpListView);
            this.findItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.findItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SortItemDisplay);
            this.findItemsWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateFindItemsProgress);

            this.destinationDirectories = new List<DestinationDirectoryInformation>();

            this.copyItemsWorker = new BackgroundWorker();
            this.copyItemsWorker.WorkerReportsProgress = true;
            this.copyItemsWorker.DoWork += new DoWorkEventHandler(this.CopyItems);
            this.copyItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ItemCopyComplete);
            this.copyItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.copyItemsWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateCopyItemsProgress);

            this.lastSelectedFindItemsPath = null;
            this.lastCopyItemsPath = null;

            this.statusStripLabel.Text = "Click \"Find photos and videos\".";
        }

        #endregion

        #region FindItemsOnClick handler and helpers

        private void FindItemsOnClick(object sender, EventArgs e)
        {
            var folderBrowser = new Ookii.Dialogs.VistaFolderBrowserDialog();

            if (string.IsNullOrEmpty(this.lastSelectedFindItemsPath))
                folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            else
                folderBrowser.SelectedPath = this.lastSelectedFindItemsPath;

            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.lastSelectedFindItemsPath = folderBrowser.SelectedPath;

                this.listViewImageList = new ImageList();
                this.listViewItems.Clear();
                this.itemDisplay.Clear();

                this.findItemsWorker.RunWorkerAsync(folderBrowser.SelectedPath);

                this.statusStripLabel.Text = string.Format("Finding photos and videos in {0}...", folderBrowser.SelectedPath);

            }
        }

        private void GetItemsInDirectory(object sender, DoWorkEventArgs e)
        {
            this.listViewImageList.ImageSize = new Size(128, 96);
            this.listViewImageList.ColorDepth = ColorDepth.Depth24Bit;

            string directoryName = (string)e.Argument;
            int i = 0;
            int highestPercentageReached = 0;
            IEnumerable<string> fileNamesToListInDirectory = GetFileNamesToListInDirectory(directoryName);
            int numImages = fileNamesToListInDirectory.Count();
            foreach (string fileName in fileNamesToListInDirectory)
            {
                ListViewItem item = null;

                if (this.IsImageFile(fileName))
                {
                    ImageData imageData = this.GetImageData(fileName);
                    this.listViewImageList.Images.Add(imageData.Thumbnail);
                    item = new ListViewItem(string.Format("{0}\n{1}", Path.GetFileName(fileName), imageData.DateTaken != DateTime.MinValue ? imageData.DateTaken.ToString() : "Unknown date"));
                }
                else
                {
                    SHFILEINFO shinfo = new SHFILEINFO();
                    IntPtr hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo,
                                  (uint)Marshal.SizeOf(shinfo),
                                   Win32.SHGFI_ICON |
                                   Win32.SHGFI_LARGEICON);
                    Icon myIcon = Icon.FromHandle(shinfo.hIcon);

                    this.listViewImageList.Images.Add(myIcon);
                    item = new ListViewItem(Path.GetFileName(fileName));
                }

                item.ImageIndex = i;
                item.Tag = fileName;
                this.listViewItems.Add(item);
                i++;

                int percentComplete = (int)((float)i / (float)numImages * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    this.findItemsWorker.ReportProgress(percentComplete, fileName);
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

        private IEnumerable<string> GetFileNamesToListInDirectory(string directoryName)
        {
            List<string> fileNamesToList = new List<string>();
            foreach (string fileName in Directory.GetFiles(directoryName))
            {
                if (IsImageFile(fileName) || IsVideoFile(fileName))
                {
                    fileNamesToList.Add(fileName);
                }
            }
            return fileNamesToList;
        }

        private bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension == ".jpg" || extension == ".png";
        }

        private bool IsVideoFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension == ".mov";
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
                // We can't get the thumbnail resource, so load the entire item and generate
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

        #endregion

        #region findItemsWorker callbacks

        private void SetUpListView(object sender, RunWorkerCompletedEventArgs e)
        {
            this.itemDisplay.LargeImageList = this.listViewImageList;
            foreach (ListViewItem item in this.listViewItems)
                this.itemDisplay.Items.Add(item);

            if (this.itemDisplay.Items.Count == 0)
            {
                this.statusStripLabel.Text = "No photos or videos found";
                MessageBox.Show(this, "No photos or videos found!", "Find photos and videos", MessageBoxButtons.OK);
            }
            else if (this.itemDisplay.Items.Count == 1)
            {
                this.statusStripLabel.Text = string.Format("{0} item found.", this.itemDisplay.Items.Count);
            }
            else
            {
                this.statusStripLabel.Text = string.Format("{0} items found.  Select items to copy.", this.itemDisplay.Items.Count);
            }
        }

        private void UpdateFindItemsProgress(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            this.statusStripLabel.Text = string.Format("Processed item {0}", Path.GetFullPath((string)e.UserState));
        }

        private void SortItemDisplay(object sender, RunWorkerCompletedEventArgs e)
        {
            this.itemDisplay.Sort();
        }

        #endregion

        #region DirectoryOnClick

        private void DirectoryOnClick(object sender, EventArgs e)
        {
            CopyPhotosAndVideosForm fileNamePrefixForm = new CopyPhotosAndVideosForm(this.lastCopyItemsPath);
            if (fileNamePrefixForm.ShowDialog() == DialogResult.OK)
            {
                DestinationDirectoryInformation destinationDirectory = new DestinationDirectoryInformation(fileNamePrefixForm.DestinationDirectory, fileNamePrefixForm.FileNamePrefix);
                this.destinationDirectories.Add(destinationDirectory);

                CopyItems(fileNamePrefixForm.DeleteFilesAfterCopy);
            }

            this.itemDisplay.Select();
        }

        #endregion

        #region CopyItemsOnClick handler and helpers

        private void CopyItems(bool deleteFilesAfterCopy)
        {
            if (this.itemDisplay.SelectedItems.Count == 0)
            {
                this.statusStripLabel.Text = "Please select at least one item to copy.";
                MessageBox.Show(this, "Please select at least one item to copy.", "Select an Item", MessageBoxButtons.OK);
                return;
            }

            if (this.destinationDirectories.Count == 0)
            {
                this.statusStripLabel.Text = "Please add a destination directory.";
                MessageBox.Show(this, "Please add a destination directory.", "Select Directory", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrEmpty(destinationDirectories[0].FileNamePrefix))
            {
                this.statusStripLabel.Text = "Please select a file name prefix for the destination directory.";
                MessageBox.Show(this, "Please select a file name prefix for the destination directory.", "Select Prefix", MessageBoxButtons.OK);
                return;
            }

            this.itemDisplay.Select();

            int numPhotos = this.itemDisplay.SelectedItems.Count;
            if (numPhotos > 0)
            {
                List<string> itemFileNames = new List<string>();
                foreach (ListViewItem item in this.itemDisplay.SelectedItems)
                {
                    itemFileNames.Add(item.Tag.ToString());
                }

                CopyItemInformation copyInformation = new CopyItemInformation();
                copyInformation.ItemFileNames = itemFileNames;
                copyInformation.DeleteSourceItemAfterCopy = deleteFilesAfterCopy;
                copyInformation.NumberOfItems = numPhotos;

                this.copyItemsWorker.RunWorkerAsync(copyInformation);

                this.statusStripLabel.Text = "Copying items...";
            }
        }

        private void CopyItems(object sender, DoWorkEventArgs e)
        {
            int highestPercentageReached = 0;
            int currentFileToCopyNumber = 1;

            CopyItemInformation copyInformation = (CopyItemInformation)e.Argument;

            var numberOfFilesToCopy = copyInformation.NumberOfItems * this.destinationDirectories.Count;

            foreach (DestinationDirectoryInformation destinationDirectory in this.destinationDirectories)
            {
                int fileNameCounter = 1;
                foreach (string fileName in copyInformation.ItemFileNames)
                {
                    string newFileName = Path.Combine(destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfItems), Path.GetExtension(fileName)));
                    while (File.Exists(newFileName))
                    {
                        fileNameCounter++;
                        newFileName = Path.Combine(destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfItems), Path.GetExtension(fileName)));
                    }

                    File.Copy(fileName, newFileName);

                    int percentComplete = (int)(((float)currentFileToCopyNumber / (float)numberOfFilesToCopy) * 100);
                    if (percentComplete > highestPercentageReached)
                    {
                        highestPercentageReached = percentComplete;
                        this.copyItemsWorker.ReportProgress(percentComplete);
                    }

                    fileNameCounter++;
                    currentFileToCopyNumber++;
                }
            }

            foreach (var fileName in copyInformation.ItemFileNames)
            {
                if (copyInformation.DeleteSourceItemAfterCopy)
                {
                    File.Delete(fileName);
                    this.itemsDeletedDuringCopy.Add(fileName);
                }
            }
        }

        private void RemoveDeletedItems()
        {
            foreach (string fileName in this.itemsDeletedDuringCopy)
            {
                foreach (ListViewItem item in this.itemDisplay.Items)
                {
                    if ((string)item.Tag == fileName)
                    {
                        this.itemDisplay.Items.Remove(item);
                    }
                }
            }
            this.itemsDeletedDuringCopy.Clear();
        }

        private string GetFileNameNumber(int fileNameCounter, int numPhotos)
        {
            if (fileNameCounter < 100)
            {
                return fileNameCounter.ToString().PadLeft(3, '0');
            }

            return fileNameCounter.ToString();
        }

        struct CopyItemInformation
        {
            public List<string> ItemFileNames;
            public int NumberOfItems;
            public bool DeleteSourceItemAfterCopy;
        }

        #endregion

        #region copyItemsWorker callbacks

        private void ItemCopyComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            RemoveDeletedItems();

            this.itemDisplay.Sort();

            var copyMessage = string.Format("The photos and videos were copied successfully to the {0} directory.", this.destinationDirectories.First().DestinationDirectoryName);

            this.destinationDirectories.Clear();

            this.statusStripLabel.Text = "Photos and videos copied successfully.";
            MessageBox.Show(this, copyMessage, "Copy Complete", MessageBoxButtons.OK);
        }

        private void UpdateCopyItemsProgress(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        #endregion

        #region Generic BackgroundWorker callbacks

        private void ClearProgressBar(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = 0;
        }

        #endregion

        #region Various event handlers for controls

        private void OnSelectionedIndexChanged(object sender, EventArgs e)
        {
            this.statusStripLabel.Text = string.Format("{0} of {1} items selected.", this.itemDisplay.SelectedItems.Count, this.itemDisplay.Items.Count);
        }

        private void DeleteCheckboxOnCheckChanged(object sender, EventArgs e)
        {
            this.itemDisplay.Select();
        }

        private void OnItemDoubleClick(object sender, EventArgs e)
        {
            if (this.itemDisplay.SelectedItems.Count == 1)
            {
                string clickedItemFileName = this.itemDisplay.SelectedItems[0].Tag.ToString();
                Process.Start(clickedItemFileName);
            }
        }

        #endregion
    }
}