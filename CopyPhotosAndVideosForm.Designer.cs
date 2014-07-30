namespace CindysPhotoSorter
{
    partial class CopyPhotosAndVideosForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyPhotosAndVideosForm));
            this.fileNamePrefixLabel = new System.Windows.Forms.Label();
            this.fileNamePrefixTextBox = new System.Windows.Forms.TextBox();
            this.copyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteCheckbox = new System.Windows.Forms.CheckBox();
            this.destinationDirectoryLabel = new System.Windows.Forms.Label();
            this.destinatonDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.chooseDestinationDirectoryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileNamePrefixLabel
            // 
            this.fileNamePrefixLabel.AutoSize = true;
            this.fileNamePrefixLabel.Location = new System.Drawing.Point(13, 43);
            this.fileNamePrefixLabel.Name = "fileNamePrefixLabel";
            this.fileNamePrefixLabel.Size = new System.Drawing.Size(83, 13);
            this.fileNamePrefixLabel.TabIndex = 1;
            this.fileNamePrefixLabel.Text = "File name prefix:";
            // 
            // fileNamePrefixTextBox
            // 
            this.fileNamePrefixTextBox.Location = new System.Drawing.Point(98, 40);
            this.fileNamePrefixTextBox.Name = "fileNamePrefixTextBox";
            this.fileNamePrefixTextBox.Size = new System.Drawing.Size(386, 20);
            this.fileNamePrefixTextBox.TabIndex = 2;
            // 
            // copyButton
            // 
            this.copyButton.Enabled = false;
            this.copyButton.Location = new System.Drawing.Point(328, 66);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 23);
            this.copyButton.TabIndex = 3;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButtonOnClick);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(409, 66);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButtonOnClick);
            // 
            // deleteCheckbox
            // 
            this.deleteCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteCheckbox.AutoSize = true;
            this.deleteCheckbox.Location = new System.Drawing.Point(16, 70);
            this.deleteCheckbox.Name = "deleteCheckbox";
            this.deleteCheckbox.Size = new System.Drawing.Size(137, 17);
            this.deleteCheckbox.TabIndex = 8;
            this.deleteCheckbox.Text = "Remove files after copy";
            this.deleteCheckbox.UseVisualStyleBackColor = true;
            // 
            // destinationDirectoryLabel
            // 
            this.destinationDirectoryLabel.AutoSize = true;
            this.destinationDirectoryLabel.Location = new System.Drawing.Point(13, 9);
            this.destinationDirectoryLabel.Name = "destinationDirectoryLabel";
            this.destinationDirectoryLabel.Size = new System.Drawing.Size(108, 13);
            this.destinationDirectoryLabel.TabIndex = 9;
            this.destinationDirectoryLabel.Text = "Destination Directory:";
            // 
            // destinatonDirectoryTextBox
            // 
            this.destinatonDirectoryTextBox.Location = new System.Drawing.Point(124, 6);
            this.destinatonDirectoryTextBox.Name = "destinatonDirectoryTextBox";
            this.destinatonDirectoryTextBox.ReadOnly = true;
            this.destinatonDirectoryTextBox.Size = new System.Drawing.Size(328, 20);
            this.destinatonDirectoryTextBox.TabIndex = 10;
            // 
            // chooseDestinationDirectoryButton
            // 
            this.chooseDestinationDirectoryButton.Location = new System.Drawing.Point(458, 5);
            this.chooseDestinationDirectoryButton.Name = "chooseDestinationDirectoryButton";
            this.chooseDestinationDirectoryButton.Size = new System.Drawing.Size(25, 23);
            this.chooseDestinationDirectoryButton.TabIndex = 11;
            this.chooseDestinationDirectoryButton.Text = "...";
            this.chooseDestinationDirectoryButton.UseVisualStyleBackColor = true;
            this.chooseDestinationDirectoryButton.Click += new System.EventHandler(this.chooseDestinationDirectoryButton_Click);
            // 
            // CopyPhotosAndVideosForm
            // 
            this.AcceptButton = this.copyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(496, 101);
            this.Controls.Add(this.chooseDestinationDirectoryButton);
            this.Controls.Add(this.destinatonDirectoryTextBox);
            this.Controls.Add(this.destinationDirectoryLabel);
            this.Controls.Add(this.deleteCheckbox);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.fileNamePrefixTextBox);
            this.Controls.Add(this.fileNamePrefixLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CopyPhotosAndVideosForm";
            this.ShowInTaskbar = false;
            this.Text = "Copy photos and videos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label fileNamePrefixLabel;
        private System.Windows.Forms.TextBox fileNamePrefixTextBox;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox deleteCheckbox;
        private System.Windows.Forms.Label destinationDirectoryLabel;
        private System.Windows.Forms.TextBox destinatonDirectoryTextBox;
        private System.Windows.Forms.Button chooseDestinationDirectoryButton;
    }
}