using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DataLoader
{
    public class PropertyObject : Property
    {
        static List<PropertyDefinition> Global;

        static PropertyObject()
        {
            Global = new List<PropertyDefinition>();
        }

        public static void Load(string fileLocation, string nameInGlobal)
        {
            FileReader fr = new FileReader("Content\\" + fileLocation);
            fr.readLine();
            PropertyObject toGlobal = new PropertyObject();
            toGlobal.Load(fr);
            Global.Add(new PropertyDefinition(nameInGlobal,toGlobal));
            fr.Finish();
        }
        public static PropertyObject Load(string fileLocation)
        {
            FileReader fr = new FileReader("Content\\" + fileLocation);
            fr.readLine();
            PropertyObject toGlobal = new PropertyObject();
            toGlobal.Load(fr);
            fr.Finish();
            return toGlobal;
        }

        public static PropertyObject GetGlobal(string name)
        {
            return (PropertyObject)Global.Find(x => x.name.Equals(name)).value;
        }

        List<PropertyDefinition> MyProperties;
        public PropertyObject()
        {
            MyProperties = new List<PropertyDefinition>();
        }

        public override object value()
        {
            return this;
        }

        public void Load(FileReader fr)
        {
            string next;
            while (!(next = fr.readLine().Trim()).Equals( "}" ))
            {
                if (next.Length > 0 && next[0] != '#')
                {
                    string[] splt = next.Split(':');
                    string propname = splt[0];
                    Property adder;
                    if (splt[1].Equals(""))
                    {
                        if (fr.readLine().Trim().Equals("{"))
                        {
                            adder = new PropertyObject();
                            ((PropertyObject)adder).Load(fr);
                        }
                        else
                        {
                            throw new MalformedDataException("Tried to load object without data");
                        }
                    }
                    else
                    {
                        string otherside = splt[1].Trim();
                        Regex isOnlyNumbers = new Regex("^\\-?[0-9]*$");
                        Regex isDecimalNumber = new Regex("^\\-?[0-9]*\\.[0-9]*$");
                        Regex isPoint = new Regex("^\\-?[0-9]+, ?\\-?[0-9]+$");
                        Regex isVector = new Regex("^\\-?[0-9]*\\.[0-9]*, ?\\-?[0-9]*\\.[0-9]*$");
                        if (isOnlyNumbers.IsMatch(otherside))
                        {
                            adder = new IntProperty(int.Parse(otherside));
                        }
                        else if (isDecimalNumber.IsMatch(otherside))
                        {
                            adder = new DoubleProperty(double.Parse(otherside));
                        }
                        else if (isPoint.IsMatch(otherside))
                        {
                            adder = new PointProperty(new Point(int.Parse(otherside.Split(',')[0]), int.Parse(otherside.Split(',')[1].Trim())));
                        }
                        else if (isVector.IsMatch(otherside))
                        {
                            adder = new Vector2Property(new Vector2(float.Parse(otherside.Split(',')[0]), float.Parse(otherside.Split(',')[1].Trim())));
                        }
                        else
                        {
                            adder = new StringProperty(otherside);
                        }
                    }
                    MyProperties.Add(new PropertyDefinition(propname, adder));
                }
            }
        }

        public bool hasProperty(string s)
        {
            foreach (PropertyDefinition p in MyProperties)
                if (p.name.Equals(s))
                    return true;
            return false;
        }


        public PropertyDefinition GetField(string s)
        {
            return MyProperties.Find(x => x.name.Equals(s));
        }

        public Property GetProperty(string s)
        {
            return MyProperties.Find(x => x.name.Equals(s)).value;
        }

        public PropertyObject GetPropertyObject(string s)
        {
            return (PropertyObject)MyProperties.Find(x => x.name.Equals(s)).value;
        }

        public List<PropertyObject> GetObjectsWithProperty(string property, string value)
        {
            List<PropertyObject> outp = new List<PropertyObject>();
            foreach (PropertyDefinition pd in MyProperties)
            {
                if (pd.value.value() is PropertyObject)
                {
                    if ((pd.value.value() as PropertyObject).hasProperty(property))
                    {
                        if (((PropertyObject)pd.value.value()).GetProperty(property).value().Equals(value))
                        {
                            outp.Add(((PropertyObject)pd.value.value()));
                        }
                    }
                }
            }
            return outp;
        }
        public List<Property> GetAllProperties()
        {
            List<Property> output = new List<Property>();
            foreach (PropertyDefinition pdf in MyProperties)
            {
                output.Add(pdf.value);
            }
            return output;
        }

        public List<PropertyDefinition> GetAllFields()
        {
            return MyProperties;
        }

        public List<PropertyObject> GetAllPropertyObjects()
        {
            List<PropertyObject> output = new List<PropertyObject>();
            foreach (PropertyDefinition pdf in MyProperties)
            {
                if(pdf.value is PropertyObject)
                    output.Add(pdf.value as PropertyObject);
            }
            return output;
        }

        public Property this[string s]
        {
            get
            {
                return MyProperties.Find(x => x.name.Equals( s )).value;
            }
            set
            {
            }
        }
        public override string Write()
        {
            string output = "\n{\n";
            foreach (PropertyDefinition pd in MyProperties)
            {
                output += pd.Write();
            }
            output += "}";
            return output;
        }

        public void WriteToContentFile(string fileloc)
        {
            string AllLines = Write().Substring(1);
            System.IO.File.WriteAllText("Content\\" + fileloc, AllLines);
        }
    }

    class MalformedDataException : Exception
    {
        public MalformedDataException(string s) : base(s) { }
    }
}
