using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBox360ControlLib;

namespace RoboInput
{
    public class XBoxRobotInput : IManualRobotInput
    {
        private const float DefaultSpeed = 100;

        private Controls xbox;

        private bool isListening;

        private bool scanPressed;

        public XBoxRobotInput()
        {
            this.xbox = new Controls();

            this.xbox.OnButtonPressed += Xbox_OnButtonPressed;
            this.xbox.OnButtonReleased += Xbox_OnButtonReleased;
            this.xbox.OnThumbStickValueChanged += Xbox_OnThumbStickValueChanged;
            this.xbox.OnTriggerValueChanged += Xbox_OnTriggerValueChanged;

            this.scanPressed = false;
        }

        public event MoveBackward OnMoveBackward;

        public event MoveForward OnMoveForward;

        public event TurnLeft OnTurnLeft;

        public event TurnRight OnTurnRight;

        public event StopMovement OnStopMovement;

        public event ScanArea OnScanArea;

        public void ListenToInput(bool listen)
        {
            if (listen && !this.isListening)
            {
                this.xbox.Start();
            }
            else if (!listen && this.isListening)
            {
                this.xbox.Stop();
            }

            this.isListening = listen;
        }

        private void Xbox_OnTriggerValueChanged(object sender, TriggerValueChangedEventArgs e)
        {

        }

        private void Xbox_OnThumbStickValueChanged(object sender, ThumbStickValueChangedEventArgs e)
        {

        }

        private void Xbox_OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
            switch (e.Identifier)
            {
                case Buttons.DPadRight:
                    if (this.OnTurnRight != null)
                    {
                        this.OnTurnRight(true);
                    }

                    break;
                case Buttons.DPadLeft:
                    if (this.OnTurnLeft != null)
                    {
                        this.OnTurnLeft(true);
                    }

                    break;
                case Buttons.DPadUp:
                    if (this.OnMoveForward != null)
                    {
                        this.OnMoveForward(true);
                    }

                    break;
                case Buttons.DPadDown:
                    if (this.OnMoveBackward != null)
                    {
                        this.OnMoveBackward(true);
                    }

                    break;
                case Buttons.BButton:
                    if (this.OnStopMovement != null)
                    {
                        this.OnStopMovement();
                    }

                    break;
                case Buttons.XButton:
                    this.scanPressed = true;

                    if (this.OnScanArea != null)
                    {
                        this.OnScanArea();
                    }

                    break;
            }
        }

        private void Xbox_OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            switch (e.Identifier)
            {
                case Buttons.DPadRight:
                    if (this.OnTurnRight != null)
                    {
                        this.OnTurnRight(false);
                    }

                    break;
                case Buttons.DPadLeft:
                    if (this.OnTurnLeft != null)
                    {
                        this.OnTurnLeft(false);
                    }

                    break;
                case Buttons.DPadUp:
                    if (this.OnMoveForward != null)
                    {
                        this.OnMoveForward(false);
                    }

                    break;
                case Buttons.DPadDown:
                    if (this.OnMoveBackward != null)
                    {
                        this.OnMoveBackward(false);
                    }

                    break;
            }
        }
    }
}
