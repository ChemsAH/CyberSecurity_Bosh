using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace FDITool
{
    public class XML
    {
        private Model myModel;

        public XML(Model argModel)
        {
            myModel = argModel;
        }


        /// <summary>
        /// Loads the automata structure from an xml-file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true if loaded succesfully</returns>
        public bool LoadAutomataStructureXML(string filename)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(PartialAutomaton.AutomataNetwork));
                FileStream fs = new FileStream(filename, FileMode.Open);
                myModel.MyNetwork = (PartialAutomaton.AutomataNetwork)s.Deserialize(fs);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                return false;
            }
        }


        /// <summary>
        /// Saves the config data into a xml-file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true if saved succesfully</returns>
        public bool SaveConfigXML(string filename)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Settings));
                TextWriter tw = new StreamWriter(filename);
                s.Serialize(tw, this.myModel.MySettings);
                tw.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                return false;
            }

        }


        /// <summary>
        /// Loads the config data from a xml-file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool LoadConfigXML(string filename)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Settings));
                FileStream sr = new FileStream(filename, FileMode.Open);                
                myModel.MySettings = (Settings)s.Deserialize(sr);
                sr.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.ExceptionWriter(e.Message + " " + e.Source + " " + e.StackTrace);
                return false;
            }
        }
    }
}
