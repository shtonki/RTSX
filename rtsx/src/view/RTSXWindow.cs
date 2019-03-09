using OpenTK;
using OpenTK.Graphics.OpenGL;
using rtsx.src.state;
using System.Drawing;
using System;
using System.Collections.Generic;
using OpenTK.Input;
using rtsx.src.util;
using OpenTK.Graphics;

namespace rtsx.src.view
{
    class RTSXWindow : GameWindow
    {
        private const int WindowHeight = 400;
        private const int WindowWidth = 400;

        public Coordinate MousePosition { get; private set; } = new Coordinate(0, 0);

        public Func<IEnumerable<Drawable>> DrawablesCallback { get; set; }

        public RTSXWindow() : base(WindowHeight, WindowHeight, new GraphicsMode(32, 24, 0, 32), "title here eh")
        {
            GL.Enable(EnableCap.Blend);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Image i = Image.FromFile("C:/Users/Daniel/Pictures/a.png");
            GUI.A = ImageLoader.BindTexture(i);
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

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            var xraw = e.X / (double)Width;
            var yraw = e.Y / (double)Height;

            var xval = xraw * 2 - 1;
            var yval = yraw * 2 - 1;

            // invert Y axis
            yval = -yval;

            MousePosition = new Coordinate(xval, yval);
            Logging.Log(MousePosition);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        private void Render()
        {
            var drawer = new Drawer();

            if (DrawablesCallback != null)
            {
                var drawables = DrawablesCallback();

                foreach (var drawable in drawables)
                {
                    drawable.Draw(drawer);
                }
            }
        }

    }
}
