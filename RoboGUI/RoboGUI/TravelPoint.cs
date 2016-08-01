using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoboGUI
{
    public class TravelPoint
    {
        public TravelPoint(Point position)
        {
            this.Position = position;
        }

        public Point Position
        {
            get;
            private set;
        }
    }
}
