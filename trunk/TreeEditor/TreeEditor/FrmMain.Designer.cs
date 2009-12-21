namespace TreeEditor
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.dgvTarget = new System.Windows.Forms.DataGridView();
            this.BtnDtToDb = new System.Windows.Forms.Button();
            this._tree = new Aga.Controls.Tree.TreeViewAdv();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiUP = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.delNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delCheckedNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.unCheckAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.BtnDt2Tree = new System.Windows.Forms.Button();
            this.checkBoxForce = new System.Windows.Forms.CheckBox();
            this.BtnDb2Tree = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTarget
            // 
            this.dgvTarget.AllowUserToDeleteRows = false;
            this.dgvTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTarget.Location = new System.Drawing.Point(3, 3);
            this.dgvTarget.Name = "dgvTarget";
            this.dgvTarget.RowTemplate.Height = 23;
            this.dgvTarget.Size = new System.Drawing.Size(368, 340);
            this.dgvTarget.TabIndex = 0;
            // 
            // BtnDtToDb
            // 
            this.BtnDtToDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDtToDb.Location = new System.Drawing.Point(377, 320);
            this.BtnDtToDb.Name = "BtnDtToDb";
            this.BtnDtToDb.Size = new System.Drawing.Size(103, 23);
            this.BtnDtToDb.TabIndex = 1;
            this.BtnDtToDb.Text = "将树写回到DB";
            this.BtnDtToDb.UseVisualStyleBackColor = true;
            this.BtnDtToDb.Click += new System.EventHandler(this.BtnSyncToDb_Click);
            // 
            // _tree
            // 
            this._tree.AllowDrop = true;
            this._tree.AutoRowHeight = true;
            this._tree.BackColor = System.Drawing.SystemColors.Window;
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tree.ContextMenuStrip = this.contextMenuStrip1;
            this._tree.DefaultToolTipProvider = null;
            this._tree.DisplayDraggingNodes = true;
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.DragDropMarkColor = System.Drawing.Color.Maroon;
            this._tree.LineColor = System.Drawing.SystemColors.ControlDark;
            this._tree.LoadOnDemand = true;
            this._tree.Location = new System.Drawing.Point(0, 0);
            this._tree.Model = null;
            this._tree.Name = "_tree";
            this._tree.SelectedNode = null;
            this._tree.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.MultiSameParent;
            this._tree.Size = new System.Drawing.Size(221, 346);
            this._tree.TabIndex = 6;
            this._tree.Text = "树";
            this._tree.DragDrop += new System.Windows.Forms.DragEventHandler(this._tree_DragDrop);
            this._tree.DragEnter += new System.Windows.Forms.DragEventHandler(this._tree_DragEnter);
            this._tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._tree_ItemDrag);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUP,
            this.tsmiDown,
            this.toolStripSeparator2,
            this.delNodeToolStripMenuItem,
            this.delCheckedNodesToolStripMenuItem,
            this.toolStripSeparator1,
            this.unCheckAllNodesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(179, 126);
            // 
            // tsmiUP
            // 
            this.tsmiUP.Name = "tsmiUP";
            this.tsmiUP.Size = new System.Drawing.Size(178, 22);
            this.tsmiUP.Text = "上移(&Up)";
            this.tsmiUP.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // tsmiDown
            // 
            this.tsmiDown.Name = "tsmiDown";
            this.tsmiDown.Size = new System.Drawing.Size(178, 22);
            this.tsmiDown.Text = "下移(&Down)";
            this.tsmiDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // delNodeToolStripMenuItem
            // 
            this.delNodeToolStripMenuItem.Name = "delNodeToolStripMenuItem";
            this.delNodeToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.delNodeToolStripMenuItem.Text = "删除当前节点";
            this.delNodeToolStripMenuItem.Click += new System.EventHandler(this.delNodeToolStripMenuItem_Click);
            // 
            // delCheckedNodesToolStripMenuItem
            // 
            this.delCheckedNodesToolStripMenuItem.Name = "delCheckedNodesToolStripMenuItem";
            this.delCheckedNodesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.delCheckedNodesToolStripMenuItem.Text = "删除所有已选定节点";
            this.delCheckedNodesToolStripMenuItem.Click += new System.EventHandler(this.delCheckedNodesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // unCheckAllNodesToolStripMenuItem
            // 
            this.unCheckAllNodesToolStripMenuItem.Name = "unCheckAllNodesToolStripMenuItem";
            this.unCheckAllNodesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.unCheckAllNodesToolStripMenuItem.Text = "重置所有已选定节点";
            this.unCheckAllNodesToolStripMenuItem.Click += new System.EventHandler(this.unCheckAllNodesToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.BtnDt2Tree);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxForce);
            this.splitContainer1.Panel2.Controls.Add(this.BtnDb2Tree);
            this.splitContainer1.Panel2.Controls.Add(this.dgvTarget);
            this.splitContainer1.Panel2.Controls.Add(this.BtnDtToDb);
            this.splitContainer1.Size = new System.Drawing.Size(708, 346);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.TabIndex = 7;
            // 
            // BtnDt2Tree
            // 
            this.BtnDt2Tree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDt2Tree.Location = new System.Drawing.Point(377, 32);
            this.BtnDt2Tree.Name = "BtnDt2Tree";
            this.BtnDt2Tree.Size = new System.Drawing.Size(103, 23);
            this.BtnDt2Tree.TabIndex = 5;
            this.BtnDt2Tree.Text = "表格同步到树";
            this.BtnDt2Tree.UseVisualStyleBackColor = true;
            this.BtnDt2Tree.Click += new System.EventHandler(this.BtnDt2Tree_Click);
            // 
            // checkBoxForce
            // 
            this.checkBoxForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxForce.AutoSize = true;
            this.checkBoxForce.ForeColor = System.Drawing.Color.Red;
            this.checkBoxForce.Location = new System.Drawing.Point(377, 265);
            this.checkBoxForce.Name = "checkBoxForce";
            this.checkBoxForce.Size = new System.Drawing.Size(72, 16);
            this.checkBoxForce.TabIndex = 4;
            this.checkBoxForce.Text = "强制同步";
            this.checkBoxForce.UseVisualStyleBackColor = true;
            this.checkBoxForce.CheckedChanged += new System.EventHandler(this.checkBoxForce_CheckedChanged);
            // 
            // BtnDb2Tree
            // 
            this.BtnDb2Tree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDb2Tree.Location = new System.Drawing.Point(377, 3);
            this.BtnDb2Tree.Name = "BtnDb2Tree";
            this.BtnDb2Tree.Size = new System.Drawing.Size(103, 23);
            this.BtnDb2Tree.TabIndex = 2;
            this.BtnDb2Tree.Text = "从DB加载树";
            this.BtnDb2Tree.UseVisualStyleBackColor = true;
            this.BtnDb2Tree.Click += new System.EventHandler(this.btnSync2Tree_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(3, 349);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(702, 111);
            this.textBoxLog.TabIndex = 8;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 463);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "树编辑器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTarget;
        private System.Windows.Forms.Button BtnDtToDb;
        private Aga.Controls.Tree.TreeViewAdv _tree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem delNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delCheckedNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unCheckAllNodesToolStripMenuItem;
        private System.Windows.Forms.Button BtnDb2Tree;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox checkBoxForce;
        private System.Windows.Forms.Button BtnDt2Tree;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiUP;
        private System.Windows.Forms.ToolStripMenuItem tsmiDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

