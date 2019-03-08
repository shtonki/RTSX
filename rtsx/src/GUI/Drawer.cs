using System.Drawing;
using OpenTK.Graphics.OpenGL;
using rtsx.src.util;

namespace rtsx.src.GUI
{
    class Drawer
    {
        public void FillRectangle(Coordinate start, Coordinate end, Color color)
        {
            GL.Color3(color);

#pragma warning disable CS0618 
            // One mans "deprecated" is another mans "basis for a framework"
            GL.Begin(BeginMode.Quads);
#pragma warning restore CS0618 

            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(start.X, end.Y);
            GL.Vertex2(end.X, end.Y);
            GL.Vertex2(end.X, start.Y);

            GL.End();
        }
    }
}
