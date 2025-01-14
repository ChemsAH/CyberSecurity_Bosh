using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class State
    {
        /// <summary>
        /// Alle ausgehende Transitionen
        /// </summary>
        public List<Transition> MyTransitions { get; set; }

        /// <summary>
        /// Zustandsausgabe
        /// </summary>
        public string Lambda { get; set; }

        /// <summary>
        /// Name of the state
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Name of the automaton that owns this state
        /// </summary>
        public string NameAutomaton { get; set; }


        /// <summary>
        /// true if this state is a possible ini state for new system cycles
        /// </summary>
        public bool IsIniState { get; set; }


        /// <summary>
        /// true if this state is a possible last state 
        /// </summary>
        public bool IsLastState { get; set; }


        public State(string name, string lambda, string AutomatonName)
            : this(name, lambda)
        {
            NameAutomaton = AutomatonName;
        }


        public State(string name, string lambda)
        {
            MyTransitions = new List<Transition>();
            Name = name;
            Lambda = lambda;
        }


        public State() { }

        
        /// <summary>
        /// Compares lambda with the observed IO-vector. Bits with a '-'-symbol in lambda are ignored
        /// </summary>
        /// <param name="vector">observed IO vector</param>
        /// <returns>true if the IO vector equals lambda (by ignoring the '-' bits of lambda</returns>
        public bool CompareIOVector(string vector)
        {
            if (vector.Length != this.Lambda.Length)
                return false;
            for (int i = 0; i < this.Lambda.Length; i++)
            {
                if (!(this.Lambda[i] == '-'))
                {
                    if (this.Lambda[i] != vector[i])
                        return false;
                }
            }
            return true;
        }
    }  
}
