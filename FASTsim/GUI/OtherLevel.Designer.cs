
namespace FASTsim.GUI
{
    partial class OtherLevel
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
            this.level9NLB = new System.Windows.Forms.ComboBox();
            this.level9Format = new System.Windows.Forms.ComboBox();
            this.level9CheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.level9Descr = new System.Windows.Forms.TextBox();
            this.level9Value = new System.Windows.Forms.TextBox();
            this.level9Name = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // level9NLB
            // 
            this.level9NLB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level9NLB.FormattingEnabled = true;
            this.level9NLB.Items.AddRange(new object[] {
            "Auto",
            "1",
            "2",
            "3"});
            this.level9NLB.Location = new System.Drawing.Point(165, 123);
            this.level9NLB.Name = "level9NLB";
            this.level9NLB.Size = new System.Drawing.Size(63, 20);
            this.level9NLB.TabIndex = 13;
            // 
            // level9Format
            // 
            this.level9Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level9Format.FormattingEnabled = true;
            this.level9Format.Items.AddRange(new object[] {
            "UNDEFINED",
            "BINARY",
            "BOOLEAN",
            "ASCII",
            "JIS8",
            "I8",
            "I1",
            "I2",
            "I4",
            "F8",
            "F4",
            "U8",
            "U1",
            "U2",
            "U4"});
            this.level9Format.Location = new System.Drawing.Point(58, 121);
            this.level9Format.Name = "level9Format";
            this.level9Format.Size = new System.Drawing.Size(63, 20);
            this.level9Format.TabIndex = 14;
            this.level9Format.SelectedIndexChanged += new System.EventHandler(this.Level9Format_SelectedIndexChanged);
            // 
            // level9CheckBox
            // 
            this.level9CheckBox.AutoSize = true;
            this.level9CheckBox.Location = new System.Drawing.Point(258, 16);
            this.level9CheckBox.Name = "level9CheckBox";
            this.level9CheckBox.Size = new System.Drawing.Size(108, 16);
            this.level9CheckBox.TabIndex = 12;
            this.level9CheckBox.Text = "Value is array";
            this.level9CheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Descr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Format";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "NLB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name";
            // 
            // level9Descr
            // 
            this.level9Descr.Location = new System.Drawing.Point(58, 66);
            this.level9Descr.Name = "level9Descr";
            this.level9Descr.Size = new System.Drawing.Size(329, 21);
            this.level9Descr.TabIndex = 4;
            // 
            // level9Value
            // 
            this.level9Value.Location = new System.Drawing.Point(275, 117);
            this.level9Value.Name = "level9Value";
            this.level9Value.Size = new System.Drawing.Size(131, 21);
            this.level9Value.TabIndex = 5;
            // 
            // level9Name
            // 
            this.level9Name.Location = new System.Drawing.Point(58, 14);
            this.level9Name.Name = "level9Name";
            this.level9Name.Size = new System.Drawing.Size(131, 21);
            this.level9Name.TabIndex = 6;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(330, 156);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 15;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OtherLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.level9NLB);
            this.Controls.Add(this.level9Format);
            this.Controls.Add(this.level9CheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.level9Descr);
            this.Controls.Add(this.level9Value);
            this.Controls.Add(this.level9Name);
            this.Name = "OtherLevel";
            this.Size = new System.Drawing.Size(422, 255);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox level9NLB;
        private System.Windows.Forms.ComboBox level9Format;
        private System.Windows.Forms.CheckBox level9CheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox level9Descr;
        private System.Windows.Forms.TextBox level9Value;
        private System.Windows.Forms.TextBox level9Name;
        private System.Windows.Forms.Button saveButton;
    }
}
