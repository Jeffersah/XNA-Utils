using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLoader
{
    public class Loader
    {
        public static void Load(string fileLocation, string nameInGlobal)
        {
            PropertyObject.Load(fileLocation, nameInGlobal);
        }
        public static PropertyObject Load(string fileLocation)
        {
            return PropertyObject.Load(fileLocation);
        }
    }
}
