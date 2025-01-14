using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FDITool
{
    public static class Log
    {

        /// <summary>
        /// Muss true sein um die Logausgabe zu veranlassen
        /// </summary>
        public static bool LogWriterToken;


        /// <summary>
        /// Muss true sein um die Blackbox Datei mitzuschreiben
        /// </summary>
        public static bool BlackBoxWriterToken;


        /// <summary>
        /// Maximale Größe der LogFile in MB
        /// </summary>
        public static int SizeLogFile;


        /// <summary>
        /// Anzahl der abzuschneidenten Zeilen nach der Dateigrößenreduktion
        /// </summary>
        public static int LinesLogFile;


        /// <summary>
        /// Anzahl der abzuschneidenten Zeilen nach der Dateigrößenreduktion
        /// </summary>
        public static int LinesBlackboxFile;


        /// <summary>
        /// Maximale Größe der BlackboxFile in MB
        /// </summary>
        public static int SizeBlackboxFile;


        public static void InitLogFile()
        {
            if (LogWriterToken)
            {
                StreamWriter myStreamWriter = new StreamWriter("log.txt");
                myStreamWriter.Close();
            }
        }


        /// <summary>
        /// Logdateiausgabe
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {
            if (LogWriterToken)
            {
                try
                {
                    Console.WriteLine(DateTime.Now.ToString() + " " + message);
                    //TextFileSizeCheck("log.txt", SizeLogFile, LinesLogFile);
                    StreamWriter writer = new StreamWriter("log.txt", true);
                    writer.WriteLine(DateTime.Now.ToString() + " " + message);
                    writer.Close();
                }
                catch (Exception e)
                {
                    ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                }
            }
        }


        /// <summary>
        /// Exceptiondateiausgabe
        /// </summary>
        /// <param name="message"></param>
        public static void ExceptionWriter(string message)
        {
            try
            {
                StreamWriter writer = new StreamWriter("error.txt", true);
                writer.WriteLine(DateTime.Now.ToString() + " " + message);
                writer.Close();
            }
            catch { }
        }


        /// <summary>
        /// Initialisiere Blackbox Datei für Aufnahme bzw. für Festellen der Dateigröße
        /// </summary>
        public static void InitBlackBoxFile()
        {
            if (BlackBoxWriterToken)
            {
                TextWriter myStreamWriter = new StreamWriter("blackbox.txt"); // Initialisiere 1. Datei (notwendig für Dateigrößenabfrage)
                myStreamWriter.Close();
            }
        }


        /// <summary>
        /// Schreibe in die Blackbox Datei mit Zeitstempel, deltaT und IOVektor
        /// </summary>
        /// <param name="deltaT"></param>
        /// <param name="vector"></param>
        public static void BlackBoxWriter(string deltaT, string vector)
        {
            if (BlackBoxWriterToken) // falls aktiviert
            {
                try
                {
                    //TextFileSizeCheck("blackbox.txt", SizeBlackboxFile, LinesBlackboxFile);
                    StreamWriter writer = new StreamWriter("blackbox.txt", true);
                    writer.WriteLine(DateTime.Now.ToString() + "\t" + deltaT + "\t" + vector);
                    writer.Close();
                }
                catch (Exception e)
                {
                    ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                }
            }
        }


        /// <summary>
        /// Überprüfung der Dateigröße der Blackbox Datei. Bei Überschreiten der Größe werden lines Zeilen am Anfang der Datei abgeschnitten.
        /// </summary>
        /// <param name="sizeMB"></param>
        public static void TextFileSizeCheck(string file, double sizeMB, int lines)
        {
            FileInfo myFileInfo = new FileInfo(file); // Checke aktuelle Datei

            if (myFileInfo.Length > sizeMB * 1024 * 1024) // bytes --> KB --> MB, falls die Datei größer als size MB ist
            {
                StringBuilder sb = new StringBuilder();

                int counter = 1;

                foreach (string s in File.ReadAllLines(file, Encoding.Default))
                {
                    if (counter > lines) // Die ersten lines Zeilen ignorieren
                        sb.AppendLine(s); // Zeile zum StringBuilder hinzufügen

                    counter++;
                }

                File.WriteAllText(file, sb.ToString(), Encoding.Default); // Alle hinzugefügten Zeiten schreiben

                Log.WriteLine(file + " reduced");
            }
        }
    }
}
