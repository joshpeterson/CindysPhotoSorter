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
            this.itemDisplay = new System.Windows.Forms.ListView();
            this.sourceDirectory = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.addDestination = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemDisplay
            // 
            this.itemDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemDisplay.Location = new System.Drawing.Point(12, 39);
            this.itemDisplay.Name = "itemDisplay";
            this.itemDisplay.Size = new System.Drawing.Size(646, 427);
            this.itemDisplay.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.itemDisplay.TabIndex = 0;
            this.itemDisplay.UseCompatibleStateImageBehavior = false;
            this.itemDisplay.SelectedIndexChanged += new System.EventHandler(this.OnSelectionedIndexChanged);
            this.itemDisplay.DoubleClick += new System.EventHandler(this.OnItemDoubleClick);
            // 
            // sourceDirectory
            // 
            this.sourceDirectory.Location = new System.Drawing.Point(12, 10);
            this.sourceDirectory.Name = "sourceDirectory";
            this.sourceDirectory.Size = new System.Drawing.Size(128, 23);
            this.sourceDirectory.TabIndex = 1;
            this.sourceDirectory.Text = "Find photos and videos";
            this.sourceDirectory.UseVisualStyleBackColor = true;
            this.sourceDirectory.Click += new System.EventHandler(this.FindItemsOnClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 469);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(670, 22);
            this.statusStrip.TabIndex = 7;
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(553, 17);
            this.statusStripLabel.Spring = true;
            this.statusStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // addDestination
            // 
            this.addDestination.Location = new System.Drawing.Point(146, 10);
            this.addDestination.Name = "addDestination";
            this.addDestination.Size = new System.Drawing.Size(91, 23);
            this.addDestination.TabIndex = 4;
            this.addDestination.Text = "Copy...";
            this.addDestination.UseVisualStyleBackColor = true;
            this.addDestination.Click += new System.EventHandler(this.DirectoryOnClick);
            // 
            // PhotoSorterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 491);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.sourceDirectory);
            this.Controls.Add(this.itemDisplay);
            this.Controls.Add(this.addDestination);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PhotoSorterForm";
            this.Text = "Cindy\'s Photo Sorter";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView itemDisplay;
        private System.Windows.Forms.Button sourceDirectory;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Button addDestination;
    }
}

