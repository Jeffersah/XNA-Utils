using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class Velocity
    {
        protected Vector2 xy;
        protected Vector2 ma;

        protected bool XYReq;
        protected bool MAReq;

        public Velocity():this(0,0,false)
        {
        }

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

        public Vector2 Move(Vector2 point)
        {
            return new Vector2(point.X + XY.X, point.Y + XY.Y);
        }
        public Vector2 Move(Vector2 point, float factor)
        {
            return new Vector2(point.X + (factor * XY.X), point.Y + (factor * XY.Y));
        }
        public static Velocity Add(Velocity v, Velocity v2)
        {
            return new Velocity(v.X + v2.X, v.Y + v2.Y, false);
        }
        public static Velocity Subtract(Velocity v, Velocity v2)
        {
            return new Velocity(v.X - v2.X, v.Y - v2.Y, false);
        }
        public static Velocity Mult(Velocity v, Velocity v2)
        {
            return new Velocity(v.X * v2.X, v.Y * v2.Y, false);
        }
        public static Velocity Div(Velocity v, Velocity v2)
        {
            return new Velocity(v.X / v2.X, v.Y / v2.Y, false);
        }
    }
}
