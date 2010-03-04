namespace DF.WinUI.TreeEditor
{
    partial class FrmTreeEditor
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
            System.Diagnostics.Trace.Listeners.Remove(_textBoxListener);
            _textBoxListener.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTreeEditor));
            this.tva = new Aga.Controls.Tree.TreeViewAdv();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnSaveTree2Db = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnDt2Tree = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.checkBoxAutoMap = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tva
            // 
            this.tva.AllowDrop = true;
            this.tva.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tva.BackColor = System.Drawing.SystemColors.Window;
            this.tva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tva.DefaultToolTipProvider = null;
            this.tva.DragDropMarkColor = System.Drawing.Color.Black;
            this.tva.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tva.Location = new System.Drawing.Point(1, 1);
            this.tva.Model = null;
            this.tva.Name = "tva";
            this.tva.SelectedNode = null;
            this.tva.Size = new System.Drawing.Size(270, 345);
            this.tva.TabIndex = 1;
            this.tva.Text = "内容树";
            this.tva.SelectionChanged += new System.EventHandler(this.tva_SelectionChanged);
            this.tva.DragDrop += new System.Windows.Forms.DragEventHandler(this.tva_DragDrop);
            this.tva.DragEnter += new System.Windows.Forms.DragEventHandler(this.tva_DragEnter);
            this.tva.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tva_ItemDrag);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDel.Location = new System.Drawing.Point(1, 352);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(103, 23);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "删除选定节点";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnSaveTree2Db
            // 
            this.btnSaveTree2Db.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveTree2Db.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSaveTree2Db.Location = new System.Drawing.Point(110, 352);
            this.btnSaveTree2Db.Name = "btnSaveTree2Db";
            this.btnSaveTree2Db.Size = new System.Drawing.Size(161, 23);
            this.btnSaveTree2Db.TabIndex = 10;
            this.btnSaveTree2Db.Text = "保存当前树结构到数据库";
            this.btnSaveTree2Db.UseVisualStyleBackColor = true;
            this.btnSaveTree2Db.Click += new System.EventHandler(this.btn_Click);
            // 
            // dgv
            // 
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv.Location = new System.Drawing.Point(277, 16);
            this.dgv.Name = "dgv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(411, 330);
            this.dgv.TabIndex = 11;
            this.dgv.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_DataError);
            // 
            // btnDt2Tree
            // 
            this.btnDt2Tree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDt2Tree.Location = new System.Drawing.Point(493, 352);
            this.btnDt2Tree.Name = "btnDt2Tree";
            this.btnDt2Tree.Size = new System.Drawing.Size(105, 23);
            this.btnDt2Tree.TabIndex = 12;
            this.btnDt2Tree.Text = "表格同步到树";
            this.btnDt2Tree.UseVisualStyleBackColor = true;
            this.btnDt2Tree.Click += new System.EventHandler(this.btnDt2Tree_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(277, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "ID，父ID，文本列必须填写，DB设定不可为空的列必须填写。";
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(604, 352);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(71, 23);
            this.btnReload.TabIndex = 14;
            this.btnReload.Text = "重新加载";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(4, 390);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxLog.Size = new System.Drawing.Size(681, 103);
            this.textBoxLog.TabIndex = 15;
            this.textBoxLog.WordWrap = false;
            // 
            // checkBoxAutoMap
            // 
            this.checkBoxAutoMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAutoMap.AutoSize = true;
            this.checkBoxAutoMap.Checked = true;
            this.checkBoxAutoMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoMap.Location = new System.Drawing.Point(279, 359);
            this.checkBoxAutoMap.Name = "checkBoxAutoMap";
            this.checkBoxAutoMap.Size = new System.Drawing.Size(150, 16);
            this.checkBoxAutoMap.TabIndex = 16;
            this.checkBoxAutoMap.Text = "逻辑层次ID映射到主键.";
            this.checkBoxAutoMap.UseVisualStyleBackColor = true;
            this.checkBoxAutoMap.CheckedChanged += new System.EventHandler(this.checkBoxAutoMap_CheckedChanged);
            // 
            // FrmTreeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 495);
            this.Controls.Add(this.checkBoxAutoMap);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDt2Tree);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnSaveTree2Db);
            this.Controls.Add(this.tva);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTreeEditor";
            this.Text = "树编辑器";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv tva;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSaveTree2Db;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnDt2Tree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.CheckBox checkBoxAutoMap;
    }
}