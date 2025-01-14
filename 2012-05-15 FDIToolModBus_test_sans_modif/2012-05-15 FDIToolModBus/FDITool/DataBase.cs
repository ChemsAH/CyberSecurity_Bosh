using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool
{
    public class DataBase
    {
        Model myModel;

        /// <summary>
        /// Adapter für Datenbankeinträge
        /// </summary>
        private fehlermeldungenDataSetTableAdapters.tabelleTableAdapter myAdapterTable { get; set; }


        /// <summary>
        /// Gobale Strukturelle Residuen aller Teilautomaten im Eventformat
        /// </summary>
        private List<PartialAutomaton.SingleEvent> globalRes1;
        private List<PartialAutomaton.SingleEvent> globalRes2;
        private List<PartialAutomaton.SingleEvent> globalRes3;
        private List<PartialAutomaton.SingleEvent> globalRes4;


        /// <summary>
        /// Globale Zeitbasierte Residuen aller Teilautomaten im Eventformat
        /// </summary>
        private List<PartialAutomaton.SingleEvent> globalTimedRes1Early;
        private List<PartialAutomaton.SingleEvent> globalTimedRes2Early;
        private List<PartialAutomaton.SingleEvent> globalTimedRes1Late;
        private List<PartialAutomaton.SingleEvent> globalTimedRes2Late;


        /// <summary>
        /// Gobale Strukturelle Residuen aller Teilautomaten im erweiterten Stringformat (interpretiert)
        /// </summary>
        private string stringRes1;
        private string stringRes2;
        private string stringRes3;
        private string stringRes4;


        /// <summary>
        /// Globale Zeitbasierte Residuen aller Teilautomaten im erweiterten Stringformat (interpretiert)
        /// </summary>
        private string stringTimedRes1Early;
        private string stringTimedRes2Early;
        private string stringTimedRes1Late;
        private string stringTimedRes2Late;


        /// <summary>
        /// Gefilterte und zusammengefasste strukturelle Residuen
        /// </summary>
        private string struktResiduals;


        /// <summary>
        /// Gefilterte und zusammengefasste zeitbasierte Residuen
        /// </summary>
        private string timedResiduals;


        /// <summary>
        /// Gibt an wenn sich die strukturellen Residuen der Automaten geändert haben
        /// </summary>
        private bool structuralResidualsChanged;


        /// <summary>
        /// Gibt an wenn sich die zeitbasierten Residuen der Automaten geändert haben
        /// </summary>
        private bool timedResidualsChanged;


        /// <summary>
        /// Konstruktor initialisiert die Verbindung zur Datenbank und die Eventlisten
        /// </summary>
        /// <param name="model"></param>
        public DataBase(Model model)
        {
            myModel = model;

            myAdapterTable = new fehlermeldungenDataSetTableAdapters.tabelleTableAdapter();

            string server = "server=" + myModel.MySettings.DBServer + ";";
            string user = "user id=" + myModel.MySettings.DBUserName + ";";
            string password = "Password=" + myModel.MySettings.DBPassword + ";";
            string database = "database=" + myModel.MySettings.DBName;

            myAdapterTable.Connection.ConnectionString = server + user + password + database; // Datenbankzugriff initialisieren

            globalRes1 = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalRes2 = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalRes3 = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalRes4 = new List<FDITool.PartialAutomaton.SingleEvent>();

            globalTimedRes1Early = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalTimedRes2Early = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalTimedRes1Late = new List<FDITool.PartialAutomaton.SingleEvent>();
            globalTimedRes2Late = new List<FDITool.PartialAutomaton.SingleEvent>();
        }


        /// <summary>
        /// Haupteinstiegspunkt zur sequenziellen Abarbeitung der Datenbankaktualisierung
        /// </summary>
        /// <param name="deltaT"></param>
        public void UpdateDatabase(double deltaT)
        {
            updateStruktResiduenListen(deltaT); // Generierung der Listen mit sämtlichen strukturellen Residuenevents

            updateTimedResiduenListen(deltaT); // Generierung der Listen mit sämtlichen zeiltlichen Residuenevents

            filterStruktResiduen(); // Filtern und konvertieren der strukturellen Residuen in string format

            filterTimedResiduen(); // Filtern und konvertieren der zeitbasierten Residuen in string format

            updateDataBaseDisplay(); // Ausgabe der Residuen in Log und Datenbank
        }


        /// <summary>
        /// Updates the fault message in the data base
        /// If at least one NDAAO is in a deadlock and if the network residuals have not changed since DeadLockRepeatTime, the last message is NOT repeated.
        /// Es wird verglichen ob sich in den Residuen Sets der des Automatennetzwerks etwas verändert hat.
        /// Wenn ja, wird ein neuer Eintrag in die Datenbank generiert.
        /// </summary>
        private void updateStruktResiduenListen(double deltaT)
        {
            structuralResidualsChanged = false;

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalRes1, myModel.MyNetwork.AutRes1))
            {
                structuralResidualsChanged = true;
                globalRes1 = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutRes1)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalRes1.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalRes2, myModel.MyNetwork.AutRes2))
            {
                structuralResidualsChanged = true;
                globalRes2 = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutRes2)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalRes2.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalRes3, myModel.MyNetwork.AutRes3))
            {
                structuralResidualsChanged = true;
                globalRes3 = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutRes3)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalRes3.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalRes4, myModel.MyNetwork.AutRes4))
            {
                structuralResidualsChanged = true;
                globalRes4 = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutRes4)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalRes4.Add(newEdge);
                }
            }
        }


        /// <summary>
        /// Updates the fault message in the data base
        /// If at least one NDAAO is in a deadlock and if the network residuals have not changed since DeadLockRepeatTime, the last message is NOT repeated.
        /// Es wird verglichen ob sich in den Residuen Sets der des Automatennetzwerks etwas verändert hat.
        /// Wenn ja, wird ein neuer Eintrag in die Datenbank generiert.
        /// </summary>
        private void updateTimedResiduenListen(double deltaT)
        {
            timedResidualsChanged = false;

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalTimedRes1Early, myModel.MyNetwork.AutTimedRes1Early))
            {
                timedResidualsChanged = true;
                globalTimedRes1Early = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutTimedRes1Early)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalTimedRes1Early.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalTimedRes2Early, myModel.MyNetwork.AutTimedRes2Early))
            {
                timedResidualsChanged = true;
                globalTimedRes2Early = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutTimedRes2Early)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalTimedRes2Early.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalTimedRes1Late, myModel.MyNetwork.AutTimedRes1Late))
            {
                timedResidualsChanged = true;
                globalTimedRes1Late = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutTimedRes1Late)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalTimedRes1Late.Add(newEdge);
                }
            }

            if (!PartialAutomaton.EdgeListEqualityComparer.Equals(globalTimedRes2Late, myModel.MyNetwork.AutTimedRes2Late))
            {
                timedResidualsChanged = true;
                globalTimedRes2Late = new List<FDITool.PartialAutomaton.SingleEvent>();
                foreach (PartialAutomaton.SingleEvent edge in myModel.MyNetwork.AutTimedRes2Late)
                {
                    PartialAutomaton.SingleEvent newEdge = myModel.MyNetwork.GetEdgeFromIOList(edge);
                    globalTimedRes2Late.Add(newEdge);
                }
            }
        }


        /// <summary>
        /// Die StringResiduen werden 1 zu 1 aus den globalen Residuenlisten generiert.
        /// Der Strukturelle Residuen string wird aus der gefilterten Verkettung aller Einzelresiduen erstellt
        /// Doppelte Einträge werden im 2. Fall nicht übernommen
        /// </summary>
        private void filterStruktResiduen()
        {
            List<string> struktEdges = new List<string>();

            struktResiduals = ""; // String für Datenbankeintrag

            stringRes1 = "";
            stringRes2 = "";
            stringRes3 = "";
            stringRes4 = "";

            foreach (PartialAutomaton.SingleEvent edge in globalRes1)
            {
                struktEdges.Add(edge.Name);
                struktResiduals += convertEdgesUnexpected(edge.Name);

                stringRes1 += convertEdgesUnexpected(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalRes2)
            {
                if (!struktEdges.Contains(edge.Name))
                {
                    struktEdges.Add(edge.Name);
                    struktResiduals += convertEdgesUnexpected(edge.Name);
                }

                stringRes2 += convertEdgesUnexpected(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalRes3)
            {
                if (!struktEdges.Contains(edge.Name))
                {
                    struktEdges.Add(edge.Name);
                    struktResiduals += convertDiagnosisEdgesMissed(edge.Name);
                }

                stringRes3 += convertDiagnosisEdgesMissed(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalRes4)
            {
                if (!struktEdges.Contains(edge.Name))
                {
                    struktEdges.Add(edge.Name);
                    struktResiduals += convertDiagnosisEdgesMissed(edge.Name);
                }

                stringRes4 += convertDiagnosisEdgesMissed(edge.Name);
            }
        }


        /// <summary>
        /// Die StringResiduen werden 1 zu 1 aus den globalen Residuenlisten generiert.
        /// Der Zeitbasierten Residuen string wird aus der gefilterten Verkettung aller Einzelresiduen erstellt
        /// Doppelte Einträge werden im 2. Fall nicht übernommen
        /// </summary>
        private void filterTimedResiduen()
        {
            List<string> timedEdges = new List<string>();

            timedResiduals = "";

            stringTimedRes1Early = "";
            stringTimedRes2Early = "";
            stringTimedRes1Late = "";
            stringTimedRes2Late = "";

            foreach (PartialAutomaton.SingleEvent edge in globalTimedRes1Early)
            {
                timedEdges.Add(edge.Name);
                timedResiduals += convertTimedDiagnosisEdges(edge.Name);
                stringTimedRes1Early += convertTimedDiagnosisEdges(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalTimedRes2Early)
            {
                if (!timedEdges.Contains(edge.Name))
                {
                    timedEdges.Add(edge.Name);
                    timedResiduals += convertTimedDiagnosisEdges(edge.Name);
                }

                stringTimedRes2Early += convertTimedDiagnosisEdges(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalTimedRes1Late)
            {
                if (!timedEdges.Contains(edge.Name))
                {
                    timedEdges.Add(edge.Name);
                    timedResiduals += convertTimedDiagnosisEdges(edge.Name);
                }

                stringTimedRes1Late += convertTimedDiagnosisEdges(edge.Name);
            }

            foreach (PartialAutomaton.SingleEvent edge in globalTimedRes2Late)
            {
                if (!timedEdges.Contains(edge.Name))
                {
                    timedEdges.Add(edge.Name);
                    timedResiduals += convertTimedDiagnosisEdges(edge.Name);
                }

                stringTimedRes2Late += convertTimedDiagnosisEdges(edge.Name);
            }
        }


        /// <summary>
        /// Einträge für die Logfiles und Datenbank fertig generierung ein schreiben
        /// </summary>
        private void updateDataBaseDisplay()
        {
            if (structuralResidualsChanged) // Wenn sich die strukturellen Residuen geändert haben
            {
                Log.WriteLine("Databaser: Structural Residuals updated");

                try
                {
                    if (stringRes1 != "" || stringRes2 != "" || stringRes3 != "" || stringRes4 != "") // Mindestens ein Residuum muss einen Inhalt haben
                    {
                        if (myModel.MyNetwork.AtLeastOneNDAAOInDeadLock()) // Deadlock
                        {
                            if (myModel.MySettings.WriteDataBase)
                                myAdapterTable.InsertQuery(DateTime.Now, struktResiduals, "", myModel.MySettings.PlantName, "Deadlock"); // Datenbankeintrag generieren

                            Log.WriteLine("-------------------------------------");
                            Log.WriteLine("Databaser: Deadlock");
                            Log.WriteLine("Databaser: Res1: " + stringRes1);
                            Log.WriteLine("Databaser: Res2: " + stringRes2);
                            Log.WriteLine("Databaser: Res3: " + stringRes3);
                            Log.WriteLine("Databaser: Res4: " + stringRes4);
                            Log.WriteLine("Databaser: Structural Residuals: " + struktResiduals);
                            Log.WriteLine("-------------------------------------");
                            //Log.WriteLine("");
                        }
                        else // I/O Abweichung
                        {
                            if (myModel.MySettings.WriteDataBase)
                                myAdapterTable.InsertQuery(DateTime.Now, struktResiduals, "", myModel.MySettings.PlantName, "Differing I/Os"); // Datenbankeintrag generieren

                            Log.WriteLine("-------------------------------------");
                            Log.WriteLine("Databaser: Differing I/Os");
                            Log.WriteLine("Databaser: Res1: " + stringRes1);
                            Log.WriteLine("Databaser: Res2: " + stringRes2);
                            Log.WriteLine("Databaser: Res3: " + stringRes3);
                            Log.WriteLine("Databaser: Res4: " + stringRes4);
                            Log.WriteLine("Databaser: Structural Residuals: " + struktResiduals);
                            Log.WriteLine("-------------------------------------");
                            //Log.WriteLine("");
                        }
                    }

                }
                catch (Exception e)
                {
                    Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                }
            }


            if (timedResidualsChanged) // Wenn sich die zeitbasierten Residuen geändert haben
            {
                Log.WriteLine("Databaser: Timed Residuals changed...");

                try
                {
                    if (stringTimedRes1Early != "" || stringTimedRes2Early != "") // Früher Fall
                    {
                        if (myModel.MySettings.WriteDataBase)
                            myAdapterTable.InsertQuery(DateTime.Now, "", timedResiduals, myModel.MySettings.PlantName, "I/Os too early"); // Datenbankeintrag generieren

                        Log.WriteLine("-------------------------------------");
                        Log.WriteLine("Databaser: TimedRes1Early: " + stringTimedRes1Early);
                        Log.WriteLine("Databaser: TimedRes2Early: " + stringTimedRes2Early);
                        Log.WriteLine("Databaser: TimedRes1Late: " + stringTimedRes1Late);
                        Log.WriteLine("Databaser: TimedRes2Late: " + stringTimedRes2Late);
                        Log.WriteLine("Databaser: Timed Residuals: " + timedResiduals);
                        Log.WriteLine("-------------------------------------");
                        //Log.WriteLine("");
                    }

                    if (stringTimedRes1Late != "" || stringTimedRes2Late != "") // Später Fall
                    {
                        if (myModel.MySettings.WriteDataBase)
                            myAdapterTable.InsertQuery(DateTime.Now, "", timedResiduals, myModel.MySettings.PlantName, "I/Os too late"); // Datenbankeintrag generieren

                        Log.WriteLine("-------------------------------------");
                        Log.WriteLine("Databaser: TimedRes1Early: " + stringTimedRes1Early);
                        Log.WriteLine("Databaser: TimedRes2Early: " + stringTimedRes2Early);
                        Log.WriteLine("Databaser: TimedRes1Late: " + stringTimedRes1Late);
                        Log.WriteLine("Databaser: TimedRes2Late: " + stringTimedRes2Late);
                        Log.WriteLine("Databaser: Timed Residuals: " + timedResiduals);
                        Log.WriteLine("-------------------------------------");
                        //Log.WriteLine("");
                    }

                }
                catch (Exception e)
                {
                    Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                }
            }
        }


        /// <summary>
        /// Schreibt die Residuen-Ausgabe für den Eintrag in die Database um
        /// </summary>
        /// <param name="resElem"></param>
        /// <returns></returns>
        private string convertTimedDiagnosisEdges(string resElem)
        {
            if (resElem.Contains('_'))
            {
                string erg = resElem.Split('_')[0];

                if (resElem.Split('_')[1] == "1")
                    return erg + " is 1; ";

                if (resElem.Split('_')[1] == "0")
                    return erg + " is 0; ";
            }
            return "";
        }


        /// <summary>
        /// Schreibt die Residuen-Ausgabe für den Eintrag in die Database um
        /// changes E2.0_1 to E2.0 ist 1, soll 0;
        /// A1.0_1 --> (A1.0 ist 1, soll 0;) (with brackets)
        /// </summary>
        /// <param name="resElem"></param>
        /// <param name="freudenbElem"></param>
        /// <returns></returns>
        private string convertEdgesUnexpected(string resElem)
        {
            if (resElem.Contains('_'))
            {
                string erg = resElem.Split('_')[0];

                if (resElem.Split('_')[1] == "1")
                    return erg + " is 1, reference 0; ";

                if (resElem.Split('_')[1] == "0")
                    return erg + " ist 0, reference 1; ";
            }
            return "";
        }


        /// <summary>
        /// Schreibt die Residuen-Ausgabe für den Eintrag in die Database um
        /// changes E2.0_1 to E2.0 ist 0, soll 1
        /// </summary>
        /// <param name="resElem"></param>
        /// <param name="freudenbElem"></param>
        /// <returns></returns>
        private string convertDiagnosisEdgesMissed(string resElem)
        {
            if (resElem.Contains('_'))
            {
                string erg = resElem.Split('_')[0];

                if (resElem.Split('_')[1] == "1")
                    return erg + " is 0, reference 1; ";

                if (resElem.Split('_')[1] == "0")
                    return erg + " is 1, reference 0; ";
            }
            return "";
        }


        /// <summary>
        /// Writes the message in each row of the database
        /// Allgemeiner Eintrag wie: Keine Verindung oder neu initialisiert
        /// </summary>
        /// <param name="message"></param>
        public void GeneralMessageEntry(string message)
        {
            try
            {
                if (myModel.MySettings.WriteDataBase)
                    myAdapterTable.InsertQuery(DateTime.Now, "", "", myModel.MySettings.PlantName, message);
            }
            catch (Exception e)
            {
                Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace + " " + e.TargetSite + " " + e.InnerException);
            }
        }


        /// <summary>
        /// Verbindung zur Datenbank beenden
        /// </summary>
        public void CloseConnection()
        {
            if (myModel.MyDataBase != null)
                myAdapterTable.Connection.Close();
        }
    }
}
