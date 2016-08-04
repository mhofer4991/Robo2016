using GeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboInput
{
    public interface IRemoteInputSender
    {
        void SendInput(ControlInput controlInput);
    }
}
