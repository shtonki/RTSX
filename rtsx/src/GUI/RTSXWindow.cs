using OpenTK;
using OpenTK.Graphics.OpenGL;
using rtsx.src.state;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace rtsx.src.GUI
{
    class RTSXWindow : GameWindow
    {
        private const int WindowHeight = 800;
        private const int WindowWidth = 800;

        public Func<IEnumerable<Drawable>> GetDrawablesCallback { get; set; }

        public RTSXWindow() : base(WindowHeight, WindowHeight)
        {
            GL.Enable(EnableCap.Blend);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            GL.PushMatrix();
            Render();
            SwapBuffers();
            GL.PopMatrix();
        }


        private void Render()
        {
            var drawer = new Drawer();

            if (GetDrawablesCallback != null)
            {
                var drawables = GetDrawablesCallback();

                foreach (var drawable in drawables)
                {
                    drawable.Draw(drawer);
                }
            }
        }

    }
}
