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
    public partial class ViewMain : Form
    {
        private Model myModel;
        private Controller myController;
        private ViewSettings myViewSettings;
        private ViewInfo myViewInfo;


        /// <summary>
        /// Versionsinformation
        /// </summary>
        private string myVersion;


        /// <summary>
        /// Zähler zum Skalieren der Farbänderung des Kreises
        /// </summary>
        private int receiveCounter;
       

        public ViewMain(Model argModel, Controller argController, string argVersion)
        {
            myModel = argModel;
            myController = argController;
            myVersion = argVersion;

            myViewSettings = new ViewSettings(argModel, argController);
            myViewInfo = new ViewInfo();
            
            receiveCounter = 0; // Initialisierung

            myModel.MyProcessMonitor.SendToAutTextBoxEvent += new ProcessMonitoring.AutBoxHandler(SendToAutBox); // AutBoxEvent abbonieren
            myModel.MyProcessMonitor.UdpReceiveEvent += new ProcessMonitoring.UdpReceiveHandler(ToggleCircleColor); // Paketerhaltungsevent abbonieren

            InitializeComponent();

            automaticStart(); // Automatikstart initialisieren
        }


        /// <summary>
        /// Ladet automatisch die Konfigdatei, die Modelldatei und startet die Diagnose wenn dies aktiviert wurde
        /// </summary>
        private void automaticStart()
        {
            if (!myController.LoadConfigXML(Application.StartupPath + "\\config.xml")) // Laden der Configdatei
                MessageBox.Show("FEHLER: Konfiguration konnte nicht geladen werden");
            else
            {
                SendToLogBox("Konfiguration erfolgreich geladen");

                if (myModel.MySettings.WriteDataBase == true)
                    datenbankSchreibenToolStripMenuItem.Checked = true;
                else
                    datenbankSchreibenToolStripMenuItem.Checked = false;

                if (myModel.MySettings.LogWriterToken == true)
                    logSchreibenToolStripMenuItem.Checked = true;
                else
                    logSchreibenToolStripMenuItem.Checked = false;

                if (myModel.MySettings.BlackBoxWriterToken == true)
                    BlackBoxSchreibenToolStripMenuItem.Checked = true;
                else
                    BlackBoxSchreibenToolStripMenuItem.Checked = false;

                if (!myController.LoadModelFiles()) // Laden der Modelldatei
                    MessageBox.Show("FEHLER: Modelle konnten nicht geladen werden");
                else
                    SendToLogBox("Modelle erfolgreich geladen");
            }

            this.Text = "FDITool " + myVersion + " " + myModel.MySettings.PlantName; // Setzen des Fensternamens mit Versionsummer und Anlagenbezeichnung

            if (myModel.MySettings.AutomaticStart)
                startToolStripMenuItem_Click(new object(), new EventArgs()); // Falls aktiviert, starte automatisch den Diagnosebetrieb
        }


        /// <summary>
        /// MENUSTRIP: Startet die Diagnose mit Parametern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            Log.LogWriterToken = logSchreibenToolStripMenuItem.Checked;
            Log.BlackBoxWriterToken = BlackBoxSchreibenToolStripMenuItem.Checked;

            //myModel.DataBaseWriteToken = datenbankSchreibenToolStripMenuItem.Checked;

            myController.StartDiagnosis();

            ovalShape2.FillColor = Color.Red;

            SendToLogBox("Diagnose gestartet");
        }


        /// <summary>
        /// MENUSTRIP: Stoppt die Diagnose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myController.StopDiagnosis();

            ovalShape2.FillColor = Color.White;

            SendToLogBox("Diagnose gestoppt");        
        }


        /// <summary>
        /// MENUSTRIP: Zeigt Form mit Programmiformationen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myViewInfo.ShowDialog();
        }


        /// <summary>
        /// MENUSTRIP: Bearbeitet die Configdatei
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void einstellungenBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myViewSettings.ShowDialog(); // Anzeigen des Configfensters

            if (!myController.LoadModelFiles()) // Laden der Modelldatei
                SendToLogBox("Modelle konnten nicht geladen werden");
            else
                SendToLogBox("Modelle erfolgreich geladen");

            this.Text = "FDITool " + myVersion + " " + myModel.MySettings.PlantName; // Setzen des Fensternamens mit Versionsummer und Anlagenbezeichnung
        }


        /// <summary>
        /// Wenn Event UdpReceiveEvent geworfen wird, dann soll nach jedem 10 Event die Farbe des Kreises geändert werden (threadsicher)
        /// </summary>
        private void ToggleCircleColor()
        {
            receiveCounter++; // Zähler der kontinuierlich hochzählt

            if (receiveCounter == 10) // Alle 10 ticks wird die Farbe genändert
            {
                MethodInvoker LabelUpdate = delegate
                {
                    if (ovalShape1.FillColor == Color.White)
                        ovalShape1.FillColor = Color.Blue;
                    else 
                        ovalShape1.FillColor = Color.White;
                };

                Invoke(LabelUpdate);
                receiveCounter = 0;
            }
        }


        /// <summary>
        /// Textbox die den Automatenstatus anzeigen soll (threadsicher)
        /// </summary>
        /// <param name="argMessage"></param>
        private void SendToAutBox(string argMessage)
        {
            try
            {
                if (argMessage != "") // Nachrichteninhalt ausgeben
                {
                    if (richTextBoxAut.InvokeRequired)
                        richTextBoxAut.Invoke(new Action<string>(SendToAutBox), new object[] { argMessage });
                    else
                        richTextBoxAut.Text = richTextBoxAut.Text + argMessage + "\n";
                }
                else // Reset der Textbox
                {
                    if (richTextBoxAut.InvokeRequired)
                        richTextBoxAut.Invoke(new Action<string>(SendToAutBox), new object[] { argMessage });
                    else
                        richTextBoxAut.Text = "";
                }
            }
            catch { }
        }


        /// <summary>
        /// Textbox für allgemeine Logeinträge (threadsicher)
        /// </summary>
        /// <param name="argMessage"></param>
        private void SendToLogBox(string argMessage)
        {
            try
            {
                if (argMessage != "") // Nachrichteninhalt ausgeben
                {
                    if (richTextBoxLog.InvokeRequired)
                        richTextBoxLog.Invoke(new Action<string>(SendToLogBox), new object[] { argMessage });
                    else
                        richTextBoxLog.Text = DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + " " + DateTime.Now.ToLongTimeString() + "\t" + argMessage + "\n" + richTextBoxLog.Text;
                }
                else // Inhalt zurücksetzen
                {
                    if (richTextBoxLog.InvokeRequired)
                        richTextBoxLog.Invoke(new Action<string>(SendToLogBox), new object[] { argMessage });
                    else
                        richTextBoxLog.Text = "";
                }
            }
            catch { }
        }


        /// <summary>
        /// FORM: Wenn Schließen verlangt wird Diagnoseprozess beenden und UDP Verbindung kappen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            myController.StopDiagnosis();

            if (myModel.MyDataBase != null)
                myModel.MyDataBase.CloseConnection();
        }

        private void datenbankSchreibenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datenbankSchreibenToolStripMenuItem.Checked == true)
                myModel.MySettings.WriteDataBase = true;
            else
                myModel.MySettings.WriteDataBase = false;
        }

        private void logSchreibenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logSchreibenToolStripMenuItem.Checked == true)
            {
                Log.LogWriterToken = true;
                myModel.MySettings.LogWriterToken = true;
            }
            else
            {
                Log.LogWriterToken = false;
                myModel.MySettings.LogWriterToken = false;
            }
        }

        private void BlackBoxSchreibenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BlackBoxSchreibenToolStripMenuItem.Checked == true)
            {
                Log.BlackBoxWriterToken = true;
                myModel.MySettings.BlackBoxWriterToken = true;
            }
            else
            {
                Log.BlackBoxWriterToken = false;
                myModel.MySettings.BlackBoxWriterToken = false;
            }
        }
    }
}
