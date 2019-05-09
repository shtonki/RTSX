using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.util
{
    interface Loggable
    {
        string Log();
    }

    static class Logging
    {
        static long startTick;

        static Logging()
        {
            startTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private static void LogRaw(string message)
        {
            long currentTick = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTick;

            Console.WriteLine(currentTick + ": " + message);
        }

        public static void Log(Loggable l)
        {
            LogRaw(l.Log());
        }

        public static void Log(object o)
        {
            LogRaw(o.ToString());
        }
    }
}
