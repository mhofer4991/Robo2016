using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralLibrary
{
    [Serializable()]
    public enum Fieldstate
    {
        free,
        occupied,
        freeScanned, 
        unscanned
    }
}