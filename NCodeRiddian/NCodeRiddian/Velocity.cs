using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    /// <summary>
    /// Stores a velocity
    /// </summary>
    public class Velocity
    {
        protected Vector2 xy;
        protected Vector2 ma;

        protected bool XYReq;
        protected bool MAReq;

        public Velocity():this(0,0,false)
        {
        }

        /// <summary>
        /// Create a velocity
        /// </summary>
        /// <param name="xm">The X value, or the magnitude</param>
        /// <param name="ya">The Y value, or the angle</param>
        /// <param name="isMA">TRUE if this velocity follows the [magnitude, angle] form, FALSE if it follows the [X, Y] form</param>
        public Velocity(float xm, float ya, bool isMA)
        {
            xy = new Vector2(xm, ya);
            ma = new Vector2(xm, ya);
            if (isMA)
            {
                XYReq = true;
                MAReq = false;
            }
            else
            {
                MAReq = true;
                XYReq = false;
            }
        }

        protected void Calculate()
        {
            if (MAReq)
            {
                ma = Physics.ConvertToMA(xy);
                MAReq = false;
            }
            if (XYReq)
            {
                xy = Physics.ConvertToXY(ma);
                XYReq = false;
            }
        }

        /// <summary>
        /// Get or set the velocity as X and Y components
        /// </summary>
        public Vector2 XY
        {
            get
            {
                if (XYReq)
                    Calculate();
                return xy;
            }
            set
            {
                if (XYReq)
                    Calculate();
                MAReq = true;
                xy = value;
            }
        }

        /// <summary>
        /// Get or set the velocity as Magnitude and Angle
        /// </summary>
        public Vector2 MA
        {
            get
            {
                if (MAReq)
                    Calculate();
                return ma;
            }
            set
            {
                if (MAReq)
                    Calculate();
                XYReq = true;
                ma = value;
            }
        }

        /// <summary>
        /// Get or set the X component of this velocity
        /// </summary>
        public float X
        {
            get
            {
                return XY.X;
            }
            set
            {
                if (XYReq)
                    Calculate();
                MAReq = true;
                xy.X = value;
            }
        }

        /// <summary>
        /// Get or set the Y component of this velocity
        /// </summary>
        public float Y
        {
            get
            {
                return XY.Y;
            }
            set
            {
                if (XYReq)
                    Calculate();
                MAReq = true;
                xy.Y = value;
            }
        }

        /// <summary>
        /// Get or set the magnitude of this velocity
        /// </summary>
        public float M
        {
            get
            {
                return MA.X;
            }
            set
            {
                if (MAReq)
                    Calculate();
                XYReq = true;
                ma.X = value;
            }
        }
        /// <summary>
        /// Get or set the angle of this velocity
        /// </summary>
        public float A
        {
            get
            {
                return MA.Y;
            }
            set
            {
                if (MAReq)
                    Calculate();
                XYReq = true;
                ma.Y = value;
            }
        }

        /// <summary>
        /// Returns the result of applying this motion to a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 Move(Vector2 point)
        {
            return new Vector2(point.X + XY.X, point.Y + XY.Y);
        }

        /// <summary>
        /// Returns the result of applying a factor of this motion to a point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public Vector2 Move(Vector2 point, float factor)
        {
            return new Vector2(point.X + (factor * XY.X), point.Y + (factor * XY.Y));
        }

        /// <summary>
        /// Adds to velocities
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Velocity Add(Velocity v, Velocity v2)
        {
            return new Velocity(v.X + v2.X, v.Y + v2.Y, false);
        }
        /// <summary>
        /// Subtracts one velocity from another
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Velocity Subtract(Velocity v, Velocity v2)
        {
            return new Velocity(v.X - v2.X, v.Y - v2.Y, false);
        }
        /// <summary>
        /// Multiplies two velocities (XY)
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Velocity Mult(Velocity v, Velocity v2)
        {
            return new Velocity(v.X * v2.X, v.Y * v2.Y, false);
        }
        /// <summary>
        /// Divides two velocities (XY)
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Velocity Div(Velocity v, Velocity v2)
        {
            return new Velocity(v.X / v2.X, v.Y / v2.Y, false);
        }
    }
}
