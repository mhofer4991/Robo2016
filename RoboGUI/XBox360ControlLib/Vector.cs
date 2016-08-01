// -----------------------------------------------------------------------
// <copyright file="Vector.cs" company="FHWN.ac.at">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <summary>This library is for getting inputs from the xBox 360 controller.</summary>
// <author>Michael Ploy</author>
// -----------------------------------------------------------------------
namespace XBox360ControlLib
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Represents a vector for the thumb sticks.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">The x value of the vector.</param>
        /// <param name="y">The y value of the vector.</param>
        public Vector(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public Vector(Vector2 vector) : this(vector.X, vector.Y)
        {
        }

        /// <summary>
        /// Gets or sets the x value of the Vector.
        /// </summary>
        /// <value>
        /// The x value of the vector.
        /// </value>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y value of the vector.
        /// </summary>
        /// <value>
        /// The y value of the vector.
        /// </value>
        public float Y { get; set; }
    }
}