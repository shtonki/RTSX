using System.Drawing;
using OpenTK.Graphics.OpenGL;
using rtsx.src.util;

namespace rtsx.src.view
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

        public void DrawRectangle(Coordinate start, Coordinate end, Color color, float thickness)
        {
            GL.Color3(color);
            GL.LineWidth(thickness);
#pragma warning disable CS0618 
            // One mans "deprecated" is another mans "basis for a framework"
            GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 

            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(start.X, end.Y);

            GL.Vertex2(start.X, end.Y);
            GL.Vertex2(end.X, end.Y);

            GL.Vertex2(end.X, end.Y);
            GL.Vertex2(end.X, start.Y);

            GL.Vertex2(end.X, start.Y);
            GL.Vertex2(start.X, start.Y);

            GL.End();
        }

        public void DrawTexture(Sprites sprite, Coordinate origin, Coordinate end, Color colour)
        {
            DrawTextureR(ImageLoader.GetBinding(sprite), origin, end, colour);
        }

        private void DrawTextureR(TextureBinding textureBinding, Coordinate origin,
            Coordinate end, Color colour)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Color4(colour);
            GL.BindTexture(TextureTarget.Texture2D, textureBinding.GLTextureId);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(origin.X, origin.Y);

            GL.TexCoord2(1, 1);
            GL.Vertex2(end.X, origin.Y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(end.X, end.Y);

            GL.TexCoord2(0, 0);
            GL.Vertex2(origin.X, end.Y);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
