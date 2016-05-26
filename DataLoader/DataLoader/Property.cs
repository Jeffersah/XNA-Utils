using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DataLoader
{
    public class PropertyDefinition
    {
        public string name;
        public Property value;

        public PropertyDefinition(string name, Property value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public abstract class Property
    {
        public abstract object value();
        public virtual E value<E>()
        {
            if(value() is E)
            {
                return (E)value();
            }

            throw new Exception("Type mismatch");
        }
    }

    public class StringProperty : Property
    {
        public string value_s;
        public override object value()
        {
            return value_s;
        }
        public StringProperty(string s)
        {
            value_s = s;
        }

        public static implicit operator string(StringProperty s)
        {
            return s.value_s;
        }
        public static implicit operator StringProperty(string s)
        {
            return new StringProperty(s);
        }
    }

    public class IntProperty : Property
    {
        public int value_i;
        public override object value()
        {
            return value_i;
        }
        public IntProperty(int s)
        {
            value_i = s;
        }

        public override E value<E>()
        {
            if (typeof(E) == typeof(double))
            {
                return (E)Convert.ChangeType(value_i, typeof(double));
            }
            else if (typeof(E) == typeof(float))
            {
                return (E)Convert.ChangeType(value_i, typeof(float));
            }
            else
            {
                return base.value<E>();
            }
        }

        public static implicit operator int(IntProperty s)
        {
            return s.value_i;
        }
        public static implicit operator IntProperty(int s)
        {
            return new IntProperty(s);
        }
    }

    public class DoubleProperty : Property
    {
        public double value_d;
        public override object value()
        {
            return value_d;
        }
        public DoubleProperty(double s)
        {
            value_d = s;
        }
        public override E value<E>()
        {
            if (typeof(E) == typeof(float))
            {
                return (E)Convert.ChangeType(value_d, typeof(float));
            }
            else
            {
                return base.value<E>();
            }
        }

        public static implicit operator double(DoubleProperty s)
        {
            return s.value_d;
        }
        public static implicit operator DoubleProperty(double s)
        {
            return new DoubleProperty(s);
        }
    }

    public class Vector2Property : Property
    {
        public Vector2 value_v;
        public override object value()
        {
            return value_v;
        }
        public Vector2Property(Vector2 s)
        {
            value_v = s;
        }

        public static implicit operator Vector2(Vector2Property s)
        {
            return s.value_v;
        }
        public static implicit operator Vector2Property(Vector2 s)
        {
            return new Vector2Property(s);
        }
    }

    public class PointProperty : Property
    {
        public Point value_v;
        public override object value()
        {
            return value_v;
        }
        public PointProperty(Point s)
        {
            value_v = s;
        }

        public static implicit operator Point(PointProperty s)
        {
            return s.value_v;
        }
        public static implicit operator PointProperty(Point s)
        {
            return new PointProperty(s);
        }
    }
}
