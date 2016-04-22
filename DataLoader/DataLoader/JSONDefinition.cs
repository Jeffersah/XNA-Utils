using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataLoader
{
    abstract class JSONDefinition
    {
        public string name;
    }
    class JSONValue<E> : JSONDefinition
    {
        E value;
        public E getValue()
        {
            return value;
        }
        public static JSONDefinition Load(ref string s)
        {
            string beginning = "";
            string end = "";
            bool stringmode = false;
            bool beg = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '"' &&(i == 0 || s[i-1]!='\\'))
                {
                    stringmode = !stringmode;
                }
                else if (s[i] == ':' && !stringmode)
                {
                    beg = false;
                }
                else if ((s[i] == ']' || s[i] == '}' || s[i] == ',') && !stringmode)
                {
                    if (s[i] == ',')
                        i++;
                    s = s.Substring(i);
                    return Load(beginning, end);
                }
                else
                {
                    if (beg && stringmode)
                        beginning += s[i];
                    else if (!beg)
                        end += s[i];
                }
            }
            s = "";
            return Load(beginning, end);
        }
        public static JSONDefinition Load(string name, string s)
        {
            Regex isDecimal = new Regex("^[+\\-]?[0-9]*\\.[0-9]+$");
            Regex isInt = new Regex("^[+\\-]?[0-9]+$");
            if (isDecimal.IsMatch(s))
            {
                JSONDouble outp = new JSONDouble();
                outp.name = name;
                outp.value = double.Parse(s);
                return outp;
            }
            else if (isInt.IsMatch(s))
            {
                JSONInt outp = new JSONInt();
                outp.name = name;
                outp.value = int.Parse(s);
                return outp;
            }
            else
            {
                JSONString outp = new JSONString();
                outp.name = name;
                outp.value = s;
                return outp;
            }
        }
    }
    class JSONInt : JSONValue<int>
    {
        public static implicit operator int(JSONInt jint)
        {
            return jint.getValue();
        }
    }
    class JSONString : JSONValue<string>
    {
        public static implicit operator string(JSONString jint)
        {
            return jint.getValue();
        }
    }
    class JSONDouble : JSONValue<double>
    {
        public static implicit operator double(JSONDouble jint)
        {
            return jint.getValue();
        }
    }
    class JSONArray : JSONDefinition
    {
        JSONDefinition[] Elements;

    }
    class JSONObject : JSONDefinition
    {
        List<JSONDefinition> Definitions;

        public JSONObject(string name)
        {
            base.name = name;
            Definitions = new List<JSONDefinition>();
        }
        public JSONObject() : this("Item_key_not_found")
        {
        }

        public JSONDefinition this[string key]
        {
            get
            {
                return Definitions.Find(x => x.name.Equals(key));
            }
            set
            {
                JSONDefinition i = Definitions.Find(x => x.name.Equals(key));
                i = value;
            }
        }
    }
}
