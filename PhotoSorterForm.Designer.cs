namespace PhotoSorter
{
    partial class PhotoSorterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoSorterForm));
            this.ItemDisplay = new System.Windows.Forms.ListView();
            this.SourceDirectory = new System.Windows.Forms.Button();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.AddDestination = new System.Windows.Forms.Button();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemDisplay
            // 
            this.ItemDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemDisplay.Location = new System.Drawing.Point(12, 39);
            this.ItemDisplay.Name = "ItemDisplay";
            this.ItemDisplay.Size = new System.Drawing.Size(646, 427);
            this.ItemDisplay.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ItemDisplay.TabIndex = 0;
            this.ItemDisplay.UseCompatibleStateImageBehavior = false;
            this.ItemDisplay.SelectedIndexChanged += new System.EventHandler(this.OnSelectionedIndexChanged);
            this.ItemDisplay.DoubleClick += new System.EventHandler(this.OnItemDoubleClick);
            // 
            // SourceDirectory
            // 
            this.SourceDirectory.Location = new System.Drawing.Point(12, 10);
            this.SourceDirectory.Name = "SourceDirectory";
            this.SourceDirectory.Size = new System.Drawing.Size(128, 23);
            this.SourceDirectory.TabIndex = 1;
            this.SourceDirectory.Text = "Find photos and videos";
            this.SourceDirectory.UseVisualStyleBackColor = true;
            this.SourceDirectory.Click += new System.EventHandler(this.FindItemsOnClick);
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
            this.AddDestination.Location = new System.Drawing.Point(146, 10);
            this.AddDestination.Name = "AddDestination";
            this.AddDestination.Size = new System.Drawing.Size(91, 23);
            this.AddDestination.TabIndex = 4;
            this.AddDestination.Text = "Copy...";
            this.AddDestination.UseVisualStyleBackColor = true;
            this.AddDestination.Click += new System.EventHandler(this.DirectoryOnClick);
            // 
            // PhotoSorterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 491);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.SourceDirectory);
            this.Controls.Add(this.ItemDisplay);
            this.Controls.Add(this.AddDestination);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PhotoSorterForm";
            this.Text = "Cindy\'s Photo Sorter";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ItemDisplay;
        private System.Windows.Forms.Button SourceDirectory;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Button AddDestination;
    }
}

