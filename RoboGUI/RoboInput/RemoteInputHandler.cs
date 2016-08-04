using GeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboInput
{
    public class RemoteInputHandler
    {
        public const int CODE_FORWARD = 1;

        public const int CODE_BACKWARD = 2;

        public const int CODE_RIGHT = 3;

        public const int CODE_LEFT = 4;

        public const int CODE_STOP = 5;

        private IManualRobotInput robotInput;

        private IRemoteInputSender inputSender;

        public RemoteInputHandler(IManualRobotInput robotInput, IRemoteInputSender inputSender)
        {
            this.robotInput = robotInput;

            this.robotInput.OnMoveForward += RobotInput_OnMoveForward;
            this.robotInput.OnMoveBackward += RobotInput_OnMoveBackward;
            this.robotInput.OnTurnRight += RobotInput_OnTurnRight;
            this.robotInput.OnTurnLeft += RobotInput_OnTurnLeft;
            this.robotInput.OnStopMovement += RobotInput_OnStopMovement;

            this.inputSender = inputSender;
        }

        public void Start()
        {
            this.robotInput.ListenToInput(true);
        }

        public void Stop()
        {
            this.robotInput.ListenToInput(false);
        }

        private void RobotInput_OnMoveForward(bool released)
        {
            ControlInput input = new ControlInput(CODE_FORWARD, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnMoveBackward(bool released)
        {
            ControlInput input = new ControlInput(CODE_BACKWARD, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnTurnRight(bool released)
        {
            ControlInput input = new ControlInput(CODE_RIGHT, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnTurnLeft(bool released)
        {
            ControlInput input = new ControlInput(CODE_LEFT, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnStopMovement()
        {
            ControlInput input = new ControlInput(CODE_STOP, true);

            this.inputSender.SendInput(input);
        }
    }
}
