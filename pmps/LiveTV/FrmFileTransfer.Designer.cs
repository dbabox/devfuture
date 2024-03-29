﻿namespace LiveTV
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
            this.BtnSendByChunk = new System.Windows.Forms.Button();
            this.BtnSend2Chunk = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BtnVerify = new System.Windows.Forms.Button();
            this.BtnClearWsCache = new System.Windows.Forms.Button();
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
            this.textBoxFileName.Text = "E:\\temp\\zb20100210.bak";
            // 
            // BtnPutStream
            // 
            this.BtnPutStream.Location = new System.Drawing.Point(328, 48);
            this.BtnPutStream.Name = "BtnPutStream";
            this.BtnPutStream.Size = new System.Drawing.Size(112, 23);
            this.BtnPutStream.TabIndex = 2;
            this.BtnPutStream.Text = "流式上传";
            this.BtnPutStream.UseVisualStyleBackColor = true;
            this.BtnPutStream.Click += new System.EventHandler(this.BtnPutStream_Click);
            // 
            // BtnSendByChunk
            // 
            this.BtnSendByChunk.Location = new System.Drawing.Point(328, 80);
            this.BtnSendByChunk.Name = "BtnSendByChunk";
            this.BtnSendByChunk.Size = new System.Drawing.Size(104, 24);
            this.BtnSendByChunk.TabIndex = 3;
            this.BtnSendByChunk.Text = "分块上传";
            this.BtnSendByChunk.UseVisualStyleBackColor = true;
            this.BtnSendByChunk.Click += new System.EventHandler(this.BtnSendByChunk_Click);
            // 
            // BtnSend2Chunk
            // 
            this.BtnSend2Chunk.Location = new System.Drawing.Point(336, 120);
            this.BtnSend2Chunk.Name = "BtnSend2Chunk";
            this.BtnSend2Chunk.Size = new System.Drawing.Size(75, 23);
            this.BtnSend2Chunk.TabIndex = 4;
            this.BtnSend2Chunk.Text = "分2次上传";
            this.BtnSend2Chunk.UseVisualStyleBackColor = true;
            this.BtnSend2Chunk.Click += new System.EventHandler(this.BtnSend2Chunk_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 104);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(272, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "zb20100210.bak";
            // 
            // BtnVerify
            // 
            this.BtnVerify.Location = new System.Drawing.Point(200, 136);
            this.BtnVerify.Name = "BtnVerify";
            this.BtnVerify.Size = new System.Drawing.Size(88, 24);
            this.BtnVerify.TabIndex = 5;
            this.BtnVerify.Text = "Verify";
            this.BtnVerify.UseVisualStyleBackColor = true;
            this.BtnVerify.Click += new System.EventHandler(this.BtnVerify_Click);
            // 
            // BtnClearWsCache
            // 
            this.BtnClearWsCache.Location = new System.Drawing.Point(328, 176);
            this.BtnClearWsCache.Name = "BtnClearWsCache";
            this.BtnClearWsCache.Size = new System.Drawing.Size(120, 23);
            this.BtnClearWsCache.TabIndex = 6;
            this.BtnClearWsCache.Text = "Clear WS Cache";
            this.BtnClearWsCache.UseVisualStyleBackColor = true;
            this.BtnClearWsCache.Click += new System.EventHandler(this.BtnClearWsCache_Click);
            // 
            // FrmFileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 267);
            this.Controls.Add(this.BtnClearWsCache);
            this.Controls.Add(this.BtnVerify);
            this.Controls.Add(this.BtnSend2Chunk);
            this.Controls.Add(this.BtnSendByChunk);
            this.Controls.Add(this.BtnPutStream);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.Button BtnSendByChunk;
        private System.Windows.Forms.Button BtnSend2Chunk;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BtnVerify;
        private System.Windows.Forms.Button BtnClearWsCache;
    }
}