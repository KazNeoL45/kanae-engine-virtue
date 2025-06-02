using System;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kanae
{
    public class Texture : IDisposable
    {
        public readonly int Handle;
        public readonly string Name;

        public Texture(string name)
        {
            Name = name;
            Handle = GL.GenTexture();
        }

        public void SetData(int width, int height, IntPtr data)
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // Ensure proper alignment for texture data
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            // Generate mipmaps if needed
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Restore default alignment
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
            GC.SuppressFinalize(this);
        }
    }
} 