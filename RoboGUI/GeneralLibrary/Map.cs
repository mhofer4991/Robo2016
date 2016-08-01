using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralLibrary
{
    [Serializable()]
    public class Map
    {
        public Field[,] Fields { get; set; }

        public int Id { get; private set; }
    }
}