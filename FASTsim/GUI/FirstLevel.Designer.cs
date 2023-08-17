
namespace FASTsim.GUI
{
    partial class FirstLevel
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
            this.level1AutoSystemBytes = new System.Windows.Forms.CheckBox();
            this.level1ReplyExpected = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.level1Descr = new System.Windows.Forms.TextBox();
            this.level1Name = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // level1AutoSystemBytes
            // 
            this.level1AutoSystemBytes.AutoSize = true;
            this.level1AutoSystemBytes.Location = new System.Drawing.Point(229, 138);
            this.level1AutoSystemBytes.Name = "level1AutoSystemBytes";
            this.level1AutoSystemBytes.Size = new System.Drawing.Size(114, 16);
            this.level1AutoSystemBytes.TabIndex = 21;
            this.level1AutoSystemBytes.Text = "AutoSystemBytes";
            this.level1AutoSystemBytes.UseVisualStyleBackColor = true;
            // 
            // level1ReplyExpected
            // 
            this.level1ReplyExpected.AutoSize = true;
            this.level1ReplyExpected.Location = new System.Drawing.Point(52, 138);
            this.level1ReplyExpected.Name = "level1ReplyExpected";
            this.level1ReplyExpected.Size = new System.Drawing.Size(102, 16);
            this.level1ReplyExpected.TabIndex = 22;
            this.level1ReplyExpected.Text = "ReplyExpected";
            this.level1ReplyExpected.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Descr";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "Name";
            // 
            // level1Descr
            // 
            this.level1Descr.Location = new System.Drawing.Point(52, 69);
            this.level1Descr.Name = "level1Descr";
            this.level1Descr.Size = new System.Drawing.Size(329, 21);
            this.level1Descr.TabIndex = 17;
            // 
            // level1Name
            // 
            this.level1Name.Location = new System.Drawing.Point(52, 20);
            this.level1Name.Name = "level1Name";
            this.level1Name.Size = new System.Drawing.Size(131, 21);
            this.level1Name.TabIndex = 18;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(305, 176);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 23;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // FirstLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.level1AutoSystemBytes);
            this.Controls.Add(this.level1ReplyExpected);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.level1Descr);
            this.Controls.Add(this.level1Name);
            this.Name = "FirstLevel";
            this.Size = new System.Drawing.Size(422, 255);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox level1AutoSystemBytes;
        private System.Windows.Forms.CheckBox level1ReplyExpected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox level1Descr;
        private System.Windows.Forms.TextBox level1Name;
        private System.Windows.Forms.Button saveButton;
    }
}
