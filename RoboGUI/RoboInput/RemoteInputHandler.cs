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

        public const int CODE_SCAN = 6;

        private IManualRobotInput robotInput;

        private IRemoteInputSender inputSender;

        private bool isMoving;

        private bool isRotating;

        public RemoteInputHandler(IManualRobotInput robotInput, IRemoteInputSender inputSender)
        {
            this.robotInput = robotInput;

            this.robotInput.OnMoveForward += RobotInput_OnMoveForward;
            this.robotInput.OnMoveBackward += RobotInput_OnMoveBackward;
            this.robotInput.OnTurnRight += RobotInput_OnTurnRight;
            this.robotInput.OnTurnLeft += RobotInput_OnTurnLeft;
            this.robotInput.OnStopMovement += RobotInput_OnStopMovement;
            this.robotInput.OnScanArea += RobotInput_OnScanArea;

            this.inputSender = inputSender;

            this.isMoving = false;
            this.isRotating = false;
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
            /*if (!released)
            {
                if (!isRotating)
                {
                    ControlInput input = new ControlInput(CODE_FORWARD, released);

                    this.inputSender.SendInput(input);

                    isMoving = true;
                }
            }
            else
            {
                if (isMoving)
                {
                    ControlInput input = new ControlInput(CODE_FORWARD, released);

                    this.inputSender.SendInput(input);

                    isMoving = false;
                }
            }*/
            ControlInput input = new ControlInput(CODE_FORWARD, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnMoveBackward(bool released)
        {
            /*if (!released)
            {
                if (!isRotating)
                {
                    ControlInput input = new ControlInput(CODE_BACKWARD, released);

                    this.inputSender.SendInput(input);

                    isMoving = true;
                }
            }
            else
            {
                if (isMoving)
                {
                    ControlInput input = new ControlInput(CODE_BACKWARD, released);

                    this.inputSender.SendInput(input);

                    isMoving = false;
                }
            }*/
            ControlInput input = new ControlInput(CODE_BACKWARD, released);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnTurnRight(bool released)
        {
            /*if (!released)
            {
                if (!isMoving)
                {
                    ControlInput input = new ControlInput(CODE_RIGHT, released);

                    this.inputSender.SendInput(input);

                    isRotating = true;
                }
            }
            else
            {
                if (isRotating)
                {
                    ControlInput input = new ControlInput(CODE_RIGHT, released);

                    this.inputSender.SendInput(input);

                    isRotating = false;
                }
            }*/
            if (released)
            {
                ControlInput input = new ControlInput(CODE_RIGHT, released, true, 90);

                this.inputSender.SendInput(input);
            }
        }

        private void RobotInput_OnTurnLeft(bool released)
        {
            /*if (!released)
            {
                if (!isMoving)
                {
                    ControlInput input = new ControlInput(CODE_LEFT, released);

                    this.inputSender.SendInput(input);

                    isRotating = true;
                }
            }
            else
            {
                if (isRotating)
                {
                    ControlInput input = new ControlInput(CODE_LEFT, released);

                    this.inputSender.SendInput(input);

                    isRotating = false;
                }
            }*/
            if (released)
            {
                ControlInput input = new ControlInput(CODE_LEFT, released, true, 90);

                this.inputSender.SendInput(input);
            }
        }

        private void RobotInput_OnStopMovement()
        {
            ControlInput input = new ControlInput(CODE_STOP, true);

            this.inputSender.SendInput(input);
        }

        private void RobotInput_OnScanArea()
        {
            ControlInput input = new ControlInput(CODE_SCAN, true, true, 0);

            this.inputSender.SendInput(input);
        }
    }
}
