using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rtsx.src.GUI
{
    static class GUI
    {
        private static RTSXWindow Window;

        private static ManualResetEventSlim WindowInitializedResetEvent;

        public static void Launch()
        {
            var guiThread = new Thread(new ThreadStart(RunWindow));
            guiThread.IsBackground = false;
            guiThread.Name = "RTSXGUI";
            guiThread.Start();

            WindowInitializedResetEvent = new ManualResetEventSlim();
            WindowInitializedResetEvent.Wait();
        }

        public static void SetGetDrawablesCallback(
            Func<IEnumerable<Drawable>> callback)
        {
            Window.GetDrawablesCallback = callback;
        }

        private static void RunWindow()
        {
            Window = new RTSXWindow();

            // depends on WindowInitializedResetEvent beeing initialized
            WindowInitializedResetEvent.Set();

            Window.Run();
        }
    }
}
