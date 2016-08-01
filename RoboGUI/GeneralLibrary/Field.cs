using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralLibrary
{
    [Serializable()]
    public class Field
    {
        public Field(int x, int y)
        {
            this.Position = new Position(x,y);
        }

        public Fieldstate State { get; private set; }

        public Position Position { get; set; }
    }
}