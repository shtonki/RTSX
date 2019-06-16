﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace rtsx.src.view
{
    class TextureBinding
    {
        public int GLTextureId { get; }

        public TextureBinding(int gLTextureId)
        {
            GLTextureId = gLTextureId;
        }
    }

    public enum Sprites
    {
        Hellspawn,

        Warrior,

        Tree,
    }

    static class ImageLoader
    {
        private static Dictionary<Sprites, TextureBinding> LoadedTextures = new Dictionary<Sprites, TextureBinding>();

        private static Dictionary<Sprites, Image> ImageDictionary;

        public static void LoadTextures()
        {
            ImageDictionary = new Dictionary<Sprites, Image>();

            ImageDictionary[Sprites.Hellspawn] = Properties.Resources.hellspawn;
            ImageDictionary[Sprites.Warrior] = Properties.Resources.soldier;
            ImageDictionary[Sprites.Tree] = Properties.Resources.tree;
        }

        public static TextureBinding GetBinding(Sprites sprite)
        {
            if (!LoadedTextures.ContainsKey(sprite))
            {
                LoadedTextures[sprite] = BindTexture(ImageDictionary[sprite]);
            }

            return LoadedTextures[sprite];
        }

        private static TextureBinding BindTexture(Image image)
        {
            var glId = MakeTexture(image);

            return new TextureBinding(glId);
        }

        private static int MakeTexture(Image image)
        {
            int id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(image);
            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            return id;
        }
    }
}
