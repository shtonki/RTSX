using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rtsx.src.view
{
    static class GUI
    {
        public static RTSXWindow Window { get; private set; }

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

        public static TextureBinding A;

        private static void RunWindow()
        {
            Window = new RTSXWindow();

            // depends on WindowInitializedResetEvent beeing initialized
            WindowInitializedResetEvent.Set();

            Window.Run();
        }
    }
}
