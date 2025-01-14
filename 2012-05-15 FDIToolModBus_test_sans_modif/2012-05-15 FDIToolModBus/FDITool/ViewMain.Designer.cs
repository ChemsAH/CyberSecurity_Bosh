namespace FDITool
{
    partial class ViewMain
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.diagnoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenAnzeigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datenbankSchreibenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logSchreibenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BlackBoxSchreibenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxAut = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.ovalShape1 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.label1 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ovalShape2 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagnoseToolStripMenuItem,
            this.einstellungenToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(615, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // diagnoseToolStripMenuItem
            // 
            this.diagnoseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.infoToolStripMenuItem1});
            this.diagnoseToolStripMenuItem.Name = "diagnoseToolStripMenuItem";
            this.diagnoseToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.diagnoseToolStripMenuItem.Text = "Diagnosis";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem1
            // 
            this.infoToolStripMenuItem1.Name = "infoToolStripMenuItem1";
            this.infoToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.infoToolStripMenuItem1.Text = "Info";
            this.infoToolStripMenuItem1.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.einstellungenAnzeigenToolStripMenuItem,
            this.datenbankSchreibenToolStripMenuItem,
            this.logSchreibenToolStripMenuItem,
            this.BlackBoxSchreibenToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.einstellungenToolStripMenuItem.Text = "Preferences";
            // 
            // einstellungenAnzeigenToolStripMenuItem
            // 
            this.einstellungenAnzeigenToolStripMenuItem.Name = "einstellungenAnzeigenToolStripMenuItem";
            this.einstellungenAnzeigenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.einstellungenAnzeigenToolStripMenuItem.Text = "Change";
            this.einstellungenAnzeigenToolStripMenuItem.Click += new System.EventHandler(this.einstellungenBearbeitenToolStripMenuItem_Click);
            // 
            // datenbankSchreibenToolStripMenuItem
            // 
            this.datenbankSchreibenToolStripMenuItem.Checked = true;
            this.datenbankSchreibenToolStripMenuItem.CheckOnClick = true;
            this.datenbankSchreibenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.datenbankSchreibenToolStripMenuItem.Name = "datenbankSchreibenToolStripMenuItem";
            this.datenbankSchreibenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.datenbankSchreibenToolStripMenuItem.Text = "Write database";
            this.datenbankSchreibenToolStripMenuItem.Click += new System.EventHandler(this.datenbankSchreibenToolStripMenuItem_Click);
            // 
            // logSchreibenToolStripMenuItem
            // 
            this.logSchreibenToolStripMenuItem.Checked = true;
            this.logSchreibenToolStripMenuItem.CheckOnClick = true;
            this.logSchreibenToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logSchreibenToolStripMenuItem.Name = "logSchreibenToolStripMenuItem";
            this.logSchreibenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logSchreibenToolStripMenuItem.Text = "Write log";
            this.logSchreibenToolStripMenuItem.Click += new System.EventHandler(this.logSchreibenToolStripMenuItem_Click);
            // 
            // BlackBoxSchreibenToolStripMenuItem
            // 
            this.BlackBoxSchreibenToolStripMenuItem.CheckOnClick = true;
            this.BlackBoxSchreibenToolStripMenuItem.Name = "BlackBoxSchreibenToolStripMenuItem";
            this.BlackBoxSchreibenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.BlackBoxSchreibenToolStripMenuItem.Text = "Write blackbox";
            this.BlackBoxSchreibenToolStripMenuItem.Click += new System.EventHandler(this.BlackBoxSchreibenToolStripMenuItem_Click);
            // 
            // richTextBoxAut
            // 
            this.richTextBoxAut.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxAut.Name = "richTextBoxAut";
            this.richTextBoxAut.ReadOnly = true;
            this.richTextBoxAut.Size = new System.Drawing.Size(579, 157);
            this.richTextBoxAut.TabIndex = 8;
            this.richTextBoxAut.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxAut);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 182);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Automata";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBoxLog);
            this.groupBox2.Location = new System.Drawing.Point(12, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(591, 189);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(579, 164);
            this.richTextBoxLog.TabIndex = 9;
            this.richTextBoxLog.Text = "";
            // 
            // ovalShape1
            // 
            this.ovalShape1.FillColor = System.Drawing.Color.White;
            this.ovalShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.ovalShape1.Location = new System.Drawing.Point(16, 416);
            this.ovalShape1.Name = "ovalShape1";
            this.ovalShape1.Size = new System.Drawing.Size(11, 11);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "UDP Receiving";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ovalShape2,
            this.ovalShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(615, 441);
            this.shapeContainer1.TabIndex = 11;
            this.shapeContainer1.TabStop = false;
            // 
            // ovalShape2
            // 
            this.ovalShape2.FillColor = System.Drawing.Color.White;
            this.ovalShape2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.ovalShape2.Location = new System.Drawing.Point(157, 417);
            this.ovalShape2.Name = "ovalShape2";
            this.ovalShape2.Size = new System.Drawing.Size(11, 11);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Diagnosis Active";
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 441);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ViewMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.View_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem diagnoseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxAut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenAnzeigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datenbankSchreibenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logSchreibenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BlackBoxSchreibenToolStripMenuItem;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape2;
        private System.Windows.Forms.Label label2;

    }
}

