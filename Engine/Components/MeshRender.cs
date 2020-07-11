using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class MeshRender
    {
        public GameObject gameObject;
        public Mesh _mesh;
        Shader _shader;
        private int IBO;
        private int VAO;

        public MeshRender(GameObject obj)
        {
            gameObject = obj;
            _mesh = new Mesh();
            _shader = new Shader("Default");

            if (_shader != null)
            {
                _shader.Use();
            }

            VAO = GL.GenVertexArray();
            IBO = GL.GenBuffer();
            int vbo = GL.GenBuffer();
            int dbo = GL.GenBuffer();
            int tbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh._indices.Length * sizeof(int), _mesh._indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._vertices.Length * sizeof(float), _mesh._vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._Colors.Length * sizeof(float), _mesh._Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._texCoords.Length * sizeof(float), _mesh._texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);
        }

        public void Draw(FrameEventArgs e)
        {
            _shader.GetTexture.Use();
            _shader.Use();

            _shader.SetMatrix4("world", gameObject._transform.GetTransformWorld);
            _shader.SetMatrix4("view", Camera.Main.viewMatrix);
            _shader.SetMatrix4("projection", Camera.Main._projection);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.DrawElements(BeginMode.Triangles, _mesh._indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void OnDestroy()
        {
            _shader.Delete();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);

            GL.DeleteBuffer(IBO);
            GL.DeleteVertexArray(VAO);

            _mesh = null;
            _shader = null;
        }

        
    }
}
