using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class Transition
    {
        /// <summary>
        /// Index of the target state in the MyStates list
        /// </summary>
        public int TargetIndex { get; set; }


        /// <summary>
        /// Index of the source state in the MyState list
        /// </summary>
        public int SourceIndex { get; set; }


        /// <summary>
        /// Minimum transition duration
        /// </summary>
        public double MinDuration { get; set; }


        /// <summary>
        /// Maximum transition duration
        /// </summary>
        public double MaxDuration { get; set; }


        /// <param name="targetIndex">Index of the target state in the MyStates list</param>
        /// <param name="sourceIndex">Index of the source state in the MyState list</param>
        public Transition(int targetIndex, int sourceIndex, double minDuration, double maxDuration)
        {
            TargetIndex = targetIndex;
            SourceIndex = sourceIndex;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
        }

        public Transition() { }
    }
}
