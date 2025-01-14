using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool
{
    public class Model
    {
        /// <summary>
        /// Network of NDAAOs
        /// </summary>
        public PartialAutomaton.AutomataNetwork MyNetwork { get;  set; }


        /// <summary>
        /// Thread that contains data reading, processing of the automata and database access
        /// </summary>
        public System.Threading.Thread OnlineDiagnosisThread { get; set; }


        /// <summary>
        /// Monitors the data connection. If for more than 1 minute no data arrives, a "Keine Verbindung" message is stored
        /// </summary>
        public System.Threading.Thread ConnectionMonitoring { get; set; }


        /// <summary>
        /// Database access
        /// </summary>
        public DataBase MyDataBase { get; set; }


        /// <summary>
        /// Diagnosis algorithm
        /// </summary>
        public ProcessMonitoring MyProcessMonitor { get; set; }


        /// <summary>
        /// Der UDP-Listener
        /// </summary>
        public System.Net.Sockets.UdpClient MyUdpClient { get; set; }


        /// <summary>
        /// Enthält alle für die Diagnose relevanten Bits.
        /// Wird über die Modell xml geliefert.
        /// </summary>
        public List<int> consideredBits { get; set; }


        /// <summary>
        /// Globale Einstellungen der config Datei
        /// </summary>
        public Settings MySettings { get; set; }


        /// <summary>
        /// Token welches den Diagnosethread am Leben hält
        /// </summary>
        public bool DiagnosisThreadToken { get; set; }


        /// <summary>
        /// Token welches des ConnectionMonitoringThread am Leben hält
        /// </summary>
        public bool MonitoringThreadToken { get; set; }
    }
}
