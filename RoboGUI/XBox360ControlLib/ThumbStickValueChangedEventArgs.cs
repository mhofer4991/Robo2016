// -----------------------------------------------------------------------
// <copyright file="ThumbStickValueChangedEventArgs.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Contains the event arguments of the thumb stick value changed event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ThumbStickValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private ThumbSticks identifier;

        /// <summary>
        /// The new value
        /// </summary>
        private Vector newValue;

        /// <summary>
        /// The old value
        /// </summary>
        private Vector oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbStickValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newVal">The new value.</param>
        /// <param name="oldVal">The old value.</param>
        public ThumbStickValueChangedEventArgs(ThumbSticks id, Vector2 newVal, Vector2 oldVal)
        {
            this.Identifier = id;
            this.NewValue = new Vector(newVal);
            this.OldValue = new Vector(oldVal);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public ThumbSticks Identifier
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
        public Vector NewValue
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
        public Vector OldValue
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