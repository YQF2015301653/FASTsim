
namespace FASTsim.GUI
{
    partial class Level9
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.level9Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.level9CheckBox = new System.Windows.Forms.CheckBox();
            this.level9Descr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.level9Value = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.level9Format = new System.Windows.Forms.ComboBox();
            this.level9NLB = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // level9Name
            // 
            this.level9Name.Location = new System.Drawing.Point(65, 44);
            this.level9Name.Name = "level9Name";
            this.level9Name.Size = new System.Drawing.Size(131, 21);
            this.level9Name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // level9CheckBox
            // 
            this.level9CheckBox.AutoSize = true;
            this.level9CheckBox.Location = new System.Drawing.Point(246, 49);
            this.level9CheckBox.Name = "level9CheckBox";
            this.level9CheckBox.Size = new System.Drawing.Size(108, 16);
            this.level9CheckBox.TabIndex = 2;
            this.level9CheckBox.Text = "Value is array";
            this.level9CheckBox.UseVisualStyleBackColor = true;
            // 
            // level9Descr
            // 
            this.level9Descr.Location = new System.Drawing.Point(65, 93);
            this.level9Descr.Name = "level9Descr";
            this.level9Descr.Size = new System.Drawing.Size(329, 21);
            this.level9Descr.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Descr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Format";
            // 
            // level9Value
            // 
            this.level9Value.Location = new System.Drawing.Point(263, 150);
            this.level9Value.Name = "level9Value";
            this.level9Value.Size = new System.Drawing.Size(131, 21);
            this.level9Value.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "NLB";
            // 
            // level9Format
            // 
            this.level9Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level9Format.FormattingEnabled = true;
            this.level9Format.Location = new System.Drawing.Point(65, 151);
            this.level9Format.Name = "level9Format";
            this.level9Format.Size = new System.Drawing.Size(63, 20);
            this.level9Format.TabIndex = 3;
            // 
            // level9NLB
            // 
            this.level9NLB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level9NLB.FormattingEnabled = true;
            this.level9NLB.Location = new System.Drawing.Point(153, 151);
            this.level9NLB.Name = "level9NLB";
            this.level9NLB.Size = new System.Drawing.Size(63, 20);
            this.level9NLB.TabIndex = 3;
            // 
            // Level9
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(406, 216);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Level9";
            this.Text = "Library Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox level9Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox level9CheckBox;
        private System.Windows.Forms.TextBox level9Descr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox level9Value;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox level9Format;
        private System.Windows.Forms.ComboBox level9NLB;
    }
}