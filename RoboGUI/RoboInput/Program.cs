using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBox360ControlLib;

namespace RoboInput
{
    class Program
    {

        private static Controls xbox;

        static void Main(string[] args)
        {
            IManualRobotInput i = new XBoxRobotInput();

            i.OnMoveForward += I_OnMoveForward;

            i.ListenToInput(true);

            Console.ReadLine();
        }

        private static void I_OnMoveForward(bool released)
        {
            Console.WriteLine("asdfasfd");
        }
    }
}
