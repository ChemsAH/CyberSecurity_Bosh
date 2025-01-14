using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class AutomataNetwork
    {
        /// <summary>
        /// List of I/Os that are part of the output function
        /// </summary>
        public List<IO> MyIOs { get; set; }


        /// <summary>
        /// List of partial Automta
        /// </summary>
        public List<TAAO> MyAutomata { get; set; }


        /// <summary>
        /// Contains the actual network situation: automaton.name --> state name
        /// If an automaton does not have an actual state, automaton.name --> "none"
        /// </summary>  
        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutRes1 { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutRes2 { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutRes3 { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutRes4 { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutTimedRes1Early { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutTimedRes2Early { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutTimedRes1Late { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<SingleEvent> AutTimedRes2Late { get; set; }
     
        public AutomataNetwork()
        {
            MyAutomata = new List<TAAO>();
            MyIOs = new List<IO>();

            AutRes1 = new List<SingleEvent>();
            AutRes2 = new List<SingleEvent>();
            AutRes3 = new List<SingleEvent>();
            AutRes4 = new List<SingleEvent>();

            AutTimedRes1Early = new List<SingleEvent>();
            AutTimedRes2Early = new List<SingleEvent>();
            AutTimedRes1Late = new List<SingleEvent>();
            AutTimedRes2Late = new List<SingleEvent>();
        }



        /// <summary>
        /// Processes the timed automata network.         
        /// </summary>
        /// <param name="newVector"></param>
        /// <param name="deadlockrepeatTime">after this time, a deadlock fault detection is repeated</param>
        public void ProcessTimedAutomataNetwork(string newVector, double deltaT)
        {
            foreach (TAAO myAutomaton in MyAutomata)
            {
                myAutomaton.TimedNDAAOFaultDetection(newVector, deltaT);
                myAutomaton.TimedNDAAOFaultIsolation(newVector, deltaT);      
            }
            //Log.WriteLine("");

            getGlobalResiduals(); // Collect the Residuals of all automata

            //Log.WriteLine("");4.4.12
        }


        /// <summary>
        /// Determines the unexpected and the missed events in the network
        /// This method performs a simple union of the residuals of the partial automata
        /// </summary>
        /// <returns></returns>
        private void getGlobalResiduals()
        {             
            AutRes1 = new List<SingleEvent>();
            AutRes2 = new List<SingleEvent>();
            AutRes3 = new List<SingleEvent>();
            AutRes4 = new List<SingleEvent>();

            AutTimedRes1Early = new List<SingleEvent>();
            AutTimedRes2Early = new List<SingleEvent>();
            AutTimedRes1Late = new List<SingleEvent>();
            AutTimedRes2Late = new List<SingleEvent>();
              
            foreach (PartialAutomaton.TAAO automaton in MyAutomata)
            {
                if (automaton.MyFaultSituation == TAAO.FaultSituation.FaultDetected)
                {
                    foreach (SingleEvent edge in automaton.Res1)
                        AutRes1 = addEdgeToNWResidual(AutRes1, edge);
                    foreach (SingleEvent edge in automaton.Res2)
                        AutRes2 = addEdgeToNWResidual(AutRes2, edge);
                    foreach (SingleEvent edge in automaton.Res3)
                        AutRes3 = addEdgeToNWResidual(AutRes3, edge);
                    foreach (SingleEvent edge in automaton.Res4)
                        AutRes4 = addEdgeToNWResidual(AutRes4, edge);

                    foreach (SingleEvent edge in automaton.TimedRes1Early)
                        AutTimedRes1Early = addEdgeToNWResidual(AutTimedRes1Early, edge);
                    foreach (SingleEvent edge in automaton.TimedRes2Early)
                        AutTimedRes2Early = addEdgeToNWResidual(AutTimedRes2Early, edge);
                    foreach (SingleEvent edge in automaton.TimedRes1Late)
                        AutTimedRes1Late = addEdgeToNWResidual(AutTimedRes1Late, edge);
                    foreach (SingleEvent edge in automaton.TimedRes2Late)
                        AutTimedRes2Late = addEdgeToNWResidual(AutTimedRes2Late, edge);
                }
            }           
        }


        /// <summary>
        /// Copies a residual set
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
        /// Only edges which are not already part of the Residual list are added
        /// </summary>
        /// <param name="NWRes"></param>
        /// <param name="edge"></param>
        /// <returns></returns>
        private List<SingleEvent> addEdgeToNWResidual(List<SingleEvent> NWRes, SingleEvent edge)
        {
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();

            if (!NWRes.Contains(edge, myComparer))
                NWRes.Add(edge);

            return NWRes;
        }


        /// <summary>
        /// Returns the edge from the I/O-list that has the same index as the given incompleteEdge.
        /// Returns null if no edge is found.
        /// </summary>
        /// <param name="incompleteEdge"></param>
        /// <returns></returns>
        public SingleEvent GetEdgeFromIOList(SingleEvent incompleteEdge)
        {
            foreach (IO io in MyIOs)
            {
                if (io.Index == incompleteEdge.MyIndex)
                {
                    if (incompleteEdge.IsRising)
                        return new SingleEvent(incompleteEdge.MyIndex, incompleteEdge.IsRising, io.Name);
                    else
                        return new SingleEvent(incompleteEdge.MyIndex, incompleteEdge.IsRising, io.Name);
                }
            }
            return null;
        }


        /// <summary>
        /// Returns an int-list containing the indices of the I/Os in the I/O-list of the network.
        /// </summary>
        /// <returns></returns>
        public List<int> GetIndexListIOs()
        {
            List<int> result = (from io in MyIOs select io.Index).ToList();
            return result;
        }


        /// <summary>
        /// Returns true if at least one NDAAO is in a deadlock
        /// </summary>
        /// <returns></returns>
        public bool AtLeastOneNDAAOInDeadLock()
        {            
            foreach (TAAO aut in MyAutomata)
            {
                if (aut.MyFaultType == TAAO.FaultType.DeadLockFault)
                    return true;
            }
            return false;
        }
    }
}
