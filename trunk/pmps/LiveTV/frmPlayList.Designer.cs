namespace LiveTV
{
    partial class frmPlayList
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColMediaName = new BrightIdeasSoftware.OLVColumn();
            this.olvColDecription = new BrightIdeasSoftware.OLVColumn();
            this.btnRefreshList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.objectListView1.AllColumns.Add(this.olvColMediaName);
            this.objectListView1.AllColumns.Add(this.olvColDecription);
            this.objectListView1.AlternateRowBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColMediaName,
            this.olvColDecription});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.EmptyListMsg = "服务器未发布任何媒体。";
            this.objectListView1.EmptyListMsgFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.HotTracking = true;
            this.objectListView1.HoverSelection = true;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.MultiSelect = false;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowItemToolTips = true;
            this.objectListView1.Size = new System.Drawing.Size(478, 161);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColMediaName
            // 
            this.olvColMediaName.AspectName = "Url";
            this.olvColMediaName.HeaderFont = null;
            this.olvColMediaName.Text = "名称";
            this.olvColMediaName.Width = 200;
            // 
            // olvColDecription
            // 
            this.olvColDecription.AspectName = "Description";
            this.olvColDecription.FillsFreeSpace = true;
            this.olvColDecription.HeaderFont = null;
            this.olvColDecription.IsTileViewColumn = true;
            this.olvColDecription.Text = "说明";
            this.olvColDecription.Width = 220;
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshList.Location = new System.Drawing.Point(398, 169);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshList.TabIndex = 1;
            this.btnRefreshList.Text = "刷新";
            this.btnRefreshList.UseVisualStyleBackColor = true;
            this.btnRefreshList.Click += new System.EventHandler(this.btnRefreshList_Click);
            // 
            // frmPlayList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(479, 195);
            this.Controls.Add(this.btnRefreshList);
            this.Controls.Add(this.objectListView1);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmPlayList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "媒体列表";
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColMediaName;
        private BrightIdeasSoftware.OLVColumn olvColDecription;
        private System.Windows.Forms.Button btnRefreshList;

    }
}