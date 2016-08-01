// -----------------------------------------------------------------------
// <copyright file="ButtonReleasedEventArgs.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using System;

    /// <summary>
    /// Contains the event arguments of the button released event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ButtonReleasedEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier of the button.
        /// </summary>
        private Buttons identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonReleasedEventArgs"/> class.
        /// </summary>
        /// <param name="button">The button.</param>
        public ButtonReleasedEventArgs(Buttons button)
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