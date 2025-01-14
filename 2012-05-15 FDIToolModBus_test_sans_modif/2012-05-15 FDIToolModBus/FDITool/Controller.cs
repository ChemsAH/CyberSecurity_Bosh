using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FDITool
{
    public class Controller
    {
        private Model myModel;
        private XML myXML;

        public Controller(Model model)
        {
            myModel = model;
            myXML = new XML(myModel);
            myModel.MySettings = new Settings();
            myModel.MyProcessMonitor = new ProcessMonitoring(myModel);
        }


        /// <summary>
        /// Automatenmodelle laden
        /// </summary>
        /// <returns></returns>
        public bool LoadModelFiles()
        {
            if (!File.Exists(myModel.MySettings.FileNameAutomataStructure))
                return false;

            return myXML.LoadAutomataStructureXML(myModel.MySettings.FileNameAutomataStructure);
        }


        /// <summary>
        /// Configdatei laden
        /// </summary>
        /// <param name="argFileName"></param>
        /// <returns></returns>
        public bool LoadConfigXML(string argFileName)
        {
            if(!File.Exists(argFileName))
                return false;

            return myXML.LoadConfigXML(argFileName);
        }


        /// <summary>
        /// Configdatei speichern
        /// </summary>
        /// <param name="argFileName"></param>
        /// <returns></returns>
        public bool SaveConfigXML(string argFileName)
        {
            return myXML.SaveConfigXML(argFileName);
        }


        /// <summary>
        /// Starts the data collection and the diagnosis task
        /// </summary>
        public void StartDiagnosis()
        {
            if (File.Exists("log.txt")) File.Delete("log.txt");
            if (File.Exists("error.txt")) File.Delete("error.txt");
            if (File.Exists("blackbox.txt")) File.Delete("blackbox.txt");

            myModel.MyDataBase = new DataBase(myModel);

            myModel.consideredBits = myModel.MyNetwork.GetIndexListIOs(); // Alle relevanten IOs bestimmen

            // THREAD für Diagnose mit höhrer Prozessorpriorität initialisieren

            myModel.OnlineDiagnosisThread = new Thread(myModel.MyProcessMonitor.TimedProcessMonitoring);
            myModel.OnlineDiagnosisThread.Priority = ThreadPriority.AboveNormal;
            myModel.OnlineDiagnosisThread.Start();
        }


        /// <summary>
        /// Stops the data collection and diagnosis task
        /// </summary>
        public void StopDiagnosis()
        {
            myModel.DiagnosisThreadToken = false;
        } 
    }
}
