using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLibrary
{
    [Serializable()]
    public class RoboStatus
    {
        public RoboStatus(float x, float y, float rotation)
        {
            this.X = x;
            this.Y = y;
            this.Rotation = rotation;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Rotation { get; set; }
    }
}
