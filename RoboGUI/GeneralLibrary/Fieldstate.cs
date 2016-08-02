using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralLibrary
{
    [Serializable()]
    public enum Fieldstate
    {
        free = 0,
        occupied = 1,
        freeScanned = 2,
        unscanned = 3
    }
}