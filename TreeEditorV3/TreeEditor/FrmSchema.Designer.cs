namespace DF.WinUI.TreeEditor
{
    partial class FrmSchema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSchema));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxConnStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTreeTableName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxIdFieldName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLogicIDMap = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxNodeName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxNodeWhere = new System.Windows.Forms.TextBox();
            this.textBoxRootFilter = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接字符串";
            // 
            // textBoxConnStr
            // 
            this.textBoxConnStr.Location = new System.Drawing.Point(83, 5);
            this.textBoxConnStr.Multiline = true;
            this.textBoxConnStr.Name = "textBoxConnStr";
            this.textBoxConnStr.Size = new System.Drawing.Size(203, 51);
            this.textBoxConnStr.TabIndex = 1;
            this.textBoxConnStr.Text = "Data Source=.;Initial Catalog=eqt;UID=sa;PWD=admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据库类型";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(83, 60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(197, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "树表名：";
            // 
            // textBoxTreeTableName
            // 
            this.textBoxTreeTableName.Location = new System.Drawing.Point(83, 87);
            this.textBoxTreeTableName.Name = "textBoxTreeTableName";
            this.textBoxTreeTableName.Size = new System.Drawing.Size(197, 21);
            this.textBoxTreeTableName.TabIndex = 5;
            this.textBoxTreeTableName.Text = "TFUNCTION";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "ID列：";
            // 
            // textBoxIdFieldName
            // 
            this.textBoxIdFieldName.Location = new System.Drawing.Point(83, 118);
            this.textBoxIdFieldName.Name = "textBoxIdFieldName";
            this.textBoxIdFieldName.Size = new System.Drawing.Size(197, 21);
            this.textBoxIdFieldName.TabIndex = 5;
            this.textBoxIdFieldName.Text = "FUNC_ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "PID列：";
            // 
            // textBoxPID
            // 
            this.textBoxPID.Location = new System.Drawing.Point(83, 149);
            this.textBoxPID.Name = "textBoxPID";
            this.textBoxPID.Size = new System.Drawing.Size(197, 21);
            this.textBoxPID.TabIndex = 5;
            this.textBoxPID.Text = "PARENT_FUNC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "逻辑ID列：";
            // 
            // textBoxLogicIDMap
            // 
            this.textBoxLogicIDMap.Location = new System.Drawing.Point(83, 176);
            this.textBoxLogicIDMap.Name = "textBoxLogicIDMap";
            this.textBoxLogicIDMap.Size = new System.Drawing.Size(197, 21);
            this.textBoxLogicIDMap.TabIndex = 5;
            this.textBoxLogicIDMap.Text = "Order_Num";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "名称列：";
            // 
            // textBoxNodeName
            // 
            this.textBoxNodeName.Location = new System.Drawing.Point(83, 201);
            this.textBoxNodeName.Name = "textBoxNodeName";
            this.textBoxNodeName.Size = new System.Drawing.Size(197, 21);
            this.textBoxNodeName.TabIndex = 5;
            this.textBoxNodeName.Text = "FUNC_NAME";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(309, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Load Config...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(309, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Save Config";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxNodeWhere
            // 
            this.textBoxNodeWhere.Location = new System.Drawing.Point(83, 255);
            this.textBoxNodeWhere.Name = "textBoxNodeWhere";
            this.textBoxNodeWhere.Size = new System.Drawing.Size(197, 21);
            this.textBoxNodeWhere.TabIndex = 13;
            // 
            // textBoxRootFilter
            // 
            this.textBoxRootFilter.Location = new System.Drawing.Point(83, 228);
            this.textBoxRootFilter.Name = "textBoxRootFilter";
            this.textBoxRootFilter.Size = new System.Drawing.Size(197, 21);
            this.textBoxRootFilter.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(281, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 12;
            this.label11.Text = "空则加载全部表";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(281, 231);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "空则自动判断";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 259);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "数据库过滤：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 231);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "根表达式：";
            // 
            // FrmSchema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 283);
            this.Controls.Add(this.textBoxNodeWhere);
            this.Controls.Add(this.textBoxRootFilter);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxNodeName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxLogicIDMap);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxIdFieldName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTreeTableName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxConnStr);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSchema";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "层次结构--Schema";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxConnStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTreeTableName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxIdFieldName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLogicIDMap;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxNodeName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxNodeWhere;
        private System.Windows.Forms.TextBox textBoxRootFilter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
    }
}