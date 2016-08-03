// -----------------------------------------------------------------------
// <copyright file="Controls.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using System;
    using System.Threading;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Contains the events when a xBox 360 controller is attached.
    /// </summary>
    public class Controls
    {
        /// <summary>
        /// Contains the previous game pad state.
        /// </summary>
        private GamePadState previousGamePadState;

        /// <summary>
        /// The update thread
        /// </summary>
        private Thread updateThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controls"/> class.
        /// </summary>
        public Controls()
        {
            this.previousGamePadState = GamePad.GetState(PlayerIndex.One);
            this.updateThread = new Thread(new ThreadStart(this.Update));
        }

        /// <summary>
        /// Occurs when a button has been pressed.
        /// </summary>
        public event EventHandler<ButtonPressedEventArgs> OnButtonPressed;

        /// <summary>
        /// Occurs when a button has been released.
        /// </summary>
        public event EventHandler<ButtonReleasedEventArgs> OnButtonReleased;

        /// <summary>
        /// Occurs when the thumb stick value has changed.
        /// </summary>
        public event EventHandler<ThumbStickValueChangedEventArgs> OnThumbStickValueChanged;

        /// <summary>
        /// Occurs when the trigger value has changed.
        /// </summary>
        public event EventHandler<TriggerValueChangedEventArgs> OnTriggerValueChanged;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.updateThread.IsBackground = true;
            this.updateThread.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.updateThread.Abort();
        }

        /// <summary>
        /// Fires the on button pressed event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonPressedEventArgs"/> instance containing the event data.</param>
        protected void FireOnButtonPressed(ButtonPressedEventArgs e)
        {
            if (this.OnButtonPressed != null)
            {
                this.OnButtonPressed(this, e);
            }
        }

        /// <summary>
        /// Fires the on button released event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonReleasedEventArgs"/> instance containing the event data.</param>
        protected void FireOnButtonReleased(ButtonReleasedEventArgs e)
        {
            if (this.OnButtonReleased != null)
            {
                this.OnButtonReleased(this, e);
            }
        }

        /// <summary>
        /// Fires the on thumb stick value changed event.
        /// </summary>
        /// <param name="e">The <see cref="ThumbStickValueChangedEventArgs"/> instance containing the event data.</param>
        protected void FireOnThumbStickValueChanged(ThumbStickValueChangedEventArgs e)
        {
            if (this.OnThumbStickValueChanged != null)
            {
                this.OnThumbStickValueChanged(this, e);
            }
        }

        /// <summary>
        /// Fires the on trigger value changed event.
        /// </summary>
        /// <param name="e">The <see cref="TriggerValueChangedEventArgs"/> instance containing the event data.</param>
        protected void FireOnTriggerValueChanged(TriggerValueChangedEventArgs e)
        {
            if (this.OnTriggerValueChanged != null)
            {
                this.OnTriggerValueChanged(this, e);
            }
        }

        /// <summary>
        /// Updates the controller input.
        /// </summary>
        private void Update()
        {
            try
            {
                while (true)
                {
                    ////Console.WriteLine("Begin Checking");
                    this.UpdateInput();
                    ////Console.WriteLine("End Checking");
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        /// <summary>
        /// Updates the input.
        /// </summary>
        private void UpdateInput()
        {
            // Get the current gamepad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            // Process input only if connected.
            if (currentState.IsConnected)
            {
                // Checking if buttons have been pressed
                this.CheckPressed(currentState);

                // Checking if buttons have been released
                this.CheckReleased(currentState);

                // Checking if ThumbSticks have changed Position
                this.CheckThumbSticks(currentState);

                // Checking if Triggers have Changed
                this.CheckTriggers(currentState);

                // Update previous gamepad state.
                this.previousGamePadState = currentState;
            }  
            else
            {
                ////Console.WriteLine("No Controller connected");
            }         
        }

        /// <summary>
        /// Checks the pressed buttons.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void CheckPressed(GamePadState currentState)
        {
            // DPad UP
            if (currentState.DPad.Up == ButtonState.Pressed &&
                this.previousGamePadState.DPad.Up == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.DPadUp));
            }

            // DPad Down
            if (currentState.DPad.Down == ButtonState.Pressed &&
                this.previousGamePadState.DPad.Down == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.DPadDown));
            }

            // DPad Right
            if (currentState.DPad.Right == ButtonState.Pressed &&
                this.previousGamePadState.DPad.Right == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.DPadRight));
            }

            // DPad Left
            if (currentState.DPad.Left == ButtonState.Pressed &&
                this.previousGamePadState.DPad.Left == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.DPadLeft));
            }

            // Left Bumper
            if (currentState.Buttons.LeftShoulder == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.LeftShoulder == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.LeftBumper));
            }

            // Right Bumper
            if (currentState.Buttons.RightShoulder == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.RightBumper));
            }

            // Back
            if (currentState.Buttons.Back == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.Back == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.Back));
            }

            // Start
            if (currentState.Buttons.Start == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.Start == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.Start));
            }

            // A
            if (currentState.Buttons.A == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.A == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.AButton));
            }

            // B
            if (currentState.Buttons.B == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.B == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.BButton));
            }

            // X
            if (currentState.Buttons.X == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.X == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.XButton));
            }

            // Y
            if (currentState.Buttons.Y == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.Y == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.YButton));
            }

            // Left Stick
            if (currentState.Buttons.LeftStick == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.LeftStick == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.LeftStick));
            }

            // Right Stick
            if (currentState.Buttons.RightStick == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.RightStick == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.RightStick));
            }

            // Big Button
            if (currentState.Buttons.BigButton == ButtonState.Pressed &&
                this.previousGamePadState.Buttons.BigButton == ButtonState.Released)
            {
                this.FireOnButtonPressed(new ButtonPressedEventArgs(Buttons.BigButton));
            }
        }

        /// <summary>
        /// Checks the released buttons.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void CheckReleased(GamePadState currentState)
        {
            // DPad UP
            if (currentState.DPad.Up == ButtonState.Released &&
                this.previousGamePadState.DPad.Up == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.DPadUp));
            }

            // DPad Down
            if (currentState.DPad.Down == ButtonState.Released &&
                this.previousGamePadState.DPad.Down == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.DPadDown));
            }

            // DPad Right
            if (currentState.DPad.Right == ButtonState.Released &&
                this.previousGamePadState.DPad.Right == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.DPadRight));
            }

            // DPad Left
            if (currentState.DPad.Left == ButtonState.Released &&
                this.previousGamePadState.DPad.Left == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.DPadLeft));
            }

            // Left Bumper
            if (currentState.Buttons.LeftShoulder == ButtonState.Released &&
                this.previousGamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.LeftBumper));
            }

            // Right Bumper
            if (currentState.Buttons.RightShoulder == ButtonState.Released &&
                this.previousGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.RightBumper));
            }

            // Back
            if (currentState.Buttons.Back == ButtonState.Released &&
                this.previousGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.Back));
            }

            // Start
            if (currentState.Buttons.Start == ButtonState.Released &&
                this.previousGamePadState.Buttons.Start == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.Start));
            }

            // A
            if (currentState.Buttons.A == ButtonState.Released &&
                this.previousGamePadState.Buttons.A == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.AButton));
            }

            // B
            if (currentState.Buttons.B == ButtonState.Released &&
                this.previousGamePadState.Buttons.B == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.BButton));
            }

            // X
            if (currentState.Buttons.X == ButtonState.Released &&
                this.previousGamePadState.Buttons.X == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.XButton));
            }

            // Y
            if (currentState.Buttons.Y == ButtonState.Released &&
                this.previousGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.YButton));
            }

            // Left Stick
            if (currentState.Buttons.LeftStick == ButtonState.Released &&
                this.previousGamePadState.Buttons.LeftStick == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.LeftStick));
            }

            // Right Stick
            if (currentState.Buttons.RightStick == ButtonState.Released &&
                this.previousGamePadState.Buttons.RightStick == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.RightStick));
            }

            // Big Button
            if (currentState.Buttons.BigButton == ButtonState.Released &&
                this.previousGamePadState.Buttons.BigButton == ButtonState.Pressed)
            {
                this.FireOnButtonReleased(new ButtonReleasedEventArgs(Buttons.BigButton));
            }
        }

        /// <summary>
        /// Checks the thumb sticks.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void CheckThumbSticks(GamePadState currentState)
        {
            // Left
            if (currentState.ThumbSticks.Left != this.previousGamePadState.ThumbSticks.Left)
            {
                this.FireOnThumbStickValueChanged(new ThumbStickValueChangedEventArgs(ThumbSticks.left, currentState.ThumbSticks.Left, this.previousGamePadState.ThumbSticks.Left));
            }

            // Right
            if (currentState.ThumbSticks.Right != this.previousGamePadState.ThumbSticks.Right)
            {
                this.FireOnThumbStickValueChanged(new ThumbStickValueChangedEventArgs(ThumbSticks.right, currentState.ThumbSticks.Right, this.previousGamePadState.ThumbSticks.Right));
            }

            //Vector2
        }

        /// <summary>
        /// Checks the triggers.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void CheckTriggers(GamePadState currentState)
        {
            // Left
            if (currentState.Triggers.Left != this.previousGamePadState.Triggers.Left)
            {
                this.FireOnTriggerValueChanged(new TriggerValueChangedEventArgs(Triggers.left, currentState.Triggers.Left, this.previousGamePadState.Triggers.Left));
            }

            // Right
            if (currentState.Triggers.Right != this.previousGamePadState.Triggers.Right)
            {
                this.FireOnTriggerValueChanged(new TriggerValueChangedEventArgs(Triggers.right, currentState.Triggers.Right, this.previousGamePadState.Triggers.Right));
            }
        }
    }
}
