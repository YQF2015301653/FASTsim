
namespace FASTsim.GUI
{
    partial class HSMSProperties
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
            this.pnlLeft = new System.Windows.Forms.Label();
            this.labelEx4 = new System.Windows.Forms.Label();
            this.labelEx5 = new System.Windows.Forms.Label();
            this.labelEx7 = new System.Windows.Forms.Label();
            this.numEstab = new System.Windows.Forms.NumericUpDown();
            this.numIP1 = new System.Windows.Forms.NumericUpDown();
            this.numTCPPort = new System.Windows.Forms.NumericUpDown();
            this.numIP2 = new System.Windows.Forms.NumericUpDown();
            this.numIP3 = new System.Windows.Forms.NumericUpDown();
            this.numIP4 = new System.Windows.Forms.NumericUpDown();
            this.labelEx3 = new System.Windows.Forms.Label();
            this.rbtActive = new System.Windows.Forms.RadioButton();
            this.rbtPassive = new System.Windows.Forms.RadioButton();
            this.labelEx6 = new System.Windows.Forms.Label();
            this.numT3 = new System.Windows.Forms.NumericUpDown();
            this.labelEx13 = new System.Windows.Forms.Label();
            this.labelEx8 = new System.Windows.Forms.Label();
            this.numCircuit = new System.Windows.Forms.NumericUpDown();
            this.numT5 = new System.Windows.Forms.NumericUpDown();
            this.labelEx14 = new System.Windows.Forms.Label();
            this.labelEx9 = new System.Windows.Forms.Label();
            this.labelEx16 = new System.Windows.Forms.Label();
            this.numTGRACE = new System.Windows.Forms.NumericUpDown();
            this.labelEx11 = new System.Windows.Forms.Label();
            this.numT6 = new System.Windows.Forms.NumericUpDown();
            this.numWrite = new System.Windows.Forms.NumericUpDown();
            this.numT8 = new System.Windows.Forms.NumericUpDown();
            this.labelEx15 = new System.Windows.Forms.Label();
            this.labelEx10 = new System.Windows.Forms.Label();
            this.numMemory = new System.Windows.Forms.NumericUpDown();
            this.numT7 = new System.Windows.Forms.NumericUpDown();
            this.labelEx12 = new System.Windows.Forms.Label();
            this.numDeviceID = new System.Windows.Forms.NumericUpDown();
            this.labelEx2 = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numEstab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTCPPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCircuit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTGRACE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWrite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceID)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.AutoSize = true;
            this.pnlLeft.Location = new System.Drawing.Point(13, 13);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(89, 12);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.Text = "SECS Protocol:";
            // 
            // labelEx4
            // 
            this.labelEx4.AutoSize = true;
            this.labelEx4.Location = new System.Drawing.Point(11, 63);
            this.labelEx4.Name = "labelEx4";
            this.labelEx4.Size = new System.Drawing.Size(149, 12);
            this.labelEx4.TabIndex = 1;
            this.labelEx4.Text = "Passive Entity IPaddress";
            // 
            // labelEx5
            // 
            this.labelEx5.AutoSize = true;
            this.labelEx5.Location = new System.Drawing.Point(11, 94);
            this.labelEx5.Name = "labelEx5";
            this.labelEx5.Size = new System.Drawing.Size(137, 12);
            this.labelEx5.TabIndex = 1;
            this.labelEx5.Text = "Passive Entity Tcpport";
            // 
            // labelEx7
            // 
            this.labelEx7.AutoSize = true;
            this.labelEx7.Location = new System.Drawing.Point(342, 142);
            this.labelEx7.Name = "labelEx7";
            this.labelEx7.Size = new System.Drawing.Size(149, 12);
            this.labelEx7.TabIndex = 1;
            this.labelEx7.Text = "Connection Establishment";
            // 
            // numEstab
            // 
            this.numEstab.Location = new System.Drawing.Point(497, 142);
            this.numEstab.Name = "numEstab";
            this.numEstab.Size = new System.Drawing.Size(54, 21);
            this.numEstab.TabIndex = 2;
            // 
            // numIP1
            // 
            this.numIP1.Location = new System.Drawing.Point(191, 61);
            this.numIP1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numIP1.Name = "numIP1";
            this.numIP1.Size = new System.Drawing.Size(54, 21);
            this.numIP1.TabIndex = 2;
            // 
            // numTCPPort
            // 
            this.numTCPPort.Location = new System.Drawing.Point(154, 94);
            this.numTCPPort.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numTCPPort.Name = "numTCPPort";
            this.numTCPPort.Size = new System.Drawing.Size(54, 21);
            this.numTCPPort.TabIndex = 2;
            // 
            // numIP2
            // 
            this.numIP2.Location = new System.Drawing.Point(251, 61);
            this.numIP2.Name = "numIP2";
            this.numIP2.Size = new System.Drawing.Size(54, 21);
            this.numIP2.TabIndex = 2;
            // 
            // numIP3
            // 
            this.numIP3.Location = new System.Drawing.Point(311, 61);
            this.numIP3.Name = "numIP3";
            this.numIP3.Size = new System.Drawing.Size(54, 21);
            this.numIP3.TabIndex = 2;
            // 
            // numIP4
            // 
            this.numIP4.Location = new System.Drawing.Point(373, 61);
            this.numIP4.Name = "numIP4";
            this.numIP4.Size = new System.Drawing.Size(54, 21);
            this.numIP4.TabIndex = 2;
            // 
            // labelEx3
            // 
            this.labelEx3.AutoSize = true;
            this.labelEx3.Location = new System.Drawing.Point(13, 38);
            this.labelEx3.Name = "labelEx3";
            this.labelEx3.Size = new System.Drawing.Size(95, 12);
            this.labelEx3.TabIndex = 0;
            this.labelEx3.Text = "Equipment Mold:";
            // 
            // rbtActive
            // 
            this.rbtActive.AutoSize = true;
            this.rbtActive.Location = new System.Drawing.Point(126, 34);
            this.rbtActive.Name = "rbtActive";
            this.rbtActive.Size = new System.Drawing.Size(59, 16);
            this.rbtActive.TabIndex = 3;
            this.rbtActive.Text = "Active";
            this.rbtActive.UseVisualStyleBackColor = true;
            // 
            // rbtPassive
            // 
            this.rbtPassive.AutoSize = true;
            this.rbtPassive.Checked = true;
            this.rbtPassive.Location = new System.Drawing.Point(193, 34);
            this.rbtPassive.Name = "rbtPassive";
            this.rbtPassive.Size = new System.Drawing.Size(65, 16);
            this.rbtPassive.TabIndex = 4;
            this.rbtPassive.TabStop = true;
            this.rbtPassive.Text = "Passive";
            this.rbtPassive.UseVisualStyleBackColor = true;
            // 
            // labelEx6
            // 
            this.labelEx6.AutoSize = true;
            this.labelEx6.Location = new System.Drawing.Point(13, 142);
            this.labelEx6.Name = "labelEx6";
            this.labelEx6.Size = new System.Drawing.Size(107, 12);
            this.labelEx6.TabIndex = 1;
            this.labelEx6.Text = "Reply Timeout(T3)";
            // 
            // numT3
            // 
            this.numT3.Location = new System.Drawing.Point(251, 140);
            this.numT3.Name = "numT3";
            this.numT3.Size = new System.Drawing.Size(54, 21);
            this.numT3.TabIndex = 2;
            // 
            // labelEx13
            // 
            this.labelEx13.AutoSize = true;
            this.labelEx13.Location = new System.Drawing.Point(340, 181);
            this.labelEx13.Name = "labelEx13";
            this.labelEx13.Size = new System.Drawing.Size(107, 12);
            this.labelEx13.TabIndex = 1;
            this.labelEx13.Text = "Circuit Assurance";
            // 
            // labelEx8
            // 
            this.labelEx8.AutoSize = true;
            this.labelEx8.Location = new System.Drawing.Point(11, 181);
            this.labelEx8.Name = "labelEx8";
            this.labelEx8.Size = new System.Drawing.Size(185, 12);
            this.labelEx8.TabIndex = 1;
            this.labelEx8.Text = "Connect Separation Timeout(T5)";
            // 
            // numCircuit
            // 
            this.numCircuit.Location = new System.Drawing.Point(497, 179);
            this.numCircuit.Name = "numCircuit";
            this.numCircuit.Size = new System.Drawing.Size(54, 21);
            this.numCircuit.TabIndex = 2;
            // 
            // numT5
            // 
            this.numT5.Location = new System.Drawing.Point(251, 179);
            this.numT5.Name = "numT5";
            this.numT5.Size = new System.Drawing.Size(54, 21);
            this.numT5.TabIndex = 2;
            // 
            // labelEx14
            // 
            this.labelEx14.AutoSize = true;
            this.labelEx14.Location = new System.Drawing.Point(340, 222);
            this.labelEx14.Name = "labelEx14";
            this.labelEx14.Size = new System.Drawing.Size(41, 12);
            this.labelEx14.TabIndex = 1;
            this.labelEx14.Text = "TGRACE";
            // 
            // labelEx9
            // 
            this.labelEx9.AutoSize = true;
            this.labelEx9.Location = new System.Drawing.Point(11, 222);
            this.labelEx9.Name = "labelEx9";
            this.labelEx9.Size = new System.Drawing.Size(185, 12);
            this.labelEx9.TabIndex = 1;
            this.labelEx9.Text = "Connect Separation Timeout(T6)";
            // 
            // labelEx16
            // 
            this.labelEx16.AutoSize = true;
            this.labelEx16.Location = new System.Drawing.Point(340, 288);
            this.labelEx16.Name = "labelEx16";
            this.labelEx16.Size = new System.Drawing.Size(71, 12);
            this.labelEx16.TabIndex = 1;
            this.labelEx16.Text = "Write Stall";
            // 
            // numTGRACE
            // 
            this.numTGRACE.Location = new System.Drawing.Point(497, 213);
            this.numTGRACE.Name = "numTGRACE";
            this.numTGRACE.Size = new System.Drawing.Size(54, 21);
            this.numTGRACE.TabIndex = 2;
            // 
            // labelEx11
            // 
            this.labelEx11.AutoSize = true;
            this.labelEx11.Location = new System.Drawing.Point(11, 288);
            this.labelEx11.Name = "labelEx11";
            this.labelEx11.Size = new System.Drawing.Size(167, 12);
            this.labelEx11.TabIndex = 1;
            this.labelEx11.Text = "Inter-Character Timeout(T8)";
            // 
            // numT6
            // 
            this.numT6.Location = new System.Drawing.Point(251, 222);
            this.numT6.Name = "numT6";
            this.numT6.Size = new System.Drawing.Size(54, 21);
            this.numT6.TabIndex = 2;
            // 
            // numWrite
            // 
            this.numWrite.Location = new System.Drawing.Point(497, 286);
            this.numWrite.Name = "numWrite";
            this.numWrite.Size = new System.Drawing.Size(54, 21);
            this.numWrite.TabIndex = 2;
            // 
            // numT8
            // 
            this.numT8.Location = new System.Drawing.Point(251, 286);
            this.numT8.Name = "numT8";
            this.numT8.Size = new System.Drawing.Size(54, 21);
            this.numT8.TabIndex = 2;
            // 
            // labelEx15
            // 
            this.labelEx15.AutoSize = true;
            this.labelEx15.Location = new System.Drawing.Point(342, 253);
            this.labelEx15.Name = "labelEx15";
            this.labelEx15.Size = new System.Drawing.Size(77, 12);
            this.labelEx15.TabIndex = 1;
            this.labelEx15.Text = "Memory Stall";
            // 
            // labelEx10
            // 
            this.labelEx10.AutoSize = true;
            this.labelEx10.Location = new System.Drawing.Point(13, 253);
            this.labelEx10.Name = "labelEx10";
            this.labelEx10.Size = new System.Drawing.Size(149, 12);
            this.labelEx10.TabIndex = 1;
            this.labelEx10.Text = "Not Selected Timeout(T7)";
            // 
            // numMemory
            // 
            this.numMemory.Location = new System.Drawing.Point(497, 244);
            this.numMemory.Name = "numMemory";
            this.numMemory.Size = new System.Drawing.Size(54, 21);
            this.numMemory.TabIndex = 2;
            // 
            // numT7
            // 
            this.numT7.Location = new System.Drawing.Point(251, 253);
            this.numT7.Name = "numT7";
            this.numT7.Size = new System.Drawing.Size(54, 21);
            this.numT7.TabIndex = 2;
            // 
            // labelEx12
            // 
            this.labelEx12.AutoSize = true;
            this.labelEx12.Location = new System.Drawing.Point(11, 332);
            this.labelEx12.Name = "labelEx12";
            this.labelEx12.Size = new System.Drawing.Size(107, 12);
            this.labelEx12.TabIndex = 1;
            this.labelEx12.Text = "DEVICEID(0-32767)";
            // 
            // numDeviceID
            // 
            this.numDeviceID.Location = new System.Drawing.Point(251, 327);
            this.numDeviceID.Name = "numDeviceID";
            this.numDeviceID.Size = new System.Drawing.Size(54, 21);
            this.numDeviceID.TabIndex = 2;
            // 
            // labelEx2
            // 
            this.labelEx2.AutoSize = true;
            this.labelEx2.Location = new System.Drawing.Point(105, 13);
            this.labelEx2.Name = "labelEx2";
            this.labelEx2.Size = new System.Drawing.Size(41, 12);
            this.labelEx2.TabIndex = 0;
            this.labelEx2.Text = "HSMS94";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(476, 332);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 5;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // HSMSProperties
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(582, 376);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.rbtPassive);
            this.Controls.Add(this.rbtActive);
            this.Controls.Add(this.numIP4);
            this.Controls.Add(this.numIP3);
            this.Controls.Add(this.numIP2);
            this.Controls.Add(this.numIP1);
            this.Controls.Add(this.numTCPPort);
            this.Controls.Add(this.numT7);
            this.Controls.Add(this.numDeviceID);
            this.Controls.Add(this.numT8);
            this.Controls.Add(this.numT5);
            this.Controls.Add(this.numMemory);
            this.Controls.Add(this.numWrite);
            this.Controls.Add(this.numCircuit);
            this.Controls.Add(this.numT6);
            this.Controls.Add(this.labelEx12);
            this.Controls.Add(this.labelEx10);
            this.Controls.Add(this.labelEx11);
            this.Controls.Add(this.numT3);
            this.Controls.Add(this.numTGRACE);
            this.Controls.Add(this.labelEx8);
            this.Controls.Add(this.labelEx15);
            this.Controls.Add(this.labelEx16);
            this.Controls.Add(this.numEstab);
            this.Controls.Add(this.labelEx9);
            this.Controls.Add(this.labelEx13);
            this.Controls.Add(this.labelEx14);
            this.Controls.Add(this.labelEx6);
            this.Controls.Add(this.labelEx7);
            this.Controls.Add(this.labelEx5);
            this.Controls.Add(this.labelEx4);
            this.Controls.Add(this.labelEx3);
            this.Controls.Add(this.labelEx2);
            this.Controls.Add(this.pnlLeft);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSMSProperties";
            this.Text = "HSMSProperties";
            this.Load += new System.EventHandler(this.HSMSProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numEstab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTCPPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCircuit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTGRACE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWrite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numT7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pnlLeft;
        private System.Windows.Forms.Label labelEx4;
        private System.Windows.Forms.Label labelEx5;
        private System.Windows.Forms.Label labelEx7;
        private System.Windows.Forms.NumericUpDown numEstab;
        private System.Windows.Forms.NumericUpDown numIP1;
        private System.Windows.Forms.NumericUpDown numTCPPort;
        private System.Windows.Forms.NumericUpDown numIP2;
        private System.Windows.Forms.NumericUpDown numIP3;
        private System.Windows.Forms.NumericUpDown numIP4;
        private System.Windows.Forms.Label labelEx3;
        private System.Windows.Forms.RadioButton rbtActive;
        private System.Windows.Forms.RadioButton rbtPassive;
        private System.Windows.Forms.Label labelEx6;
        private System.Windows.Forms.NumericUpDown numT3;
        private System.Windows.Forms.Label labelEx13;
        private System.Windows.Forms.Label labelEx8;
        private System.Windows.Forms.NumericUpDown numCircuit;
        private System.Windows.Forms.NumericUpDown numT5;
        private System.Windows.Forms.Label labelEx14;
        private System.Windows.Forms.Label labelEx9;
        private System.Windows.Forms.Label labelEx16;
        private System.Windows.Forms.NumericUpDown numTGRACE;
        private System.Windows.Forms.Label labelEx11;
        private System.Windows.Forms.NumericUpDown numT6;
        private System.Windows.Forms.NumericUpDown numWrite;
        private System.Windows.Forms.NumericUpDown numT8;
        private System.Windows.Forms.Label labelEx15;
        private System.Windows.Forms.Label labelEx10;
        private System.Windows.Forms.NumericUpDown numMemory;
        private System.Windows.Forms.NumericUpDown numT7;
        private System.Windows.Forms.Label labelEx12;
        private System.Windows.Forms.NumericUpDown numDeviceID;
        private System.Windows.Forms.Label labelEx2;
        private System.Windows.Forms.Button applyButton;
    }
}