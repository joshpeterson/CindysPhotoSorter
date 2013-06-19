using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoSorter
{
    public partial class FileNamePrefixForm : Form
    {
        private string fileNamePrefix;

        public FileNamePrefixForm(string destinationDirectory)
        {
            InitializeComponent();
            this.destinationDirectoryLabel.Text = string.Format("Directory: {0}", destinationDirectory);
        }

        public string FileNamePrefix
        {
            get
            {
                return this.fileNamePrefix;
            }
        }

        private void okButtonOnClick(object sender, EventArgs e)
        {
            this.fileNamePrefix = this.fileNamePrefixTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButtonOnClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
