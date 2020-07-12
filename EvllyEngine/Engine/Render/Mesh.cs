using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class Mesh
    {
        public Material _Material;
        public float[] _vertices;
        public int[] _indices;
        public float[] _texCoords;
        public float[] _Colors;

        public Mesh() 
        {
            _vertices = new float[]
            {
                 //Position          Texture coordinates
                 0.5f,  0.5f, 0.0f, // top right
                 0.5f, -0.5f, 0.0f, // bottom right
                -0.5f, -0.5f, 0.0f, // bottom left
                -0.5f,  0.5f, 0.0f // top left
            };

            _indices = new int[]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            _Colors = new float[] {
                0.0f,1.0f,0.0f,1.0f,
                1.0f,0.0f,0.0f,1.0f,
                0.0f,0.0f,1.0f,1.0f
            };

            _texCoords = new float[] {
                1.0f, 1.0f,
                1.0f, 0.0f,
                0.0f, 0.0f,
                0.0f, 1.0f
            };
        }
        public Mesh(float[] vertices, int[] indices, float[] colors)
        {
            _vertices = vertices;
            _indices = indices;
            _Colors = colors;
        }
        public Mesh(float[] vertices, int[] indices)
        {
            _vertices = vertices;
            _indices = indices;
        }
    }
}
