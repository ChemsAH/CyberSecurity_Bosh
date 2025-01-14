namespace FDITool
{
    partial class ViewSettings
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
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxDBPasswort = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDBServer = new System.Windows.Forms.TextBox();
            this.buttonDurchsuchenModell = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxSeperatorTStamp = new System.Windows.Forms.ComboBox();
            this.comboBoxSeperatorIO = new System.Windows.Forms.ComboBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxNameDatenbank = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDBUser = new System.Windows.Forms.TextBox();
            this.textBoxModellDatei = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openXMLFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUpdateInterval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAnlage = new System.Windows.Forms.Label();
            this.txtAnlage = new System.Windows.Forms.TextBox();
            this.chkAutomatik = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.txtBlackboxFile = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLogReducing = new System.Windows.Forms.TextBox();
            this.txtBlackboxReducing = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 186);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 52;
            this.label10.Text = "Password database";
            // 
            // textBoxDBPasswort
            // 
            this.textBoxDBPasswort.Location = new System.Drawing.Point(152, 183);
            this.textBoxDBPasswort.Name = "textBoxDBPasswort";
            this.textBoxDBPasswort.Size = new System.Drawing.Size(449, 20);
            this.textBoxDBPasswort.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 108);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 50;
            this.label9.Text = "Server database";
            // 
            // textBoxDBServer
            // 
            this.textBoxDBServer.Location = new System.Drawing.Point(152, 105);
            this.textBoxDBServer.Name = "textBoxDBServer";
            this.textBoxDBServer.Size = new System.Drawing.Size(449, 20);
            this.textBoxDBServer.TabIndex = 4;
            // 
            // buttonDurchsuchenModell
            // 
            this.buttonDurchsuchenModell.Location = new System.Drawing.Point(152, 64);
            this.buttonDurchsuchenModell.Name = "buttonDurchsuchenModell";
            this.buttonDurchsuchenModell.Size = new System.Drawing.Size(127, 35);
            this.buttonDurchsuchenModell.TabIndex = 3;
            this.buttonDurchsuchenModell.Text = "Search";
            this.buttonDurchsuchenModell.UseVisualStyleBackColor = true;
            this.buttonDurchsuchenModell.Click += new System.EventHandler(this.buttonDurchsuchenModell_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(302, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "SeperatTimestamp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 238);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Seperator I/Os:";
            // 
            // comboBoxSeperatorTStamp
            // 
            this.comboBoxSeperatorTStamp.FormattingEnabled = true;
            this.comboBoxSeperatorTStamp.Items.AddRange(new object[] {
            "\'\\t\'"});
            this.comboBoxSeperatorTStamp.Location = new System.Drawing.Point(442, 235);
            this.comboBoxSeperatorTStamp.Name = "comboBoxSeperatorTStamp";
            this.comboBoxSeperatorTStamp.Size = new System.Drawing.Size(66, 21);
            this.comboBoxSeperatorTStamp.TabIndex = 10;
            // 
            // comboBoxSeperatorIO
            // 
            this.comboBoxSeperatorIO.FormattingEnabled = true;
            this.comboBoxSeperatorIO.Items.AddRange(new object[] {
            "\'\\t\'",
            "keiner"});
            this.comboBoxSeperatorIO.Location = new System.Drawing.Point(152, 235);
            this.comboBoxSeperatorIO.Name = "comboBoxSeperatorIO";
            this.comboBoxSeperatorIO.Size = new System.Drawing.Size(66, 21);
            this.comboBoxSeperatorIO.TabIndex = 9;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(12, 392);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(152, 35);
            this.buttonApply.TabIndex = 13;
            this.buttonApply.Text = "Save and close";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBoxNameDatenbank
            // 
            this.textBoxNameDatenbank.Location = new System.Drawing.Point(152, 157);
            this.textBoxNameDatenbank.Name = "textBoxNameDatenbank";
            this.textBoxNameDatenbank.Size = new System.Drawing.Size(449, 20);
            this.textBoxNameDatenbank.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Name database";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(152, 209);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxPort.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Port SPS:";
            // 
            // textBoxDBUser
            // 
            this.textBoxDBUser.Location = new System.Drawing.Point(152, 131);
            this.textBoxDBUser.Name = "textBoxDBUser";
            this.textBoxDBUser.Size = new System.Drawing.Size(449, 20);
            this.textBoxDBUser.TabIndex = 5;
            // 
            // textBoxModellDatei
            // 
            this.textBoxModellDatei.Location = new System.Drawing.Point(152, 38);
            this.textBoxModellDatei.Name = "textBoxModellDatei";
            this.textBoxModellDatei.Size = new System.Drawing.Size(449, 20);
            this.textBoxModellDatei.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "User database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Model:";
            // 
            // openXMLFileDialog
            // 
            this.openXMLFileDialog.DefaultExt = "xml";
            this.openXMLFileDialog.FileName = "*.xml";
            this.openXMLFileDialog.Filter = "\"xml-Dateien|*.xml|Alle Dateien|*.*\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(302, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Connection Monitoring:";
            // 
            // txtUpdateInterval
            // 
            this.txtUpdateInterval.Location = new System.Drawing.Point(442, 209);
            this.txtUpdateInterval.Name = "txtUpdateInterval";
            this.txtUpdateInterval.Size = new System.Drawing.Size(48, 20);
            this.txtUpdateInterval.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(496, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "[seconds]";
            // 
            // lblAnlage
            // 
            this.lblAnlage.AutoSize = true;
            this.lblAnlage.Location = new System.Drawing.Point(12, 15);
            this.lblAnlage.Name = "lblAnlage";
            this.lblAnlage.Size = new System.Drawing.Size(62, 13);
            this.lblAnlage.TabIndex = 63;
            this.lblAnlage.Text = "Plant Name";
            // 
            // txtAnlage
            // 
            this.txtAnlage.Location = new System.Drawing.Point(152, 12);
            this.txtAnlage.Name = "txtAnlage";
            this.txtAnlage.Size = new System.Drawing.Size(449, 20);
            this.txtAnlage.TabIndex = 1;
            // 
            // chkAutomatik
            // 
            this.chkAutomatik.AutoSize = true;
            this.chkAutomatik.Location = new System.Drawing.Point(152, 316);
            this.chkAutomatik.Name = "chkAutomatik";
            this.chkAutomatik.Size = new System.Drawing.Size(15, 14);
            this.chkAutomatik.TabIndex = 12;
            this.chkAutomatik.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 316);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 13);
            this.label11.TabIndex = 64;
            this.label11.Text = "Automatic start up";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 265);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 13);
            this.label12.TabIndex = 65;
            this.label12.Text = "Log maximum file size";
            // 
            // txtLogFile
            // 
            this.txtLogFile.Location = new System.Drawing.Point(152, 262);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(48, 20);
            this.txtLogFile.TabIndex = 66;
            // 
            // txtBlackboxFile
            // 
            this.txtBlackboxFile.Location = new System.Drawing.Point(152, 288);
            this.txtBlackboxFile.Name = "txtBlackboxFile";
            this.txtBlackboxFile.Size = new System.Drawing.Size(48, 20);
            this.txtBlackboxFile.TabIndex = 67;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 291);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(134, 13);
            this.label13.TabIndex = 68;
            this.label13.Tag = "";
            this.label13.Text = "Blackbox maximum file size";
            // 
            // txtLogReducing
            // 
            this.txtLogReducing.Location = new System.Drawing.Point(442, 262);
            this.txtLogReducing.Name = "txtLogReducing";
            this.txtLogReducing.Size = new System.Drawing.Size(48, 20);
            this.txtLogReducing.TabIndex = 69;
            // 
            // txtBlackboxReducing
            // 
            this.txtBlackboxReducing.Location = new System.Drawing.Point(442, 288);
            this.txtBlackboxReducing.Name = "txtBlackboxReducing";
            this.txtBlackboxReducing.Size = new System.Drawing.Size(48, 20);
            this.txtBlackboxReducing.TabIndex = 70;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(302, 291);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(119, 13);
            this.label14.TabIndex = 71;
            this.label14.Tag = "";
            this.label14.Text = "Blackbox reducing lines";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(206, 265);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 13);
            this.label15.TabIndex = 72;
            this.label15.Text = "[MB]";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(206, 291);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 13);
            this.label16.TabIndex = 73;
            this.label16.Text = "[MB]";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(302, 265);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(93, 13);
            this.label17.TabIndex = 74;
            this.label17.Tag = "";
            this.label17.Text = "Log reducing lines";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(496, 265);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 13);
            this.label18.TabIndex = 75;
            this.label18.Text = "[lines]";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(496, 291);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 76;
            this.label19.Text = "[lines]";
            // 
            // ViewSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 439);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtBlackboxReducing);
            this.Controls.Add(this.txtLogReducing);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtBlackboxFile);
            this.Controls.Add(this.txtLogFile);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chkAutomatik);
            this.Controls.Add(this.txtAnlage);
            this.Controls.Add(this.lblAnlage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUpdateInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxDBPasswort);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxDBServer);
            this.Controls.Add(this.buttonDurchsuchenModell);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxSeperatorTStamp);
            this.Controls.Add(this.comboBoxSeperatorIO);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.textBoxNameDatenbank);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDBUser);
            this.Controls.Add(this.textBoxModellDatei);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "ViewSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.LoadContent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxDBPasswort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDBServer;
        private System.Windows.Forms.Button buttonDurchsuchenModell;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxSeperatorTStamp;
        private System.Windows.Forms.ComboBox comboBoxSeperatorIO;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBoxNameDatenbank;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDBUser;
        private System.Windows.Forms.TextBox textBoxModellDatei;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openXMLFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUpdateInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAnlage;
        private System.Windows.Forms.TextBox txtAnlage;
        private System.Windows.Forms.CheckBox chkAutomatik;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.TextBox txtBlackboxFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLogReducing;
        private System.Windows.Forms.TextBox txtBlackboxReducing;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
    }
}