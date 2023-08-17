
namespace FASTsim.GUI
{
    partial class Level1
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.level1Descr = new System.Windows.Forms.TextBox();
            this.level1Name = new System.Windows.Forms.TextBox();
            this.level1ReplyExpected = new System.Windows.Forms.CheckBox();
            this.level1AutoSystemBytes = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "Descr";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "Name";
            // 
            // level1Descr
            // 
            this.level1Descr.Location = new System.Drawing.Point(53, 86);
            this.level1Descr.Name = "level1Descr";
            this.level1Descr.Size = new System.Drawing.Size(329, 21);
            this.level1Descr.TabIndex = 12;
            // 
            // level1Name
            // 
            this.level1Name.Location = new System.Drawing.Point(53, 37);
            this.level1Name.Name = "level1Name";
            this.level1Name.Size = new System.Drawing.Size(131, 21);
            this.level1Name.TabIndex = 13;
            // 
            // level1ReplyExpected
            // 
            this.level1ReplyExpected.AutoSize = true;
            this.level1ReplyExpected.Location = new System.Drawing.Point(53, 155);
            this.level1ReplyExpected.Name = "level1ReplyExpected";
            this.level1ReplyExpected.Size = new System.Drawing.Size(102, 16);
            this.level1ReplyExpected.TabIndex = 16;
            this.level1ReplyExpected.Text = "ReplyExpected";
            this.level1ReplyExpected.UseVisualStyleBackColor = true;
            // 
            // level1AutoSystemBytes
            // 
            this.level1AutoSystemBytes.AutoSize = true;
            this.level1AutoSystemBytes.Location = new System.Drawing.Point(230, 155);
            this.level1AutoSystemBytes.Name = "level1AutoSystemBytes";
            this.level1AutoSystemBytes.Size = new System.Drawing.Size(114, 16);
            this.level1AutoSystemBytes.TabIndex = 16;
            this.level1AutoSystemBytes.Text = "AutoSystemBytes";
            this.level1AutoSystemBytes.UseVisualStyleBackColor = true;
            // 
            // Level1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(406, 216);
            this.Controls.Add(this.level1AutoSystemBytes);
            this.Controls.Add(this.level1ReplyExpected);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.level1Descr);
            this.Controls.Add(this.level1Name);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Level1";
            this.Text = "Library Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox level1Descr;
        private System.Windows.Forms.TextBox level1Name;
        private System.Windows.Forms.CheckBox level1ReplyExpected;
        private System.Windows.Forms.CheckBox level1AutoSystemBytes;
    }
}