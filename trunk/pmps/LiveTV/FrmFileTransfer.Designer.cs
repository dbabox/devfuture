namespace LiveTV
{
    partial class FrmFileTransfer
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
            this.BtnDownloadViaStream = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.BtnPutStream = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnDownloadViaStream
            // 
            this.BtnDownloadViaStream.Location = new System.Drawing.Point(328, 16);
            this.BtnDownloadViaStream.Name = "BtnDownloadViaStream";
            this.BtnDownloadViaStream.Size = new System.Drawing.Size(120, 24);
            this.BtnDownloadViaStream.TabIndex = 0;
            this.BtnDownloadViaStream.Text = "流方式下载";
            this.BtnDownloadViaStream.UseVisualStyleBackColor = true;
            this.BtnDownloadViaStream.Click += new System.EventHandler(this.BtnDownloadViaStream_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(16, 48);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(272, 21);
            this.textBoxFileName.TabIndex = 1;
            this.textBoxFileName.Text = "E:\\temp\\ddd.bak";
            // 
            // BtnPutStream
            // 
            this.BtnPutStream.Location = new System.Drawing.Point(336, 80);
            this.BtnPutStream.Name = "BtnPutStream";
            this.BtnPutStream.Size = new System.Drawing.Size(112, 23);
            this.BtnPutStream.TabIndex = 2;
            this.BtnPutStream.Text = "流式上传";
            this.BtnPutStream.UseVisualStyleBackColor = true;
            this.BtnPutStream.Click += new System.EventHandler(this.BtnPutStream_Click);
            // 
            // FrmFileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 267);
            this.Controls.Add(this.BtnPutStream);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.BtnDownloadViaStream);
            this.Name = "FrmFileTransfer";
            this.Text = "FrmFileTransfer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnDownloadViaStream;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button BtnPutStream;
    }
}