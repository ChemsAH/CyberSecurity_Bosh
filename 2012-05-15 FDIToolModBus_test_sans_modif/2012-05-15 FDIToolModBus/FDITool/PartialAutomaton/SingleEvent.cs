using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class SingleEvent
    {
        /// <summary>
        /// Index des zurgehörigen IOs im IO-Vektor
        /// </summary>
        public int MyIndex { get; set; }


        /// <summary>
        /// Wert des single Events (true wenn Wechsel von 0 auf 1, false wenn Wechsel von 1 auf 0)
        /// </summary>
        public bool IsRising { get; set; }


        /// <summary>
        /// Name des zugehörigen IOs
        /// </summary>
        public string Name { get; set; }


        public SingleEvent(int index, bool isRising, string name)
        {
            MyIndex = index;
            IsRising = isRising;

            if (IsRising)
                Name = name + "_1";
            else
                Name = name + "_0";
        }


        public SingleEvent() { }
    }


    /// <summary>
    /// Compares two single events
    /// </summary>
    class EdgeEqualityComparer : IEqualityComparer<SingleEvent>
    {
        public bool Equals(SingleEvent sE1, SingleEvent sE2)
        {
            if (sE1.MyIndex == sE2.MyIndex)
                if (sE1.IsRising == sE2.IsRising)
                    return true;

            return false;
        }

        /// <summary>
        /// Remark: hash code must be equal for two objects that are considered as equal by the Equals method
        /// </summary>
        /// <param name="sE"></param>
        /// <returns></returns>
        public int GetHashCode(SingleEvent sE)
        {
            int result = sE.MyIndex.GetHashCode() ^ sE.IsRising.GetHashCode();
            return result;
        }
    }


    /// <summary>
    /// Compares two lists of single Events
    /// </summary>
    class EdgeListEqualityComparer
    {
        public static bool Equals(List<SingleEvent> list1, List<SingleEvent> list2)
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
            EdgeEqualityComparer myComparer = new EdgeEqualityComparer();
            
            foreach (SingleEvent sE in list1)
            {
                if (!list2.Contains(sE, myComparer))
                {
                    return false;
                }
            }
            foreach (SingleEvent sE in list2)
            {
                if (!list1.Contains(sE, myComparer))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
