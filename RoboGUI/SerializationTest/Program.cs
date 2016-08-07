using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using GeneralLibrary;
using Newtonsoft.Json;

namespace SerializationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Field f = new Field(21, 6);
            Map m = new Map();
            m.Fields = new Field[1, 1];
            m.Fields[0, 0] = f;
            Fieldstate fs = Fieldstate.freeScanned;
            Position p = new Position(9, 11);
            RoboStatus r = new RoboStatus(9,11,90);
            
            // Serializing to JSON
            string text = JsonConvert.SerializeObject(r);
            Console.WriteLine(text);
            using (StreamWriter writer = new StreamWriter("Json.txt", false,  Encoding.UTF8))
            {
                writer.Write(text);
            }

            Console.ReadLine();

            string jsonText;
            using (StreamReader reader = new StreamReader("Json.txt", Encoding.UTF8))
            {
                jsonText = reader.ReadToEnd();
            }

            // Deserializing from JSON
            Map map = JsonConvert.DeserializeObject<Map>(jsonText);

            Console.ReadLine();*/
            Field[,] fields = new Field[200, 200];

            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    fields[i, j] = new Field(i, j) { State = Fieldstate.free };
                }
            }

            Map ma = new Map();

            ma.Fields = fields;

            string text = JsonConvert.SerializeObject(ma);

            File.AppendAllText("hallo.txt", text);
        }     
    }
}
