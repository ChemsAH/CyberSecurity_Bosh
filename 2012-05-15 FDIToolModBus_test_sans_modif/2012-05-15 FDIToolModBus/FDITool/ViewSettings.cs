using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FDITool
{
    public partial class ViewSettings : Form
    {
        private Model myModel;
        private Controller myController;

        public ViewSettings(Model argModel, Controller argController)
        {
            myModel = argModel;
            myController = argController;
            InitializeComponent();

            
        }


        /// <summary>
        /// Laden der Steuerelementeinhalte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadContent(object sender, EventArgs e)
        {
            textBoxDBUser.Text = myModel.MySettings.DBUserName;
            textBoxModellDatei.Text = myModel.MySettings.FileNameAutomataStructure;
            textBoxNameDatenbank.Text = myModel.MySettings.DBName;
            textBoxPort.Text = myModel.MySettings.Port.ToString();
            textBoxDBPasswort.Text = myModel.MySettings.DBPassword;
            textBoxDBServer.Text = myModel.MySettings.DBServer;
            txtUpdateInterval.Text = myModel.MySettings.UpdateDataBaseDelay.ToString();
            txtAnlage.Text = myModel.MySettings.PlantName;
            if (myModel.MySettings.SeperatorIOs == '\t') comboBoxSeperatorIO.SelectedIndex = 0;
            if (myModel.MySettings.SeperatorIOs == 'n') comboBoxSeperatorIO.SelectedIndex = 1;
            if (myModel.MySettings.SeperatorTStamp == '\t') comboBoxSeperatorTStamp.SelectedIndex = 0;
            if (myModel.MySettings.AutomaticStart) chkAutomatik.Checked = true;
            else myModel.MySettings.AutomaticStart = false;
            txtLogFile.Text = myModel.MySettings.SizeLogFile.ToString();
            txtBlackboxFile.Text = myModel.MySettings.SizeBlackboxFile.ToString();
            txtLogReducing.Text = myModel.MySettings.LinesLogFile.ToString();
            txtBlackboxReducing.Text = myModel.MySettings.LinesBlackboxFile.ToString();
        }


        /// <summary>
        /// Eingabe des Modellpfades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDurchsuchenModell_Click(object sender, EventArgs e)
        {
            openXMLFileDialog.InitialDirectory = Application.StartupPath;

            if (openXMLFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxModellDatei.Text = openXMLFileDialog.FileName; // Steuerlement aktualisieren
                myModel.MySettings.FileNameAutomataStructure = textBoxModellDatei.Text; // Settingseintrag aktualisieren
            }
        }


        /// <summary>
        /// Config übernehmen und schließen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            myModel.MySettings.DBName = textBoxNameDatenbank.Text;
            myModel.MySettings.DBUserName = textBoxDBUser.Text;
            myModel.MySettings.DBServer = textBoxDBServer.Text;
            myModel.MySettings.DBPassword = textBoxDBPasswort.Text;
            myModel.MySettings.FileNameAutomataStructure = textBoxModellDatei.Text;
            myModel.MySettings.Port = Convert.ToInt32(textBoxPort.Text);
            myModel.MySettings.UpdateDataBaseDelay = Convert.ToInt32(txtUpdateInterval.Text);
            myModel.MySettings.PlantName = txtAnlage.Text;
            if ((string)comboBoxSeperatorIO.SelectedItem == "'\\t'")
                myModel.MySettings.SeperatorIOs = '\t';
            if ((string)comboBoxSeperatorIO.SelectedItem == "keiner")
                myModel.MySettings.SeperatorIOs = 'n';
            if ((string)comboBoxSeperatorTStamp.SelectedItem == "'\\t'")
                myModel.MySettings.SeperatorTStamp = '\t';
            if (chkAutomatik.Checked)
                myModel.MySettings.AutomaticStart = true;
            else
                myModel.MySettings.AutomaticStart = false;

            Log.SizeLogFile = Convert.ToInt32(txtLogFile.Text);
            myModel.MySettings.SizeLogFile = Convert.ToInt32(txtLogFile.Text);

            Log.LinesLogFile = Convert.ToInt32(txtLogReducing.Text);
            myModel.MySettings.LinesLogFile = Convert.ToInt32(txtLogReducing.Text);

            Log.SizeBlackboxFile = Convert.ToInt32(txtBlackboxFile.Text);
            myModel.MySettings.SizeBlackboxFile = Convert.ToInt32(txtBlackboxFile.Text);

            Log.LinesBlackboxFile = Convert.ToInt32(txtBlackboxReducing.Text);
            myModel.MySettings.LinesBlackboxFile = Convert.ToInt32(txtBlackboxReducing.Text);

            if (!this.myController.SaveConfigXML(Application.StartupPath + "\\config.xml"))
            {
                Log.WriteLine("Konfigurationsdatei konnte nicht gespeichert werden");
            }

            Close();
        }
    }
}
