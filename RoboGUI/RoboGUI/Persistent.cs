using GeneralLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RoboGUI
{
    public static class Persistent
    {
        public static bool SaveMap(Map map, string path)
        {
            try
            {
                Stream stream = File.Open(path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, map);
                stream.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Map LoadMap(string path)
        {
            Map map = null;

            try
            {
                Stream stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                map = (Map)bformatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
            }

            return map;
        }
    }
}
