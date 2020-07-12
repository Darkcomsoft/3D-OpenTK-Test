using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.Drawing.Imaging;

namespace EvllyEngine
{
    public class AssetsManager
    {
        public AssetsManager() {}

        public static ShaderFile LoadShader(string path, string file)
        {
            string vertshader = string.Concat(path, file, ".vert");
            string fragshader = string.Concat(path, file, ".frag");

            if (!File.Exists(vertshader) || !File.Exists(fragshader))
            {
                Debug.LogError("Shader Files Can't be found!");
                throw new Exception("Shader Files Can't be found!");
            }

            return new ShaderFile(File.ReadAllText(vertshader), File.ReadAllText(fragshader));
        }

        public static ImageFile LoadImage(string path, string file, string extensio)
        {
            if (!File.Exists(string.Concat(path, file, "." + extensio)))
            {
                Debug.LogError("Texture Files Can't be found At: " + string.Concat(path, file, "." + extensio));
                throw new Exception("Texture Files Can't be found At: " + string.Concat(path, file, "." + extensio));
            }

            using (var image = new Bitmap(string.Concat(path, file, "." + extensio)))
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                var data = image.LockBits( new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                return new ImageFile(data.Scan0, data.Width, data.Height);
            }
        }
    }
}

public struct ShaderFile
{
    public string _vertshader;
    public string _fragshader;

    public ShaderFile(string vertshader, string fragshader)
    {
        _vertshader = vertshader;
        _fragshader = fragshader;
    }
}

public struct ImageFile
{
    public int _width;
    public int _height;
    public IntPtr _ImgData;

    public ImageFile(IntPtr scan0, int width, int height) : this()
    {
        this._ImgData = scan0;
        this._width = width;
        this._height = height;
    }
}