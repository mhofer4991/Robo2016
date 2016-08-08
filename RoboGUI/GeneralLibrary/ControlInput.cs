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
        public ControlInput(int code, bool released, bool useValue, float value)
        {
            this.Code = code;
            this.Released = released;
            this.UseValue = useValue;
            this.Value = value;
        }

        public ControlInput(int code, bool released) : this(code, released, false, 0)
        {
        }

        public int Code { get; set; }

        public bool Released { get; set; }

        public bool UseValue { get; set; }

        public float Value { get; set; }
    }
}
