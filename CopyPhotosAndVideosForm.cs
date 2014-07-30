using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CindysPhotoSorter
{
    public partial class CopyPhotosAndVideosForm : Form
    {
        private string lastCopyItemsPath;

        public string FileNamePrefix {get; private set; }
        public bool DeleteFilesAfterCopy { get; private set; }
        public string DestinationDirectory { get; private set; }

        public CopyPhotosAndVideosForm(string lastCopyItemsPath)
        {
            InitializeComponent();
            this.lastCopyItemsPath = lastCopyItemsPath;
        }

        private void copyButtonOnClick(object sender, EventArgs e)
        {
            this.FileNamePrefix = this.fileNamePrefixTextBox.Text;
            this.DeleteFilesAfterCopy = this.deleteCheckbox.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButtonOnClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chooseDestinationDirectoryButton_Click(object sender, EventArgs e)
        {
            var folderBrowser = new Ookii.Dialogs.VistaFolderBrowserDialog();

            if (string.IsNullOrEmpty(this.lastCopyItemsPath))
                folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            else
                folderBrowser.SelectedPath = this.lastCopyItemsPath;

            DialogResult folderResult = folderBrowser.ShowDialog();

            if (folderResult == DialogResult.OK)
            {
                this.DestinationDirectory = folderBrowser.SelectedPath;
                this.destinatonDirectoryTextBox.Text = this.DestinationDirectory;
                this.copyButton.Enabled = true;
            }
        }
    }
}
