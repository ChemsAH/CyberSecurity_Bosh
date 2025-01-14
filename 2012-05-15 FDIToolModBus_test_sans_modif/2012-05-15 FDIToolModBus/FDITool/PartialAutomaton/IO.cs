using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDITool.PartialAutomaton
{
    [Serializable]
    public class IO
    {
        /// <summary>
        /// Index des IOs im empfangenen IO-Vektor string
        /// </summary>
        public int Index { get; set; }


        /// <summary>
        /// Name des IOs
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Beschreibung des IOs (aus SPS-Programm)
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// true if the I/O is an input of the PLC
        /// </summary>
        public bool IsInput { get; set; }


        public IO(int index, string name, string description, bool isInput)
        {
            Index = index;
            Name = name;
            Description = description;
            IsInput = isInput;
        }

        public IO() { }
    }


    /// <summary>
    /// Compares two I/Os 
    /// </summary>
    public class IOEqualityComparer : IEqualityComparer<IO>
    {
        public bool Equals(IO io1, IO io2)
        {
            if (io1.IsInput == io2.IsInput)
                if (io1.Index == io2.Index)
                    if (io1.Name == io2.Name)
                        if (io1.Description == io2.Description)
                            return true;
            return false;
        }

        public int GetHashCode(IO io)
        {
            return io.GetHashCode();
        }
    }
}
