namespace PhotoSorter
{
    partial class PhotoSorter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoSorter));
            this.PhotoDisplay = new System.Windows.Forms.ListView();
            this.SourceDirectory = new System.Windows.Forms.Button();
            this.AddDirectory = new System.Windows.Forms.Button();
            this.DestinationDirectoriesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.CopyPhotos = new System.Windows.Forms.Button();
            this.DeleteCheckbox = new System.Windows.Forms.CheckBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.RemoveAllDestinations = new System.Windows.Forms.Button();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhotoDisplay
            // 
            this.PhotoDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PhotoDisplay.Location = new System.Drawing.Point(12, 12);
            this.PhotoDisplay.Name = "PhotoDisplay";
            this.PhotoDisplay.Size = new System.Drawing.Size(530, 454);
            this.PhotoDisplay.TabIndex = 0;
            this.PhotoDisplay.UseCompatibleStateImageBehavior = false;
            // 
            // SourceDirectory
            // 
            this.SourceDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceDirectory.Location = new System.Drawing.Point(548, 12);
            this.SourceDirectory.Name = "SourceDirectory";
            this.SourceDirectory.Size = new System.Drawing.Size(110, 23);
            this.SourceDirectory.TabIndex = 1;
            this.SourceDirectory.Text = "Find Photos";
            this.SourceDirectory.UseVisualStyleBackColor = true;
            this.SourceDirectory.Click += new System.EventHandler(this.SourceDirectoryOnClick);
            // 
            // AddDirectory
            // 
            this.AddDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDirectory.Location = new System.Drawing.Point(548, 41);
            this.AddDirectory.Name = "AddDirectory";
            this.AddDirectory.Size = new System.Drawing.Size(110, 23);
            this.AddDirectory.TabIndex = 3;
            this.AddDirectory.Text = "Add Destination";
            this.AddDirectory.UseVisualStyleBackColor = true;
            this.AddDirectory.Click += new System.EventHandler(this.AddDirectoryOnClick);
            // 
            // DestinationDirectoriesPanel
            // 
            this.DestinationDirectoriesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationDirectoriesPanel.AutoScroll = true;
            this.DestinationDirectoriesPanel.Location = new System.Drawing.Point(551, 151);
            this.DestinationDirectoriesPanel.Name = "DestinationDirectoriesPanel";
            this.DestinationDirectoriesPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.DestinationDirectoriesPanel.Size = new System.Drawing.Size(118, 328);
            this.DestinationDirectoriesPanel.TabIndex = 4;
            // 
            // CopyPhotos
            // 
            this.CopyPhotos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyPhotos.Location = new System.Drawing.Point(548, 99);
            this.CopyPhotos.Name = "CopyPhotos";
            this.CopyPhotos.Size = new System.Drawing.Size(109, 23);
            this.CopyPhotos.TabIndex = 5;
            this.CopyPhotos.Text = "Copy Photos";
            this.CopyPhotos.UseVisualStyleBackColor = true;
            this.CopyPhotos.Click += new System.EventHandler(this.CopyPhotosOnClick);
            // 
            // DeleteCheckbox
            // 
            this.DeleteCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteCheckbox.AutoSize = true;
            this.DeleteCheckbox.Location = new System.Drawing.Point(548, 128);
            this.DeleteCheckbox.Name = "DeleteCheckbox";
            this.DeleteCheckbox.Size = new System.Drawing.Size(118, 17);
            this.DeleteCheckbox.TabIndex = 6;
            this.DeleteCheckbox.Text = "Remove After Copy";
            this.DeleteCheckbox.UseVisualStyleBackColor = true;
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 469);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(670, 22);
            this.StatusStrip.TabIndex = 7;
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // RemoveAllDestinations
            // 
            this.RemoveAllDestinations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveAllDestinations.Location = new System.Drawing.Point(548, 70);
            this.RemoveAllDestinations.Name = "RemoveAllDestinations";
            this.RemoveAllDestinations.Size = new System.Drawing.Size(109, 23);
            this.RemoveAllDestinations.TabIndex = 8;
            this.RemoveAllDestinations.Text = "Delete Destinations";
            this.RemoveAllDestinations.UseVisualStyleBackColor = true;
            this.RemoveAllDestinations.Click += new System.EventHandler(this.RemoveAllDestinationsOnClick);
            // 
            // PhotoSorter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 491);
            this.Controls.Add(this.RemoveAllDestinations);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.DeleteCheckbox);
            this.Controls.Add(this.CopyPhotos);
            this.Controls.Add(this.DestinationDirectoriesPanel);
            this.Controls.Add(this.AddDirectory);
            this.Controls.Add(this.SourceDirectory);
            this.Controls.Add(this.PhotoDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PhotoSorter";
            this.Text = "Cindy\'s Photo Sorter";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView PhotoDisplay;
        private System.Windows.Forms.Button SourceDirectory;
        private System.Windows.Forms.Button AddDirectory;
        private System.Windows.Forms.FlowLayoutPanel DestinationDirectoriesPanel;
        private System.Windows.Forms.Button CopyPhotos;
        private System.Windows.Forms.CheckBox DeleteCheckbox;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.Button RemoveAllDestinations;
    }
}

