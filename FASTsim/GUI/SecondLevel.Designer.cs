
namespace FASTsim.GUI
{
    partial class SecondLevel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.level2Descr = new System.Windows.Forms.TextBox();
            this.level2Stream = new System.Windows.Forms.TextBox();
            this.level2Fun = new System.Windows.Forms.TextBox();
            this.level2Name = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Descr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "Stream";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(127, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "Function";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Name";
            // 
            // level2Descr
            // 
            this.level2Descr.Location = new System.Drawing.Point(58, 77);
            this.level2Descr.Name = "level2Descr";
            this.level2Descr.Size = new System.Drawing.Size(329, 21);
            this.level2Descr.TabIndex = 12;
            // 
            // level2Stream
            // 
            this.level2Stream.Enabled = false;
            this.level2Stream.Location = new System.Drawing.Point(76, 135);
            this.level2Stream.Name = "level2Stream";
            this.level2Stream.Size = new System.Drawing.Size(45, 21);
            this.level2Stream.TabIndex = 13;
            // 
            // level2Fun
            // 
            this.level2Fun.Enabled = false;
            this.level2Fun.Location = new System.Drawing.Point(186, 135);
            this.level2Fun.Name = "level2Fun";
            this.level2Fun.Size = new System.Drawing.Size(45, 21);
            this.level2Fun.TabIndex = 14;
            // 
            // level2Name
            // 
            this.level2Name.Location = new System.Drawing.Point(58, 28);
            this.level2Name.Name = "level2Name";
            this.level2Name.Size = new System.Drawing.Size(131, 21);
            this.level2Name.TabIndex = 15;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(312, 133);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SecondLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.level2Descr);
            this.Controls.Add(this.level2Stream);
            this.Controls.Add(this.level2Fun);
            this.Controls.Add(this.level2Name);
            this.Name = "SecondLevel";
            this.Size = new System.Drawing.Size(422, 255);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox level2Descr;
        private System.Windows.Forms.TextBox level2Stream;
        private System.Windows.Forms.TextBox level2Fun;
        private System.Windows.Forms.TextBox level2Name;
        private System.Windows.Forms.Button saveButton;
    }
}
