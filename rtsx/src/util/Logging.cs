using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.util
{
    static class Logging
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(object o)
        {
            Log(o.ToString());
        }
    }
}
