using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLibrary
{
    [Serializable()]
    public class ControlInput
    {
        public ControlInput(int code, bool released)
        {
            this.Code = code;
            this.Released = released;
        }

        public int Code { get; set; }

        public bool Released { get; set; }
    }
}
