using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FDITool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string version = "v1.0.6 ModBus"; // Aktuelle Programmversion

            // Version v1.0.5
            //  - Spezielle Version für ModBus MMS Anlage in Cachan

            // Version v1.0.5
            //  - Größe Log Datei und Blackbox und die Anzahl abgeschnittener Zeilen ist nun einstellbar
            //  - Die Einstellungen für Datenbank, Log und Blackbox wird gespeichert
            //  - Blackbox, Logfile Größenreduktion erneut mit kleinen Größen erfolgreich gechecked (Langzeittest mir großen Dateien notwendig)

            // Version v1.0.4
            //  - Bug bei MaxDuration = 0 behoben, wenn nur Tansitionen mit unendlichen Zeitgrenzen in einem Zustand existiert haben, dann war die maximale Grenze nicht editiert worden und ein Deadlock wurde immer erkannt
            //  - Abtastzeitfensterüberwachung auskommentiert

            // Version v1.0.3
            //  - Fault detection bug entfernt bei der Uhr die zu null wurde und zu Abstürzen geführt hat
            //  - Blackbox, logfile schreiben mit größe wurde gecheckt
            //  - Größe noch nicht einstellbar
            //  - Problem bei Zeiterfassung wurde erkannt: Wenn Buffer volläuft werden die Zeitfenster sehr klein bzw. das Erste sehr groß, kann zu Fehlalarmen führen

            Model model = new Model();
            Controller controller = new Controller(model);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewMain(model, controller, version));
        }
    }
}
