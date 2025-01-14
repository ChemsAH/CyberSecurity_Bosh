using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Timers;

namespace FDITool
{
    /// <summary>
    /// Implements various methods for the process monitoring
    /// </summary>
    public class ProcessMonitoring
    {
        /// <summary>
        /// EVENT: Automatenstatus in view
        /// </summary>
        /// <param name="message"></param>
        public delegate void AutBoxHandler(string message);
        public event AutBoxHandler SendToAutTextBoxEvent; // Automata box in view


        /// <summary>
        /// EVENT: Angekommenes UDP Paket zur farbigen Kreis-Visualisierung in view
        /// </summary>
        public delegate void UdpReceiveHandler();
        public event UdpReceiveHandler UdpReceiveEvent;


        /// <summary>
        /// Programmmodell (MVC)
        /// </summary>
        private Model myModel;


        /// <summary>
        /// Timer wird alle x-Sekunden ausgelöst zum überprüfen des Verbindungsstatus
        /// </summary>
        private Timer connectionMonitoringTimer;


        /// <summary>
        /// Dient zum Erkennen von Unterbrechungen in der Datenübertragung, wird bei einem Empfangenen Datensatz auf true gesetzt
        /// </summary>
        private bool connectionAlive;

            
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="argModel"></param>
        public ProcessMonitoring(Model argModel)
        {
            myModel = argModel;

            connectionMonitoringTimer = new System.Timers.Timer(); // Verbindungsüberwachungsuhr initialisieren
            connectionMonitoringTimer.Enabled = false; // Noch nicht starten

            connectionMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(connectionMonitoringTimerElapsed); // Wenn die Zeit abgelaufen ist wird die Methode aufgerufen
        }


        /// <summary>
        /// THREAD
        /// Initializes and performs the basic diagnosis loop
        /// </summary>
        public void TimedProcessMonitoring()
        {
            // INIT Verbindungsüberwachung

            connectionAlive = false; // Initialisiere keine Verbindung
            connectionMonitoringTimer.Interval = myModel.MySettings.UpdateDataBaseDelay * 1000; // Intervallfestlegung des Timers in Sekunden
            connectionMonitoringTimer.Enabled = true; // Starte den Timer

            // INIT View Automatenbox

            SendToAutTextBoxEvent("");
            foreach (PartialAutomaton.TAAO aut in myModel.MyNetwork.MyAutomata)
                SendToAutTextBoxEvent(aut.Name + "\t" + " Actual state: " + "invalid"); // Initialisiere Automatenfeld in View

            // INIT Zustände Automaten

            foreach (PartialAutomaton.TAAO aut in myModel.MyNetwork.MyAutomata)
                aut.ActualState = null;

            // INIT Zustände Automatennetzwerk

            List<PartialAutomaton.State> currentNWSetting = new List<FDITool.PartialAutomaton.State>();
            List<PartialAutomaton.State> formerNWSetting = new List<FDITool.PartialAutomaton.State>();
            PartialAutomaton.NDAAOStateListComparer NDAAOStateListComparer = new FDITool.PartialAutomaton.NDAAOStateListComparer();

            // INIT UDP-Verbindung

            myModel.MyUdpClient = new System.Net.Sockets.UdpClient(myModel.MySettings.Port); // UDP Verbindung aufbauen
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, myModel.MySettings.Port);

            // INIT Globales Zeitmonitoring

            Stopwatch clock = new Stopwatch(); // Stoppuhr starten
            clock.Start();

            // INIT Blackboxaufnahme

            double blackboxDeltaT = 0;
            Log.InitBlackBoxFile();

            // INIT Threaddaten

            string recordedData = "";
            string oldData = "";
            Log.InitLogFile();

            myModel.DiagnosisThreadToken = true; // Starte den Thread und halte ihn am Leben

            // ******* ZYKLISCHE DIAGNOSEROUTIONE *******

            //double elapsedTime = 0;

            //int refreshTime = 500; // [ms] muss größer als das Abfrageintervall des Gateways sein!

            while (myModel.DiagnosisThreadToken)
            {
                // Datenaufnahme

                //try
                //{

                    recordedData = pickUpDataFromBuffer(ref remoteEndPoint); // receives UDP data

                    if (recordedData == "") // Keine Daten erhalten, eventuell deadlock?
                        continue;

                    //clock.Reset(); // Daten enthalten Zeitstempel und IOVektor, daher keine interne Zeitmessung erforderlich
                    //clock.Start();

                    //double elapsedTime = clock.ElapsedMilliseconds; // measures the elapsed time

                    string[] arrayOfVectors = recordedData.Split('\n'); // Wenn mehrere IO Vektoren in einem Paket kommen müssen diese aufgetrennt werden
                    List<string> listOfVectors = arrayOfVectors.ToList<string>(); // Liste mit mehreren IO Vektoren

                    List<string> listOfVectorsMasked = new List<string>(); // Manchmal werden leere Zeilen angehängt, diese müssen herausgefiltert werden

                    foreach (string s in listOfVectors) // filtern aller leeren Zeilen (Konvertierungsproblem)
                        if (s != "")
                            listOfVectorsMasked.Add(s);

                    //Console.WriteLine("number of IOvectorlist elements: " + listOfVectorsMasked.Count.ToString());

                    foreach (string s in listOfVectorsMasked) // für alle übertragenen IOVektoren
                    {
                        double elapsedTime = 0; // deltaT das von der SPS mitgeliert wird

                        string[] recordings = recordedData.Split('\t'); // auftrennen zwischen deltaT und IOVektor

                        if (recordings.Length < 2) // bei Fehlerhafter übertragung kann ein Teil des Vektors verloren gegangen sein
                        {
                            Log.WriteLine("Transmission error: I/O vector or deltaT missing!!!");
                            continue; // falls das der Fall ist wird es abgefangen, führt aber in der Regel bei der Diagnose zu einem Fehlalarm
                        }

                        elapsedTime = Convert.ToDouble(recordedData.Split('\t')[0]); // deltaT
                        recordedData = recordedData.Split('\t')[1]; // eigentlicher IO Vektor

                        if (recordedData.Length >= 208) // Die übertrgagenen Daten müssen mindestens 208 bits lang sein (26 Wörter)
                            recordedData = recordedData.Substring(0, 208); // Abtrennen von überflüssigen Bits am Ende des Vektors
                        else
                        {
                            recordedData = oldData; // Teil des IOVektors ging wegen übertragungsfehler verloren
                            Log.WriteLine("Transmission error: I/O vector too short!!!");
                        }

                        //clock.Reset();
                        //clock.Start();

                        //Console.WriteLine(elapsedTime.ToString() + '\t' + recordedData); // Ausgabe des aktuell empfangenen Telegramms (ein Telegramm je SPS-Zyklus)

                        if (myModel.MyUdpClient.Client.Available > 0)
                            Log.WriteLine("Buffersize: " + myModel.MyUdpClient.Client.Available);

                        //if ((elapsedTime > 40) | (elapsedTime < 20))
                        //    Log.WriteLine("Abtastzeitfenster verlassen: " + elapsedTime.ToString());

                        // VIEW LOG Datenausgabe

                        UdpReceiveEvent(); // Event für Viewkreis werfen

                        if (!oldEquNewRelevantData(recordedData, oldData)) // If a new IO vector is received based on the relevant data
                        {
                            Log.WriteLine("");
                            Log.WriteLine("Buffersize before receive: " + myModel.MyUdpClient.Client.Available);
                            Log.WriteLine(elapsedTime.ToString() + " " + recordedData); // Logdatei schreiben
                        }

                        // Automatenzustände Aktualisierung

                        myModel.MyNetwork.ProcessTimedAutomataNetwork(recordedData, elapsedTime); // Verarbeite das automata network

                        currentNWSetting = DetermineCurrentNWSetting(myModel.MyNetwork.MyAutomata); // returns the list of current states for all automata

                        if (!NDAAOStateListComparer.Equals(currentNWSetting, formerNWSetting)) // if NetworkSetting has changed
                        {
                            bool stateCombOfPossInitialStates = true;

                            foreach (PartialAutomaton.TAAO aut in myModel.MyNetwork.MyAutomata) // check if states of all automata are init states or not
                            {
                                if ((aut.ActualState == null) || (!aut.ActualState.IsIniState)) // wenn mindestens einer kein init zustand ist
                                    stateCombOfPossInitialStates = false;
                            }

                            if (stateCombOfPossInitialStates) // If all automata are in intial state, the network is initialized
                            {
                                Log.WriteLine("New initialized");
                                myModel.MyDataBase.GeneralMessageEntry("New initialized");
                            }
                        }

                        // VIEW Automatenboxausgabe

                        if (!oldEquNewRelevantData(recordedData, oldData)) // if a new IO vector is received
                        {
                            SendToAutTextBoxEvent("");

                            foreach (PartialAutomaton.TAAO aut in myModel.MyNetwork.MyAutomata)
                            {
                                if (aut.ActualState != null)
                                    SendToAutTextBoxEvent(aut.Name + "\t" + " actual state: " + aut.ActualState.Name);
                                else
                                    SendToAutTextBoxEvent(aut.Name + "\t" + " actual state: " + "invalid");
                            }
                        }

                        // LOG Blackboxaufnahme

                        if (Log.BlackBoxWriterToken) /// Schreiben einer blackboxdatei mit OnlineWerten der letzten x Tage, alle Bits sind relevant
                        {
                            blackboxDeltaT += elapsedTime;

                            if (recordedData != oldData)
                            {
                                Log.BlackBoxWriter(blackboxDeltaT.ToString(), oldData);
                                blackboxDeltaT = 0;
                            }
                        }

                        // DATENBANK Aktualisierung

                        myModel.MyDataBase.UpdateDatabase(elapsedTime);

                        // Neuinitialisierung

                        formerNWSetting = currentNWSetting;
                        oldData = recordedData;

                    }
                /*} // try
                catch (Exception e)
                {
                    Log.ExceptionWriter(e.Data.ToString());
                    Log.ExceptionWriter(e.HelpLink.ToString());
                    Log.ExceptionWriter(e.InnerException.ToString());
                    Log.ExceptionWriter(e.Message.ToString());
                    Log.ExceptionWriter(e.Source.ToString());
                    Log.ExceptionWriter(e.StackTrace.ToString());
                    Log.ExceptionWriter(e.TargetSite.ToString());
                } */// catch
            } // while

            // Objektbereinigung

            myModel.MyUdpClient.Close();
            
        }


        /// <summary>
        /// Receives the UDP Data and converts it into a string representing the I/O vector
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <returns></returns>
        public string pickUpDataFromBuffer(ref IPEndPoint remoteEndPoint)
        {
            string recordedData = ""; // Daten kommen hier direkt als string an und müssen nicht weiter konvertiert werden

            try
            {
                //if (myModel.MyUdpClient.Client.Available == 0) 4.4.12
                    //Log.WriteLine("Puffergröße vor Empfang: " + myModel.MyUdpClient.Client.Available); 4.4.12

                byte[] erg = myModel.MyUdpClient.Receive(ref remoteEndPoint);

                connectionAlive = true;

                //byte[] converg = ConvertIntBytesToBinaryBytes(erg);

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                recordedData = enc.GetString(erg);

                //foreach (byte elem in converg)
                    //recordedData += elem.ToString();
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message + " " + e.Source);
            }

            return recordedData;
        }


        /// <summary>
        /// Returns a list with the current active states
        /// </summary>
        /// <param name="automata"></param>
        /// <returns></returns>
        public List<PartialAutomaton.State> DetermineCurrentNWSetting(List<PartialAutomaton.TAAO> automata)
        {
            List<PartialAutomaton.State> NWSetting = (from aut in automata select aut.ActualState).ToList();
            return NWSetting;
        }


        /// <summary>
        /// Method to decompress bytes signal information from IO server to 1001010100 presentation
        /// </summary>
        /// <param name="compressedData">The image of the server (compressed, bitwise information)</param>
        /// <returns>The decompressed image of the server (bytewise)</returns>
        private byte[] ConvertIntBytesToBinaryBytes(byte[] compressedData)
        {
            byte[] result = new byte[compressedData.Length * 8];

            for (int i = 0; i < compressedData.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    result[i * 8 + j] = (byte)((compressedData[i] >> j) & 0x01);
                }

            }

            return result;
        }


        /// <summary>
        /// Compares two I/O vectors using only the relevant bits
        /// </summary>
        /// <returns></returns>
        private bool oldEquNewRelevantData(string recordedData, string oldData)
        {
            if (recordedData.Length != oldData.Length)
                return false;

            if (myModel.consideredBits == null)
            {
                if (recordedData == oldData)
                    return true;
                else
                    return false;
            }

            if (myModel.consideredBits.Count == 0)
            {
                if (recordedData == oldData)
                    return true;
                else
                    return false;
            }

            for (int i = 0; i < recordedData.Length; i++)
            {
                if (myModel.consideredBits.Contains(i))
                {
                    if (recordedData[i] != oldData[i])
                        return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Nachdem der Timer zur Verbindungsüberwachung abgelaufen ist wird diese Methode aufgerufen.
        /// Wenn das Token nicht vom Diagnose thread gesetzt wurde wird ein Datenbankeintrag erzeugt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionMonitoringTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if ((!connectionAlive) & (myModel.DiagnosisThreadToken))
            {
                try
                {
                    //Log.WriteLine("Keine Verbindung");
                    //myModel.MyDataBase.GeneralMessageEntry("Keine Verbindung");
                }
                catch (Exception ex)
                {
                    Log.ExceptionWriter(ex.Message);
                }
            }
            connectionAlive = false;
        }
    }
}
