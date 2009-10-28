namespace PhotoSorter
{
    partial class CopyFilesForm
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
            this.AddDirectory = new System.Windows.Forms.Button();
            this.CopyPhotos = new System.Windows.Forms.Button();
            this.DeleteCheckbox = new System.Windows.Forms.CheckBox();
            this.DestinationDirectoriesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.DestinationHeaderLabel = new System.Windows.Forms.Label();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddDirectory
            // 
            this.AddDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDirectory.Location = new System.Drawing.Point(10, 12);
            this.AddDirectory.Name = "AddDirectory";
            this.AddDirectory.Size = new System.Drawing.Size(110, 23);
            this.AddDirectory.TabIndex = 4;
            this.AddDirectory.Text = "Add Destination";
            this.AddDirectory.UseVisualStyleBackColor = true;
            this.AddDirectory.Click += new System.EventHandler(this.AddDirectoryOnClick);
            // 
            // CopyPhotos
            // 
            this.CopyPhotos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyPhotos.Location = new System.Drawing.Point(126, 12);
            this.CopyPhotos.Name = "CopyPhotos";
            this.CopyPhotos.Size = new System.Drawing.Size(109, 23);
            this.CopyPhotos.TabIndex = 6;
            this.CopyPhotos.Text = "Copy Photos";
            this.CopyPhotos.UseVisualStyleBackColor = true;
            this.CopyPhotos.Click += new System.EventHandler(this.CopyPhotosOnClick);
            // 
            // DeleteCheckbox
            // 
            this.DeleteCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteCheckbox.AutoSize = true;
            this.DeleteCheckbox.Location = new System.Drawing.Point(241, 16);
            this.DeleteCheckbox.Name = "DeleteCheckbox";
            this.DeleteCheckbox.Size = new System.Drawing.Size(118, 17);
            this.DeleteCheckbox.TabIndex = 7;
            this.DeleteCheckbox.Text = "Remove After Copy";
            this.DeleteCheckbox.UseVisualStyleBackColor = true;
            // 
            // DestinationDirectoriesPanel
            // 
            this.DestinationDirectoriesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationDirectoriesPanel.AutoScroll = true;
            this.DestinationDirectoriesPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DestinationDirectoriesPanel.Location = new System.Drawing.Point(5, 58);
            this.DestinationDirectoriesPanel.Name = "DestinationDirectoriesPanel";
            this.DestinationDirectoriesPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.DestinationDirectoriesPanel.Size = new System.Drawing.Size(598, 111);
            this.DestinationDirectoriesPanel.TabIndex = 8;
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel,
            this.ProgressBar});
            this.StatusStrip.Location = new System.Drawing.Point(0, 174);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(615, 22);
            this.StatusStrip.TabIndex = 9;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(498, 17);
            this.StatusStripLabel.Spring = true;
            this.StatusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // DestinationHeaderLabel
            // 
            this.DestinationHeaderLabel.AutoSize = true;
            this.DestinationHeaderLabel.Location = new System.Drawing.Point(12, 38);
            this.DestinationHeaderLabel.Name = "DestinationHeaderLabel";
            this.DestinationHeaderLabel.Size = new System.Drawing.Size(68, 13);
            this.DestinationHeaderLabel.TabIndex = 0;
            this.DestinationHeaderLabel.Text = "Destinations:";
            // 
            // CopyFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 196);
            this.Controls.Add(this.DestinationHeaderLabel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.DestinationDirectoriesPanel);
            this.Controls.Add(this.DeleteCheckbox);
            this.Controls.Add(this.CopyPhotos);
            this.Controls.Add(this.AddDirectory);
            this.Name = "CopyFilesForm";
            this.Text = "Copy Photos";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddDirectory;
        private System.Windows.Forms.Button CopyPhotos;
        private System.Windows.Forms.CheckBox DeleteCheckbox;
        private System.Windows.Forms.FlowLayoutPanel DestinationDirectoriesPanel;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Label DestinationHeaderLabel;
    }
}