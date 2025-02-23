﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class Shader
    {
        private Texture _Texture;
        public int _shaderProgram;
        private bool IsValid = true;

        private Dictionary<string, int> _uniformLocations;

        public Shader(ShaderFile fileShader) 
        {
            _uniformLocations = new Dictionary<string, int>();
            _Texture = new Texture(AssetsManager.instance.GetTexture("devTexture", "jpg"));
            CompileShader(fileShader);
        }

        public Shader(ShaderFile fileShader, Texture tex0)
        {
            _uniformLocations = new Dictionary<string, int>();
            _Texture = tex0;
            CompileShader(fileShader);
        }

        public void Delete()
        {
            _Texture.Delete();
            _Texture = null;
            if (IsValid)
            {
                GL.DeleteProgram(_shaderProgram);
                IsValid = false;
            }
        }

        public void Use()
        {
            if (IsValid)
            {
                GL.UseProgram(_shaderProgram);
            }
        }

        private int CompileShader(ShaderFile shaderData)
        {
            int vert_shader = GL.CreateShader(ShaderType.VertexShader);
            int frag_shader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vert_shader, shaderData._vertshader);
            GL.ShaderSource(frag_shader, shaderData._fragshader);

            GL.CompileShader(vert_shader);
            GL.CompileShader(frag_shader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vert_shader);
            GL.AttachShader(program, frag_shader);
            GL.LinkProgram(program);

            GL.DeleteShader(vert_shader);
            GL.DeleteShader(frag_shader);

            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(program, i, out _, out _);
                var location = GL.GetUniformLocation(program, key);
                _uniformLocations.Add(key, location);
            }

            GL.ValidateProgram(program);
            if (GL.GetError() != ErrorCode.NoError)
            {
                Debug.LogError("SHADER: Failed to create program : " + GL.GetError());
                GL.DeleteProgram(program);
            }
            _shaderProgram = program;
            return program;
        }

        public void AddTexture(Texture texture)
        {
            _Texture = texture;
        }

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetInt(string name, int data)
        {
            GL.UseProgram(_shaderProgram);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(_shaderProgram);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(_shaderProgram);
            GL.Uniform3(_uniformLocations[name], data);
        }

        public Texture GetTexture { get { return _Texture; } }
    }
}
