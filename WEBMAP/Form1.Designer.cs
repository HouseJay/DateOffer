namespace WEBMAP
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tebExcelPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butAction = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tebExcelPath
            // 
            this.tebExcelPath.Location = new System.Drawing.Point(59, 12);
            this.tebExcelPath.Name = "tebExcelPath";
            this.tebExcelPath.Size = new System.Drawing.Size(438, 21);
            this.tebExcelPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "路径：";
            // 
            // butAction
            // 
            this.butAction.Location = new System.Drawing.Point(524, 10);
            this.butAction.Name = "butAction";
            this.butAction.Size = new System.Drawing.Size(149, 23);
            this.butAction.TabIndex = 2;
            this.butAction.Text = "开始";
            this.butAction.UseVisualStyleBackColor = true;
            this.butAction.Click += new System.EventHandler(this.butAction_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.butAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tebExcelPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tebExcelPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butAction;
    }
}

