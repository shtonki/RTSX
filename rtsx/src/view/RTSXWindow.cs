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
        private const int WindowHeight = 800;
        private const int WindowWidth = 800;

        public Coordinate MousePosition { get; private set; } = new Coordinate(0, 0);

        public Scene Scene;

        public RTSXWindow() : base(WindowHeight, WindowHeight, new GraphicsMode(32, 24, 0, 32), "title here eh")
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ImageLoader.LoadTextures();
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
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Scene?.HandleInput(new MouseInput(MouseInput.MouseDirection.Down, e.Button));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Scene?.HandleInput(new MouseInput(MouseInput.MouseDirection.Up, e.Button));
        }

        private void Render()
        {
            if (Scene == null) { return; }

            Scene.Render(new Drawer());
        }

    }
}
