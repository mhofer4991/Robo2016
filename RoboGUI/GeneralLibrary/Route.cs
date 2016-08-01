using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralLibrary
{
    [Serializable()]
    public class Route
    {
        public Route(List<Position> route)
        {
            this.Points = route;
        }

        public List<Position> Points { get; set; }
    }
}