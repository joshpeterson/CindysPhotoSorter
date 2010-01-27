﻿namespace PhotoSorter
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
            this.CopyPhotos = new System.Windows.Forms.Button();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.AddDestination = new System.Windows.Forms.Button();
            this.DeleteCheckbox = new System.Windows.Forms.CheckBox();
            this.DestinationDirectoriesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhotoDisplay
            // 
            this.PhotoDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PhotoDisplay.Location = new System.Drawing.Point(12, 40);
            this.PhotoDisplay.Name = "PhotoDisplay";
            this.PhotoDisplay.Size = new System.Drawing.Size(646, 398);
            this.PhotoDisplay.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.PhotoDisplay.TabIndex = 0;
            this.PhotoDisplay.UseCompatibleStateImageBehavior = false;
            this.PhotoDisplay.SelectedIndexChanged += new System.EventHandler(this.OnSelectionedIndexChanged);
            // 
            // SourceDirectory
            // 
            this.SourceDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceDirectory.Location = new System.Drawing.Point(12, 11);
            this.SourceDirectory.Name = "SourceDirectory";
            this.SourceDirectory.Size = new System.Drawing.Size(78, 23);
            this.SourceDirectory.TabIndex = 1;
            this.SourceDirectory.Text = "Find Photos";
            this.SourceDirectory.UseVisualStyleBackColor = true;
            this.SourceDirectory.Click += new System.EventHandler(this.SourceDirectoryOnClick);
            // 
            // CopyPhotos
            // 
            this.CopyPhotos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyPhotos.Location = new System.Drawing.Point(12, 444);
            this.CopyPhotos.Name = "CopyPhotos";
            this.CopyPhotos.Size = new System.Drawing.Size(78, 23);
            this.CopyPhotos.TabIndex = 5;
            this.CopyPhotos.Text = "Copy Photos";
            this.CopyPhotos.UseVisualStyleBackColor = true;
            this.CopyPhotos.Click += new System.EventHandler(this.CopyPhotosOnClick);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel,
            this.ProgressBar});
            this.StatusStrip.Location = new System.Drawing.Point(0, 469);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(670, 22);
            this.StatusStrip.TabIndex = 7;
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(553, 17);
            this.StatusStripLabel.Spring = true;
            this.StatusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // AddDestination
            // 
            this.AddDestination.Location = new System.Drawing.Point(96, 12);
            this.AddDestination.Name = "AddDestination";
            this.AddDestination.Size = new System.Drawing.Size(75, 23);
            this.AddDestination.TabIndex = 4;
            this.AddDestination.Text = "Destination";
            this.AddDestination.UseVisualStyleBackColor = true;
            this.AddDestination.Click += new System.EventHandler(this.AddDirectoryOnClick);
            // 
            // DeleteCheckbox
            // 
            this.DeleteCheckbox.AutoSize = true;
            this.DeleteCheckbox.Location = new System.Drawing.Point(96, 448);
            this.DeleteCheckbox.Name = "DeleteCheckbox";
            this.DeleteCheckbox.Size = new System.Drawing.Size(118, 17);
            this.DeleteCheckbox.TabIndex = 7;
            this.DeleteCheckbox.Text = "Remove After Copy";
            this.DeleteCheckbox.UseVisualStyleBackColor = true;
            this.DeleteCheckbox.CheckedChanged += new System.EventHandler(this.DeleteCheckboxOnCheckChanged);
            // 
            // DestinationDirectoriesPanel
            // 
            this.DestinationDirectoriesPanel.Location = new System.Drawing.Point(174, 11);
            this.DestinationDirectoriesPanel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.DestinationDirectoriesPanel.Name = "DestinationDirectoriesPanel";
            this.DestinationDirectoriesPanel.Size = new System.Drawing.Size(484, 22);
            this.DestinationDirectoriesPanel.TabIndex = 8;
            // 
            // PhotoSorter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 491);
            this.Controls.Add(this.DestinationDirectoriesPanel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.CopyPhotos);
            this.Controls.Add(this.SourceDirectory);
            this.Controls.Add(this.DeleteCheckbox);
            this.Controls.Add(this.PhotoDisplay);
            this.Controls.Add(this.AddDestination);
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
        private System.Windows.Forms.Button CopyPhotos;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Button AddDestination;
        private System.Windows.Forms.CheckBox DeleteCheckbox;
        private System.Windows.Forms.FlowLayoutPanel DestinationDirectoriesPanel;
    }
}

