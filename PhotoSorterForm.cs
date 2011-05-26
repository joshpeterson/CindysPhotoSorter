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
        private DestinationDirectoryInformation destinationDirectory;
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

            this.destinationDirectory = null;

            this.copyItemsWorker = new BackgroundWorker();
            this.copyItemsWorker.WorkerReportsProgress = true;
            this.copyItemsWorker.DoWork += new DoWorkEventHandler(this.CopyItems);
            this.copyItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ItemCopyComplete);
            this.copyItemsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ClearProgressBar);
            this.copyItemsWorker.ProgressChanged += new ProgressChangedEventHandler(this.UpdateCopyItemsProgress);

            this.lastSelectedFindItemsPath = null;
            this.lastCopyItemsPath = null;

            this.StatusStripLabel.Text = "Click Find Photos and Videos.";
        }

        #endregion

        #region FindItemsOnClick handler and helpers

        private void FindItemsOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (string.IsNullOrEmpty(this.lastSelectedFindItemsPath))
            {
                folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            }
            else
            {
                folderBrowser.SelectedPath = this.lastSelectedFindItemsPath;
            }

            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.lastSelectedFindItemsPath = folderBrowser.SelectedPath;

                this.listViewImageList = new ImageList();
                this.listViewItems.Clear();
                this.ItemDisplay.Clear();

                this.findItemsWorker.RunWorkerAsync(folderBrowser.SelectedPath);

                this.StatusStripLabel.Text = string.Format("Finding photos and videos in {0}...", folderBrowser.SelectedPath);

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
            this.ItemDisplay.LargeImageList = this.listViewImageList;
            foreach (ListViewItem item in this.listViewItems)
            {
                this.ItemDisplay.Items.Add(item);
            }

            if (this.ItemDisplay.Items.Count == 0)
            {
                this.StatusStripLabel.Text = "No photos or videos found";
                MessageBox.Show(this, "No photos or videos found!", "Find Photos and Videos", MessageBoxButtons.OK);
            }
            else if (this.ItemDisplay.Items.Count == 1)
            {
                this.StatusStripLabel.Text = string.Format("{0} item found.", this.ItemDisplay.Items.Count);
            }
            else
            {
                this.StatusStripLabel.Text = string.Format("{0} items found.  Select items to copy.", this.ItemDisplay.Items.Count);
            }
        }

        private void UpdateFindItemsProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
            this.StatusStripLabel.Text = string.Format("Processed item {0}", Path.GetFullPath((string)e.UserState));
        }

        private void SortItemDisplay(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ItemDisplay.Sort();
        }

        #endregion

        #region DirectoryOnClick

        private void DirectoryOnClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (string.IsNullOrEmpty(this.lastCopyItemsPath))
            {
                folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            else
            {
                folderBrowser.SelectedPath = this.lastCopyItemsPath;
            }

            DialogResult folderResult = folderBrowser.ShowDialog();

            if (folderResult == DialogResult.OK)
            {
                this.lastCopyItemsPath = folderBrowser.SelectedPath;

                FileNamePrefixForm fileNamePrefixForm = new FileNamePrefixForm(folderBrowser.SelectedPath);
                DialogResult fileNamePrefixResult = fileNamePrefixForm.ShowDialog();

                if (fileNamePrefixResult == DialogResult.OK)
                {
                    if (this.destinationDirectory != null)
                    {
                        this.destinationDirectory.RemoveFromPanel(this.DestinationDirectoriesPanel);
                    }

                    this.destinationDirectory = new DestinationDirectoryInformation(folderBrowser.SelectedPath, fileNamePrefixForm.FileNamePrefix);
                    destinationDirectory.AddToPanel(this.DestinationDirectoriesPanel);
                }
            }

            this.ItemDisplay.Select();

            this.StatusStripLabel.Text = "Click Copy Photos and Videos to copy photos and videos to the destination directory.";
        }

        #endregion

        #region CopyItemsOnClick handler and helpers

        private void CopyItemsOnClick(object sender, EventArgs e)
        {
            if (this.ItemDisplay.SelectedItems.Count == 0)
            {
                this.StatusStripLabel.Text = "Please select at least one item to copy.";
                MessageBox.Show(this, "Please select at least one item to copy.", "Select an Item", MessageBoxButtons.OK);
                return;
            }

            if (this.destinationDirectory == null)
            {
                this.StatusStripLabel.Text = "Please add a destination directory.";
                MessageBox.Show(this, "Please add a destination directory.", "Select Directory", MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrEmpty(destinationDirectory.FileNamePrefix))
            {
                this.StatusStripLabel.Text = "Please select a file name prefix for the destination directory.";
                MessageBox.Show(this, "Please select a file name prefix for the destination directory.", "Select Prefix", MessageBoxButtons.OK);
                return;
            }

            this.ItemDisplay.Select();

            int numPhotos = this.ItemDisplay.SelectedItems.Count;
            if (numPhotos > 0)
            {
                List<string> itemFileNames = new List<string>();
                foreach (ListViewItem item in this.ItemDisplay.SelectedItems)
                {
                    itemFileNames.Add(item.Tag.ToString());
                }

                CopyItemInformation copyInformation = new CopyItemInformation();
                copyInformation.ItemFileNames = itemFileNames;
                copyInformation.DeleteSourceItemAfterCopy = this.DeleteCheckbox.Checked;
                copyInformation.NumberOfItems = numPhotos;

                this.copyItemsWorker.RunWorkerAsync(copyInformation);

                this.StatusStripLabel.Text = "Copying items...";
            }
        }

        private void CopyItems(object sender, DoWorkEventArgs e)
        {
            int highestPercentageReached = 0;
            int fileNameCounter = 1;
            int i = 0;
            CopyItemInformation copyInformation = (CopyItemInformation)e.Argument;
            foreach (string fileName in copyInformation.ItemFileNames)
            {
                string newFileName = Path.Combine(this.destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", this.destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfItems), Path.GetExtension(fileName)));
                while (File.Exists(newFileName))
                {
                    fileNameCounter++;
                    newFileName = Path.Combine(this.destinationDirectory.DestinationDirectoryName, string.Format("{0} {1}{2}", this.destinationDirectory.FileNamePrefix, this.GetFileNameNumber(fileNameCounter, copyInformation.NumberOfItems), Path.GetExtension(fileName)));
                }

                File.Copy(fileName, newFileName);

                if (copyInformation.DeleteSourceItemAfterCopy)
                {
                    File.Delete(fileName);
                    this.itemsDeletedDuringCopy.Add(fileName);
                }

                int percentComplete = (int)((float)i / (float)copyInformation.NumberOfItems * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    this.copyItemsWorker.ReportProgress(percentComplete);
                }

                fileNameCounter++;
                i++;
            }
        }

        private void RemoveDeletedItems()
        {
            foreach (string fileName in this.itemsDeletedDuringCopy)
            {
                foreach (ListViewItem item in this.ItemDisplay.Items)
                {
                    if ((string)item.Tag == fileName)
                    {
                        this.ItemDisplay.Items.Remove(item);
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

            this.ItemDisplay.Sort();

            if (this.DeleteCheckbox.Checked)
            {
                this.DeleteCheckbox.Checked = false;
            }

            this.StatusStripLabel.Text = "Photos and videos copied successfully.";
        }

        private void UpdateCopyItemsProgress(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
        }

        #endregion

        #region Generic BackgroundWorker callbacks

        private void ClearProgressBar(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressBar.Value = 0;
        }

        #endregion

        #region Various event handlers for controls

        private void OnSelectionedIndexChanged(object sender, EventArgs e)
        {
            this.StatusStripLabel.Text = string.Format("{0} of {1} items selected.", this.ItemDisplay.SelectedItems.Count, this.ItemDisplay.Items.Count);
        }

        private void DeleteCheckboxOnCheckChanged(object sender, EventArgs e)
        {
            this.ItemDisplay.Select();
        }

        private void OnItemDoubleClick(object sender, EventArgs e)
        {
            if (this.ItemDisplay.SelectedItems.Count == 1)
            {
                string clickedItemFileName = this.ItemDisplay.SelectedItems[0].Tag.ToString();
                Process.Start(clickedItemFileName);
            }
        }

        #endregion
    }
}