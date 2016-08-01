// -----------------------------------------------------------------------
// <copyright file="TriggerValueChangedEventArgs.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using System;

    /// <summary>
    /// Contains the event arguments of the trigger value changed event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TriggerValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private Triggers identifier;

        /// <summary>
        /// The new value
        /// </summary>
        private float newValue;

        /// <summary>
        /// The old value
        /// </summary>
        private float oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newVal">The new value.</param>
        /// <param name="oldVal">The old value.</param>
        public TriggerValueChangedEventArgs(Triggers id, float newVal, float oldVal)
        {
            this.Identifier = id;
            this.NewValue = newVal;
            this.OldValue = oldVal;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Triggers Identifier
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

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public float NewValue
        {
            get
            {
                return this.newValue;
            }

            set
            {
                this.newValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public float OldValue
        {
            get
            {
                return this.oldValue;
            }

            set
            {
                this.oldValue = value;
            }
        }
    }
}