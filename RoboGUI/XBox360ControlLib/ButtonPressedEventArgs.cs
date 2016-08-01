// -----------------------------------------------------------------------
// <copyright file="ButtonPressedEventArgs.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using System;

    /// <summary>
    /// Contains the Event arguments of the button pressed event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ButtonPressedEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier of the button.
        /// </summary>
        private Buttons identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPressedEventArgs"/> class.
        /// </summary>
        /// <param name="button">The button.</param>
        public ButtonPressedEventArgs(Buttons button)
        {
            this.Identifier = button;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Buttons Identifier
        {
            get
            {
                return this.identifier;
            }

            set
            {
                this.identifier = value;
            }
        }
    }
}