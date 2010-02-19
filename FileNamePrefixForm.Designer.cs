namespace PhotoSorter
{
    partial class FileNamePrefixForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileNamePrefixForm));
            this.destinationDirectoryLabel = new System.Windows.Forms.Label();
            this.fileNamePrefixLabel = new System.Windows.Forms.Label();
            this.fileNamePrefixTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // destinationDirectoryLabel
            // 
            this.destinationDirectoryLabel.AutoSize = true;
            this.destinationDirectoryLabel.Location = new System.Drawing.Point(13, 13);
            this.destinationDirectoryLabel.Name = "destinationDirectoryLabel";
            this.destinationDirectoryLabel.Size = new System.Drawing.Size(0, 13);
            this.destinationDirectoryLabel.TabIndex = 0;
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
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(328, 66);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButtonOnClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(409, 66);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButtonOnClick);
            // 
            // FileNamePrefixForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 101);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fileNamePrefixTextBox);
            this.Controls.Add(this.fileNamePrefixLabel);
            this.Controls.Add(this.destinationDirectoryLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileNamePrefixForm";
            this.ShowInTaskbar = false;
            this.Text = "File Name Prefix";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label destinationDirectoryLabel;
        private System.Windows.Forms.Label fileNamePrefixLabel;
        private System.Windows.Forms.TextBox fileNamePrefixTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}