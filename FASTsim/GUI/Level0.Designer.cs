
namespace FASTsim.GUI
{
    partial class Level0
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
            this.level0Descr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Descr";
            // 
            // level0Descr
            // 
            this.level0Descr.Location = new System.Drawing.Point(48, 39);
            this.level0Descr.Name = "level0Descr";
            this.level0Descr.Size = new System.Drawing.Size(329, 21);
            this.level0Descr.TabIndex = 15;
            this.level0Descr.TextChanged += new System.EventHandler(this.level0Descr_TextChanged);
            // 
            // Level0
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(406, 216);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.level0Descr);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Level0";
            this.Text = "Library Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox level0Descr;
    }
}