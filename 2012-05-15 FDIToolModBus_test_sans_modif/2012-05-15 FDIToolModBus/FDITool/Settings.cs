using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool
{
    public class Settings
    {        
        /// <summary>
        /// Seperates the inputs from the outputs. 'n' if none.
        /// </summary>
        public char SeperatorIOs { get; set; }


        /// <summary>
        /// Seperates the tStamp from the I/Os
        /// </summary>
        public char SeperatorTStamp { get; set; }


        /// <summary>
        /// Name of the xml-file that contains the automata structure
        /// </summary>
        public string FileNameAutomataStructure { get; set; }


        /// <summary>
        /// Port for the UDP connection
        /// </summary>
        public int Port { get; set; }


        /// <summary>
        /// Username for the data base
        /// </summary>
        public string DBUserName { get; set; }


        /// <summary>
        /// Name of the data base
        /// </summary>
        public string DBName { get; set; }


        /// <summary>
        /// Password for data base access
        /// </summary>
        public string DBPassword { get; set; }


        /// <summary>
        /// Server with data base
        /// </summary>
        public string DBServer { get; set; }


        /// <summary>
        /// Dauer in Sekunden wie lange gewartet wird bis erneut ein NoConnection Entry in Datenbank geschrieben wird
        /// </summary>
        public int UpdateDataBaseDelay { get; set; }


        /// <summary>
        /// Name der diangnostizierten Anlage
        /// </summary>
        public string PlantName { get; set; }


        /// <summary>
        /// Automatisch beim Laden des Fensters mit der Diagnose starten
        /// </summary>
        public bool AutomaticStart { get; set; }


        /// <summary>
        /// Speichern der Einstellung ob die Datenbank beschrieben werden soll
        /// </summary>
        public bool WriteDataBase { get; set; }


        /// <summary>
        /// Muss true sein um die Logausgabe zu veranlassen
        /// </summary>
        public bool LogWriterToken { get; set; }


        /// <summary>
        /// Muss true sein um die Blackbox Datei mitzuschreiben
        /// </summary>
        public bool BlackBoxWriterToken { get; set; }


        /// <summary>
        /// Maximale Größe der LogFile in MB
        /// </summary>
        public int SizeLogFile { get; set; }


        /// <summary>
        /// Maximale Größe der BlackboxFile in MB
        /// </summary>
        public int SizeBlackboxFile { get; set; }


        /// <summary>
        /// Anzahl der abzuschneidenten Zeilen nach der Dateigrößenreduktion
        /// </summary>
        public int LinesLogFile { get; set; }


        /// <summary>
        /// Anzahl der abzuschneidenten Zeilen nach der Dateigrößenreduktion
        /// </summary>
        public int LinesBlackboxFile { get; set; }
    }
}
