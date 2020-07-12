using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace EvllyEngine
{
    public class Texture
    {
        public int Handle;
        public int _Width;
        public int _Height;
        public byte[] _imagePixel;

        public TextureTarget _TextureTarget = TextureTarget.Texture2D;
        public PixelInternalFormat _PixelInternalFormat = PixelInternalFormat.Rgba;
        public PixelFormat _PixelFormat = PixelFormat.Bgra;


        public Texture(string TextureName, string FileExtension)
        {
            ImageFile imageData = AssetsManager.LoadImage("Assets/Texture/", TextureName, FileExtension);

            _Width = imageData._width;
            _Height = imageData._height;

            Handle = GL.GenTexture();

            Use();

            GL.TexImage2D(_TextureTarget, 0, _PixelInternalFormat, _Width, _Height, 0, _PixelFormat, PixelType.UnsignedByte, imageData._ImgData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Delete()
        {
            _imagePixel = null;
        }

        public byte[] GetimageData { get { return _imagePixel; } }
        public int GetWidth { get { return _Width; } }
        public int GetHeight { get { return _Height; } }
    }
}
