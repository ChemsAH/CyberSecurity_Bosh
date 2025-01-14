using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class TAAO
    {
        /// <summary>
        /// States in the automaton
        /// </summary>
        public List<State> MyStates { get; set; }


        /// <summary>
        /// Contains possible ini states for new system cycles
        /// </summary>
        public List<State> PossibleIniStates { get; set; }


        /// <summary>
        /// Contains possible last states for new system cycles
        /// </summary>
        ///
        public List<State> PossibleLastStates { get; set; }


        /// <summary>
        /// List with indices of possible states during the reinitialization procedure
        /// </summary>
        List<int> iniCandidateStates;


        /// <summary>
        /// Current automaton state. Null if none
        /// </summary>
        public State ActualState { get; set; }


        /// <summary>
        /// Contains the former actual state (in case of fault detection, ActualState is null)
        /// </summary>
        public State FormerActualState { get; set; }


        /// <summary>
        /// Parameter used for identification
        /// </summary>
        public int ParamterK { get; set; }


        /// <summary>
        /// Name of the partial automaton
        /// </summary>
        public string Name { get; set; }


        string formerIOVector = "";


        /// <summary>
        /// stores the former I/O vector during intialization
        /// </summary>
        string iniFormerIOVector = "";


        /// <summary>
        /// Counts the vectors analyzed for the reinitialization --> must be lower equal k
        /// </summary>
        int iniCounter = 0;


        [System.Xml.Serialization.XmlIgnore]
        /// <summary>
        /// Automatenuhr
        /// </summary>
        public double? myClock;


        /// <summary>
        /// Obere Grenze alle Intervaller aller Transitionen von einem aktuellen Zustand aus
        /// </summary>
        private double maxDurationOfAllTransitions;


        /// <summary>
        /// Strukturelle Residuen
        /// </summary>
        public List<SingleEvent> Res1 { get; set; }
        public List<SingleEvent> Res2 { get; set; }
        public List<SingleEvent> Res3 { get; set; }
        public List<SingleEvent> Res4 { get; set; }


        /// <summary>
        /// Zeitbasierte Residuen
        /// </summary>
        public List<SingleEvent> TimedRes1Early { get; set; }
        public List<SingleEvent> TimedRes2Early { get; set; }
        public List<SingleEvent> TimedRes1Late { get; set; }
        public List<SingleEvent> TimedRes2Late { get; set; }


        /// <summary>
        /// Different automata situations
        /// </summary>
        public enum FaultSituation { FaultFree, FaultDetected }


        [System.Xml.Serialization.XmlIgnore]
        /// <summary>
        /// Contains information of the actual automaton situation
        /// </summary>
        public FaultSituation MyFaultSituation { get; set; }
        
        
        /// <summary>
        /// Different possible fault types
        /// </summary>        
        public enum FaultType { DeadLockFault, IOFault, TimeFault, InitFault, NoFault }


        [System.Xml.Serialization.XmlIgnore]
        /// <summary>
        /// Contains information of the detected fault 
        /// </summary>
        public FaultType MyFaultType { get; set; }


        bool firstIsolationRun = true;

        bool recentlyInitialized = true;

        public double deadlockClock;

        public TAAO(string name, int k)
        {
            Name = name;
            ParamterK = k;
            MyStates = new List<State>();
            PossibleIniStates = new List<State>();
            PossibleLastStates = new List<State>();
        }

        public TAAO() { }


        /// <summary>
        /// FEHLERDETEKTION
        /// 
        /// Processes the automaton using the given vector. The new current state is determined.
        /// If a fault is detected the candidate reduction is NOT performed. 
        ///
        /// Benötigt das deltaT JEDES empfangen events. Notwendig zur Detektion von Deadlocks.
        /// Zustand wird mit Algorithmus aus Diss Roth detektiert. Eindeutiger Zustand kann nach k Schritten spätestens gefunden werden.
        /// Zeitbasierte Fehlerdetektion im Falle neuer Events.
        /// Wenn kein neues Event angekommen ist kann ein Deadlock detektiert werden.
        /// </summary>
        /// <param name="newVector"></param>
        /// <param name="deltaT">elapsed time</param>
        public void TimedNDAAOFaultDetection(string newVector, double deltaT)
        {
            if (!myClock.HasValue)
                myClock = 0;

            myClock += deltaT;

            //
            // AUTOMATON INITIALIZATION
            //

            if (ActualState == null) // if no actual state exists, the automaton is reinitialized
            {
                MyFaultSituation = FaultSituation.FaultDetected;
                MyFaultType = FaultType.InitFault;

                iniCandidateStates = initialStateEstimation(newVector); // Diss Roth: Evaluator Algorithm

                if (iniCandidateStates.Count == 1) // if actual state found
                {
                    FormerActualState = ActualState;
                    ActualState = MyStates[iniCandidateStates[0]];

                    ResetResiduals();
                    myClock = null;

                    MyFaultSituation = FaultSituation.FaultFree;
                    MyFaultType = FaultType.NoFault;

                    recentlyInitialized = true;

                    Log.WriteLine("Detector: " + Name + " new initialized at state " + ActualState.Name);
                }

            } // if (ActualState == null)

            //
            // FAULT DETECTION
            //

            else if (ActualState != null) // if there exists an actual state
            {
                // NEW EVENT RECEIVED: STRUCTURAL AND TIMED FAULT DETECTION

                if (!ActualState.CompareIOVector(newVector)) // if newVector differs from the actual lambda
                {

                    foreach (Transition transition in ActualState.MyTransitions) // Überprüfe jeden Folgezustand ob eine Zustand mit gleichem Lambda existiert
                    {
                        // STRUCTURAL FAULT DETECTION

                        MyFaultSituation = FaultSituation.FaultDetected;
                        MyFaultType = FaultType.IOFault;

                        if (MyStates[transition.TargetIndex].CompareIOVector(newVector)) // Falls ein gültiger struktureller Folgezustand existiert
                        {
                            // TIMED FAULT DETECTION

                            Log.WriteLine("Detector: " + Name + " Transition found, minimum time bound: " + transition.MinDuration.ToString() + " clock: " + myClock.Value.ToString() + " maximum time bound: " + transition.MaxDuration.ToString());

                            if ((myClock.Value <= transition.MaxDuration && myClock.Value >= transition.MinDuration) || ActualState.IsIniState || recentlyInitialized) // Überprüfe die Zeitbedingung, außer bei Initialzuständen
                            {
                                if (ActualState.IsIniState)
                                    Log.WriteLine("Detector: " + Name + " in initial state");

                                if (recentlyInitialized)
                                    Log.WriteLine("Detector: " + Name + " initialized recently");

                                FormerActualState = ActualState;
                                ActualState = MyStates[transition.TargetIndex];

                                MyFaultSituation = FaultSituation.FaultFree;
                                MyFaultType = FaultType.NoFault;                  

                                Log.WriteLine("Detector: " + Name + " successful switching from state " + FormerActualState.Name + " to state " + ActualState.Name);

                                recentlyInitialized = false;

                                ResetResiduals();
                                myClock = null;
                                break;
                            }
                            else // Zeitfehler erkannt, allerdings wird gültiger Folgezustand trotzdem übernommen
                            {
                                maxDurationOfAllTransitions = 0;

                                foreach (Transition transitionMaxT in ActualState.MyTransitions) // determining maximum upper time bound of all possbile transitions
                                {
                                    if ((transitionMaxT.MaxDuration > maxDurationOfAllTransitions) & (transitionMaxT.MaxDuration != Double.PositiveInfinity)) // Alle Transitionen mit unendlich werden für die Maxgrenze ignoriert
                                        maxDurationOfAllTransitions = transitionMaxT.MaxDuration;
                                }

                                MyFaultSituation = FaultSituation.FaultDetected;
                                MyFaultType = FaultType.TimeFault;

                                FormerActualState = ActualState;
                                ActualState = MyStates[transition.TargetIndex];

                                Log.WriteLine("Detector: " + Name + " time fault detected, transition from state " + FormerActualState.Name + " to state " + ActualState.Name + " successful anyway");
                                Log.WriteLine("Detector: " + Name + " maximum time bound of all transitions: " + maxDurationOfAllTransitions.ToString());
                                break;
                            }

                        } // if (MyStates[transition.TargetIndex].CompareIOVector(newVector))

                    } // foreach (Transition transition in ActualState.MyTransitions)

                    TreatLastState(newVector); // if the actual state is a last state, check if the vector belongs to one of the ini states or one of their successors (wird auch ausgeführt wenn Schaltvorgang erfolgreich war)
                
                } // if (!ActualState.CompareIOVector(newVector))

                // DEADLOCK DETECTION
                //
                // NO NEW/RELEVANT IO VECTOR RECEIVED: 
                //
                // ACHTUNG: Es werden nur Transitionen berücksichtigt deren obere Grenze kleiner als +unendlich ist. Grund: Da diese 0, +unendlich Transitionen nur sehr selten genommen werden,
                //      kann davon ausgegangen werden, das diese im laufenden Betrieb auch nur sehr selten vorkommen. Um trotzdem deadlocks hier erkennen zu können, muss die obere Grenze der
                //      messbaren Transitionen als maximal mögliche angenommen werden.

                else if (ActualState.CompareIOVector(newVector)) // no new IO vector
                {
                    MyFaultSituation = FaultSituation.FaultFree;
                    MyFaultType = FaultType.NoFault;

                    if ((ActualState.IsLastState) & (!ActualState.IsIniState)) // Nur wenn der Last state nicht automatisch auch der init state ist (tritt bei zyklischen Automaten auf)
                        TreatLastState(newVector); // last state Behandlung
                    else
                    {
                        if (!ActualState.IsIniState) // Falls Zustand kein Initzustand ist
                        {
                            bool onlyInfty = true; // Falls nur Transitionen mit unendlichen Grenzen existieren, dann true;
                            maxDurationOfAllTransitions = 0;

                            foreach (Transition transition in ActualState.MyTransitions) // determining maximum upper time bound of all possbile transitions
                            {
                                if ((transition.MaxDuration > maxDurationOfAllTransitions) & (transition.MaxDuration != Double.PositiveInfinity)) // Alle Transitionen mit unendlich werden für die Maxgrenze ignoriert
                                {
                                    onlyInfty = false; // Es existiert mindestens eine Transition, wo die Zeitgrenze ungleich unendlich und größer als 0 ist
                                    maxDurationOfAllTransitions = transition.MaxDuration;
                                }
                            }

                            if (onlyInfty) // Wenn keine Transition mit endlichen Zeitgrenzen exisitert, dann deaktiviere die Deadlocküberwachung
                                maxDurationOfAllTransitions = Double.PositiveInfinity;

                            if (myClock.Value > maxDurationOfAllTransitions) // falls die aktuelle Zeit größer als die maximale Zeitschranke ist
                            {
                                MyFaultSituation = FaultSituation.FaultDetected; // Deadlock erkannt
                                MyFaultType = FaultType.DeadLockFault;

                                Log.WriteLine("");
                                Log.WriteLine("Detector: " + Name + " maximum time bound " + maxDurationOfAllTransitions.ToString() + " at state " + ActualState.Name + " violated: deadlock!");
                                Log.WriteLine("Detector: " + Name + " clock: " + myClock.Value.ToString());
                            }
                        }
                    }

                } // else if (ActualState.CompareIOVector(newVector))

            } // else if (ActualState != null)

            /* 4.4.12
            if (ActualState != null)
                Log.WriteLine("Detector: Automat: " + Name + " Zustand: " + this.ActualState.Name.ToString() + " Uhr: " + myClock.ToString() + " " + MyFaultSituation.ToString() + " " + MyFaultType.ToString());
            else
                Log.WriteLine("Detector: Automat: " + Name + " Zustand: ungültig" + " Uhr: " + myClock.ToString() + " " + MyFaultSituation.ToString() + " " + MyFaultType.ToString());
            */

        } // TimedNDAAOFaultDetection


        /// <summary>
        /// FAULT ISOLATION
        /// 
        /// Sobald der Automat einen Fehler erkannt und spezifiziert hat werden die Residuen auf Basis des Fehlers berechnet
        /// 
        /// </summary>
        /// <param name="newVector"></param>
        /// <param name="deltaT"></param>
        public void TimedNDAAOFaultIsolation(string newVector, double deltaT)
        {
            if (MyFaultSituation == FaultSituation.FaultDetected) // if faultDetection is true, a fault was detected and the residuals must be calculated
            {
                if (firstIsolationRun)
                {
                    if (ActualState != null)
                        Log.WriteLine("Isolator: " + Name + " processing " + MyFaultType.ToString() + " from state " + ActualState.Name);
                    else
                        Log.WriteLine("Isolator: " + Name + " processed " + MyFaultType.ToString());
                }

                if (MyFaultType == FaultType.DeadLockFault) // if maximum state duration exceeded get the missed edges
                {
                    if (firstIsolationRun)
                        Log.WriteLine("Isolator: " + Name + " calculating Res4'...");

                    GetRes4Dash(ActualState);
                }
                else if (MyFaultType == FaultType.IOFault) // calculate IO residuals
                {
                    if (firstIsolationRun)
                        Log.WriteLine("Isolator: " + Name + " calculating Res1-4...");

                    GetRes1(ActualState, newVector);
                    GetRes2(ActualState, newVector);
                    GetRes3(ActualState, newVector);
                    GetRes4(ActualState, newVector);
                    ActualState = null;
                    myClock = null;
                }
                else if (MyFaultType == FaultType.TimeFault) // calculate timed residuals 
                {
                    if (firstIsolationRun)
                        Log.WriteLine("Isolator: " + Name + " calculating TimedRes1-4...");

                    GetTimedRes1Early(FormerActualState, newVector, myClock.Value);
                    GetTimedRes2Early(FormerActualState, newVector, myClock.Value);
                    GetTimedRes1Late(FormerActualState, newVector, myClock.Value);
                    GetTimedRes2Late(FormerActualState, newVector, myClock.Value);
                    myClock = null;
                }

                firstIsolationRun = false;

            } //if (MyFaultSituation == FaultSituation.FaultDetected)    

            else
                firstIsolationRun = true;
        }


        /// <summary>
        /// Returns the indices of states with the given output. 
        /// States must be successor states of the current estimation given in this.iniCandidateStates.
        /// If this.iniCandidateStates is empty, the whole state list is analyzed.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private List<int> initialStateEstimation(string output)
        {   
            if (iniCounter >= ParamterK)
            {
                iniCandidateStates.Clear();
                iniCounter = 0;
            }

            if (iniFormerIOVector != "")
            {
                if (!CompareIOVectorWithReference(formerIOVector, output, MyStates[0].Lambda))
                    iniCounter += 1;              
            }
            else
            {                
                iniCounter += 1;
            }
             
            iniFormerIOVector = output;

            List<int> resultingIniCandidateStates = new List<int>();

            if (iniCandidateStates == null) 
                iniCandidateStates = new List<int>();

            if (iniCandidateStates.Count == 0) // no initial state candidates in the list
            {
                for (int i = 0; i < MyStates.Count; i++) // check each state
                {
                    if (MyStates[i].CompareIOVector(output)) // if state has equal output to observed output
                    {
                        if (!resultingIniCandidateStates.Contains(i)) 
                            resultingIniCandidateStates.Add(i); // add do resulting state list
                    }
                }
            }

            else // if there are initial state candidates in the list
            {
                foreach (int index in iniCandidateStates)
                {
                    if (MyStates[index].CompareIOVector(output))  // if the considered state in the iniCandidateStates has the same as the observed output, it remains in the list
                    {
                        if (!resultingIniCandidateStates.Contains(index)) 
                            resultingIniCandidateStates.Add(index);
                    }

                    else // if the state has not the same as the observed output, check if any following state has
                    {
                        foreach (Transition transition in MyStates[index].MyTransitions)
                        {
                            if (MyStates[transition.TargetIndex].CompareIOVector(output))
                            {
                                if (!resultingIniCandidateStates.Contains(transition.TargetIndex)) 
                                    resultingIniCandidateStates.Add(transition.TargetIndex);
                            }
                        }
                    }
                }
            }

            iniCandidateStates.Clear();
            iniCandidateStates = resultingIniCandidateStates;

            if (resultingIniCandidateStates.Count == 1)
            {
                iniFormerIOVector = "";
                iniCounter = 0;
            }
            
            return resultingIniCandidateStates;
        }


        /// <summary>
        /// Compares two vectors vector1 and vector2. Each bits with a '-'-symbol in referencevector is ignored
        /// </summary>
        /// <param name="vector1">first vector to be compared</param>
        /// <param name="vector2">second vector to be compared</param>
        /// <param name="referencevector">vector wit '-' symbols at places to be ignored</param>
        /// <returns>true if vector1 and vector2 are equal (by ignoring the '-' bits of lambda</returns>
        private bool CompareIOVectorWithReference(string vector1, string vector2, string referencevector)
        {
            if (vector1.Length != vector2.Length)
                return false;
            if (vector1.Length != referencevector.Length)
                return false;
            for (int i = 0; i < referencevector.Length; i++)
            {
                if (!(referencevector[i] == '-'))
                {
                    if (vector1[i] != vector2[i])
                        return false;
                }
            }
            return true;
        }


        /// <summary>
        /// if the actual state is a last state, check if the new vector belongs to one of the ini states or one of their successors
        /// if yes, determine the new actual state and do not raise any alert
        /// 
        /// GRUND: Bei nicht ringförmigen Modellen kann ein Initzustand und ein Endzustand den gleichen IO Vektor haben, allerdings keine
        ///     Transition die die beiden Zustände miteinander verbindet. Das passiert wenn die Identifikationsdaten jeweils immer nur
        ///     ein Zyklus enthalten und nicht mehrere Zyklen den Kreis schließen können. En Zustand wird nur dann vermischt, wenn auch der
        ///     Folgezustand beider Zustände ubereinstimmt und das ist offensichtlich bei last states nie der fall.
        /// </summary>
        /// <param name="newVector"></param>
        private void TreatLastState(string newVector)
        {
            if ((ActualState.IsLastState) & (!ActualState.IsIniState)) // Voraussetzung: Aktueller Zustand ist ein last state aber kein init state (kann beides sein bei zyklischen Automaten)
            {
                Log.WriteLine("Detector (last state) " + Name + " processing...");

                bool actualStateChanged = false;

                foreach (State iniState in PossibleIniStates)
                {
                    if (iniState.CompareIOVector(newVector)) // Wenn der IO Vektor gleich einem Initzustand 
                    {
                        FormerActualState = ActualState;
                        ActualState = iniState;             // Übernahme des Zustandes
                        MyFaultSituation = FaultSituation.FaultFree;
                        MyFaultType = FaultType.NoFault;
                        ResetResiduals();
                        myClock = null;
                        Log.WriteLine("Detector (last state) " + Name + " initial state " + ActualState.Name + " set");
                        break;
                    }
                    else // Wenn der IO Vektor nicht gleich einem InitZustand ist überprüfe alle Folgezustände der Initzustände
                    {
                        foreach (Transition transition in iniState.MyTransitions)
                        {
                            if (MyStates[transition.TargetIndex].CompareIOVector(newVector))
                            {
                                FormerActualState = ActualState;
                                ActualState = MyStates[transition.TargetIndex];
                                MyFaultSituation = FaultSituation.FaultFree;
                                MyFaultType = FaultType.NoFault;
                                ResetResiduals();
                                myClock = null;
                                actualStateChanged = true;
                                Log.WriteLine("Detector (last state) " + Name + " following state " + ActualState.Name + " of initial state " + FormerActualState.Name + " set");
                                break;
                            }
                        }

                        if (actualStateChanged)
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Copies a residual
        /// </summary>
        /// <param name="list"></param>
        /// <returns>a copy of list</returns>
        private List<SingleEvent> copyEdgeList(List<SingleEvent> list)
        {
            List<SingleEvent> copy = new List<SingleEvent>();
            foreach (SingleEvent myEdge in list)
            {
                copy.Add(new SingleEvent(myEdge.MyIndex, myEdge.IsRising, myEdge.Name));
            }
            return copy;
        }


        /// <summary>
        /// Assigns empty lists to each residual
        /// </summary>
        public void ResetResiduals()
        {
            Res1 = new List<SingleEvent>();
            Res2 = new List<SingleEvent>();
            Res3 = new List<SingleEvent>();
            Res4 = new List<SingleEvent>();

            TimedRes1Early = new List<SingleEvent>();
            TimedRes2Early = new List<SingleEvent>();
            TimedRes1Late = new List<SingleEvent>();
            TimedRes2Late = new List<SingleEvent>();
        }


        /// <summary>
        /// Calculates the residual set according to DCDS09-draft
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <returns></returns>
        public List<SingleEvent> GetRes1(State actualState, string newVector)
        {
            if (actualState == null)
                return new List<SingleEvent>();
            
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();
            
            List<SingleEvent> result = new List<SingleEvent>();
            result = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> union = UnionOfEventsBetweenActualAndFollStates(actualState);
            IEnumerable<SingleEvent> test = result.Except(union, myComparer);
            result = (result.Except(union, myComparer)).ToList();
            Res1 = result;            
            return result;
        }

        /// <summary>
        /// Calculates the residual set according to DCDS09-draft
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <returns></returns>        
        public List<SingleEvent> GetRes2(State actualState, string newVector)
        {
            if (actualState == null)
                return new List<SingleEvent>();
            
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();            

            List<SingleEvent> result = new List<SingleEvent>();
            result = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> intersection = IntersectionOfEventsBetweenActualAndFollStates(actualState);
            result = (result.Except(intersection, myComparer)).ToList();
            Res2 = result;
            return result;
        }

        /// <summary>
        /// Calculates the residual set according to DCDS09-draft
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <returns></returns>
        public List<SingleEvent> GetRes3(State actualState, string newVector)
        {
            if (actualState == null)
                return new List<SingleEvent>();
            
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();            

            List<SingleEvent> intersection = IntersectionOfEventsBetweenActualAndFollStates(actualState);
            List<SingleEvent> obs = getEvolutionSet(actualState.Lambda, newVector);
            intersection = (intersection.Except(obs, myComparer)).ToList();
            Res3 = intersection;
            return intersection;
        }

        /// <summary>
        /// Calculates the residual set according to DCDS09-draft
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <returns></returns>
        public List<SingleEvent> GetRes4(State actualState, string newVector)
        {
            if (actualState == null)
                return new List<SingleEvent>();
            
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            List<SingleEvent> union = UnionOfEventsBetweenActualAndFollStates(actualState);
            List<SingleEvent> obs = getEvolutionSet(actualState.Lambda, newVector);
            union = (union.Except(obs, myComparer)).ToList();
            Res4 = union;
            return union;
        }


        /// <summary>
        /// Calculates the union of the iteration over Es(lambda(x),lambda(x')) for each following transition
        /// </summary>
        /// <param name="actualState">actual state of the automaton</param>
        public List<SingleEvent> UnionOfEventsBetweenActualAndFollStates(State actualState)
        {
            List<List<SingleEvent>> EsList = new List<List<SingleEvent>>();

            EdgeEqualityComparer myEdgeComparer = new EdgeEqualityComparer();

            foreach (Transition transition in actualState.MyTransitions)
            {
                EsList.Add(getEvolutionSet(MyStates[transition.SourceIndex].Lambda, MyStates[transition.TargetIndex].Lambda));
            }

            List<SingleEvent> result = new List<SingleEvent>();

            if (EsList.Count >= 1)
                result = EsList[0];
            else
                return result;

            for (int i = 0; i < EsList.Count - 1; i++)
            {
                result = (result.Union(EsList[i + 1], myEdgeComparer)).ToList();
            }

            return result;
        }


        /// <summary>
        /// Calculates the intersection of the iteration over Es(l(x),l(x')) 
        /// </summary>
        /// <param name="actualState">actual state of the automaton</param>
        public List<SingleEvent> IntersectionOfEventsBetweenActualAndFollStates(State actualState)
        {
            List<List<SingleEvent>> EsList = new List<List<SingleEvent>>();

            EdgeEqualityComparer myEdgeComparer = new EdgeEqualityComparer();

            foreach (Transition transition in actualState.MyTransitions)
            {

                EsList.Add(getEvolutionSet(MyStates[transition.SourceIndex].Lambda, MyStates[transition.TargetIndex].Lambda));
            }

            List<SingleEvent> result = new List<SingleEvent>();

            if (EsList.Count >= 1)
                result = EsList[0];
            else
                return result;

            for (int i = 0; i < EsList.Count - 1; i++)
            {
                result = (result.Intersect(EsList[i + 1], myEdgeComparer)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Calculate the evolution set
        /// </summary>
        /// <param name="vector1">first considered vector</param>
        /// <param name="vector2">second considered vector</param>
        /// <returns>A list of edges that occur when the two I/O vectors are compared</returns>
        public List<SingleEvent> getEvolutionSet(string vector1, string vector2)
        {
            List<SingleEvent> ES = new List<SingleEvent>();

            if (vector1.Length != vector2.Length)
                return ES;

            for (int i = 0; i < vector1.Length; i++)
            {
                if (vector1[i] != vector2[i])
                {
                    if ((vector1[i] == '0') && (vector2[i] == '1')) //rising edge
                    {
                        SingleEvent newEdge = new SingleEvent(i, true, "unknown");
                        ES.Add(newEdge);
                    }
                    if ((vector1[i] == '1') && (vector2[i] == '0')) //falling edge
                    {
                        SingleEvent newEdge = new SingleEvent(i, false, "unknown");
                        ES.Add(newEdge);
                    }
                }
            }
            return ES;
        }


        /// <summary>
        /// Calculates Res3 and Res4 with an empty evolution set.
        /// Can be used to get the missed edges when the maximum state duration is exceeded.
        /// Erases residual 1 and 2.
        /// </summary>
        /// <param name="actualState"></param>
        /// <returns>returns true if Res3 of Res4 changed their value</returns>
        public bool GetRes4Dash(State actualState)
        {
            string newVector = actualState.Lambda;

            Res1 = new List<SingleEvent>(); // erase residual 1,2,3
            Res2 = new List<SingleEvent>();
            Res3 = new List<SingleEvent>();

            List<SingleEvent> oldRes4 = copyEdgeList(Res4);

            GetRes4(actualState, newVector);

            if (Res4.Count != oldRes4.Count) // check if res3 has changed. 
                return false;

            EdgeEqualityComparer comparer = new EdgeEqualityComparer();

            foreach (SingleEvent myEdge in Res4)
            {
                if (!oldRes4.Contains(myEdge, comparer))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Timed Residual according to DCDS11
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <param name="elapsedTime"></param>
        private void GetTimedRes1Early(State actualState, string newVector, double elapsedTime)
        {
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            List<SingleEvent> observedEvolutionSet = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> intersectionOfEvolutionSets = intersectionOfEventsBetweenActualAndFollStatesWithTimingBounds(actualState, "upper", elapsedTime);
            List<SingleEvent> resultingEvolutionSet = (observedEvolutionSet.Intersect(intersectionOfEvolutionSets, myComparer)).ToList();

            TimedRes1Early = resultingEvolutionSet;
        }


        /// <summary>
        /// Timed Residual according to DCDS11
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <param name="elapsedTime"></param>
        public void GetTimedRes2Early(State actualState, string newVector, double elapsedTime)
        {
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            List<SingleEvent> observedEvolutionSet = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> unionOfEvolutionSets = unionOfEventsBetweenActualAndFollStatesWithTimingBounds(actualState, "upper", elapsedTime);
            List<SingleEvent> resultingEvolutionSet = (observedEvolutionSet.Intersect(unionOfEvolutionSets, myComparer)).ToList();

            TimedRes2Early = resultingEvolutionSet;
        }


        /// <summary>
        /// Timed Residual according to DCDS11
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <param name="elapsedTime"></param>
        public void GetTimedRes1Late(State actualState, string newVector, double elapsedTime)
        {
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            List<SingleEvent> observedEvolutionSet = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> intersectionOfEvolutionSets = intersectionOfEventsBetweenActualAndFollStatesWithTimingBounds(actualState, "lower", elapsedTime);
            List<SingleEvent> resultingEvolutionSet = (observedEvolutionSet.Intersect(intersectionOfEvolutionSets, myComparer)).ToList();

            TimedRes1Late = resultingEvolutionSet;
        }


        /// <summary>
        /// Timed Residual according to DCDS11
        /// </summary>
        /// <param name="actualState"></param>
        /// <param name="newVector"></param>
        /// <param name="elapsedTime"></param>
        public void GetTimedRes2Late(State actualState, string newVector, double elapsedTime)
        {
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            List<SingleEvent> observedEvolutionSet = getEvolutionSet(actualState.Lambda, newVector);
            List<SingleEvent> unionOfEvolutionSets = unionOfEventsBetweenActualAndFollStatesWithTimingBounds(actualState, "lower", elapsedTime);
            List<SingleEvent> resultingEvolutionSet = (observedEvolutionSet.Intersect(unionOfEvolutionSets, myComparer)).ToList();

            TimedRes2Late = resultingEvolutionSet;
        }


        /// <summary>
        /// Calculates the union of Iteration over all observed following states
        /// </summary>
        /// <param name="actualState">actual state of the automaton</param>
        private List<SingleEvent> unionOfEventsBetweenActualAndFollStatesWithTimingBounds(State actualState, string section, double elapsedTime)
        {
            EdgeEqualityComparer myEdgeComparer = new EdgeEqualityComparer();

            List<List<SingleEvent>> listOfEvolutionSets = new List<List<SingleEvent>>();

            List<State> listPostStates = getObservedPostStates(actualState);

            List<State> listTimeFilteredPostStates = new List<State>();

            // filter states depending on the valid timing intervall
            foreach (State postState in listPostStates)
            {
                if (section == "upper")
                {
                    if (elapsedTime <= getTransition(actualState, postState).MinDuration)
                    {
                        listTimeFilteredPostStates.Add(postState);
                    }
                }
                else if (section == "lower")
                {
                    if (elapsedTime >= getTransition(actualState, postState).MaxDuration)
                    {
                        listTimeFilteredPostStates.Add(postState);
                    }
                }
            }

            foreach (State follState in listTimeFilteredPostStates)
            {
                listOfEvolutionSets.Add(getEvolutionSet(actualState.Lambda, follState.Lambda));
            }

            List<SingleEvent> resultingEvolutionSet = new List<SingleEvent>();

            if (listOfEvolutionSets.Count > 0)
            {
                resultingEvolutionSet = listOfEvolutionSets[0];

                for (int i = 0; i < listOfEvolutionSets.Count - 1; i++)
                {
                    resultingEvolutionSet = (resultingEvolutionSet.Union(listOfEvolutionSets[i + 1], myEdgeComparer)).ToList();
                }
            }
            return resultingEvolutionSet;
        }


        private List<SingleEvent> intersectionOfEventsBetweenActualAndFollStatesWithTimingBounds(State actualState, string section, double elapsedTime)
        {
            EdgeEqualityComparer myEdgeComparer = new EdgeEqualityComparer();

            List<List<SingleEvent>> listOfEvolutionSets = new List<List<SingleEvent>>();
            List<State> listOfPostStates = getObservedPostStates(actualState);

            List<State> listOfTimeFilteredPostStates = new List<State>();

            // filter states depending on the valid timing intervall
            foreach (State postState in listOfPostStates)
            {
                if (section == "upper")
                {
                    if (elapsedTime <= getTransition(actualState, postState).MinDuration)
                    {
                        listOfTimeFilteredPostStates.Add(postState);
                    }
                }
                else if (section == "lower")
                {
                    if (elapsedTime >= getTransition(actualState, postState).MaxDuration)
                    {
                        listOfTimeFilteredPostStates.Add(postState);
                    }
                }
            }

            foreach (State follState in listOfTimeFilteredPostStates)
            {
                listOfEvolutionSets.Add(getEvolutionSet(actualState.Lambda, follState.Lambda));
            }

            List<SingleEvent> resultingEvolutionSet = new List<SingleEvent>();

            if (listOfEvolutionSets.Count > 0)
            {
                resultingEvolutionSet = listOfEvolutionSets[0];

                for (int i = 0; i < listOfEvolutionSets.Count - 1; i++)
                {
                    resultingEvolutionSet = (resultingEvolutionSet.Intersect(listOfEvolutionSets[i + 1], myEdgeComparer)).ToList();
                }
            }
            return resultingEvolutionSet;
        }


        private List<State> getObservedPostStates(State actualState)
        {
            List<State> listOfFollowingStates = new List<State>();

            foreach (Transition transition in actualState.MyTransitions)
            {
                listOfFollowingStates.Add(MyStates[transition.TargetIndex]);
            }
            return listOfFollowingStates;
        }


        private Transition getTransition(State state1, State state2)
        {
            foreach (Transition transition in state1.MyTransitions)
            {
                if (Convert.ToString(transition.TargetIndex) == state2.Name)
                {
                    return transition;
                }
            }
            return null;
        }
    }


    class NDAAOStateListComparer : IEqualityComparer<List<State>>
    {
        public bool Equals(List<State> list1, List<State> list2)
        {
            if (list1 == null)
                if (list2 == null)
                    return true;
            if (list1 == null)
                if (list2 != null)
                    return false;
            if (list2 == null)
                if (list1 != null)
                    return false;
            if (list1.Count != list2.Count)
                return false;
            List<State> intersection = new List<State>();
            foreach (State state1 in list1)
            {
                foreach (State state2 in list2)
                {
                    if (state1 == null && state2 == null)
                        intersection.Add(state1);
                    if (state1 != null)
                    {
                        if (state2 != null)
                        {
                            if (state1.Name == state2.Name)
                            {
                                if (state1.NameAutomaton == state2.NameAutomaton)
                                    intersection.Add(state1);
                            }
                        }
                    }
                }
            }
            if (intersection.Count == list1.Count)
                return true;
            return false;
        }

        public int GetHashCode(List<State> list)
        {
            return list.GetHashCode();
        }


    }
}
