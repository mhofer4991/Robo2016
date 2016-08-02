﻿using System;
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
            this.State = Fieldstate.free;
        }

        public Fieldstate State { get; set; }

        public Position Position { get; set; }
    }
}