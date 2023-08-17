
namespace FASTsim.GUI
{
    partial class Level2
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
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.level2Descr = new System.Windows.Forms.TextBox();
            this.level2Fun = new System.Windows.Forms.TextBox();
            this.level2Name = new System.Windows.Forms.TextBox();
            this.level2Stream = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Descr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Stream";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(128, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Function";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name";
            // 
            // level2Descr
            // 
            this.level2Descr.Location = new System.Drawing.Point(59, 94);
            this.level2Descr.Name = "level2Descr";
            this.level2Descr.Size = new System.Drawing.Size(329, 21);
            this.level2Descr.TabIndex = 4;
            // 
            // level2Fun
            // 
            this.level2Fun.Location = new System.Drawing.Point(187, 152);
            this.level2Fun.Name = "level2Fun";
            this.level2Fun.Size = new System.Drawing.Size(45, 21);
            this.level2Fun.TabIndex = 5;
            // 
            // level2Name
            // 
            this.level2Name.Location = new System.Drawing.Point(59, 45);
            this.level2Name.Name = "level2Name";
            this.level2Name.Size = new System.Drawing.Size(131, 21);
            this.level2Name.TabIndex = 6;
            // 
            // level2Stream
            // 
            this.level2Stream.Location = new System.Drawing.Point(77, 152);
            this.level2Stream.Name = "level2Stream";
            this.level2Stream.Size = new System.Drawing.Size(45, 21);
            this.level2Stream.TabIndex = 5;
            // 
            // Level2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(406, 216);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.level2Descr);
            this.Controls.Add(this.level2Stream);
            this.Controls.Add(this.level2Fun);
            this.Controls.Add(this.level2Name);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Level2";
            this.Text = "Library Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox level2Descr;
        private System.Windows.Forms.TextBox level2Fun;
        private System.Windows.Forms.TextBox level2Name;
        private System.Windows.Forms.TextBox level2Stream;
    }
}