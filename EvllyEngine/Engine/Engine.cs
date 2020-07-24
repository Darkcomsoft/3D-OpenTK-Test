using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Threading;

namespace EvllyEngine
{
    public class Engine : GameWindow
    {
        public static Engine Instance;
        public AssetsManager _assetsManager;
        private UIManager _UIManager;
        private Physics _physics;
        private int FPS;

        public bool EngineReady;

        public int GetFPS { get { return FPS; } }

        /// <summary>
        /// All objects with all object information like, transform, meshrender, colliders etc. this for any one wants obj information
        /// </summary>
        public List<GameObject> GameObjects = new List<GameObject>();
        /// <summary>
        /// all objects with render stuff, like mesh, particle, sprites etc. but this is only for render loop
        /// </summary>
        //public List<T> RenderObjects = new List<T>();

        public Engine(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { Instance = this; }

        public Action<FrameEventArgs> DrawUpdate;
        GameObject LaucherPed;
        protected override void OnLoad(EventArgs e)
        {
            //Load the Window/pre Graphics Stuff//
            VSync = VSyncMode.Off;
            WindowBorder = WindowBorder.Fixed;

            GL.ClearColor(Color4.DeepSkyBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Front);
            GL.Viewport(0, 0, Width, Height);
            //Load the Engine Stuff//
            _assetsManager = new AssetsManager();
            _UIManager = new UIManager();
            _physics = new Physics();
            SceneManager.LoadDontDestroyScene();
            SceneManager.LoadDefaultScene();
            
            GameObject Player = GameObject.Instantiate("Player", 1);
            Player._transform.Position = new Vector3(0, 50, 0);
            Player.AddRigidBody();
            Player.AddScript(new PlayerEntity());

            GameObject camobj = GameObject.Instantiate("Camera", 1);
            camobj._transform.Position = new Vector3(0,1,0);
            camobj.AddCamera();

            camobj._transform.SetChild(Player._transform);

           GameObject World = GameObject.Instantiate(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), 1);
            World.AddScript(new World());

            for (int x = 0; x < 2; x++)
            {
                if (x == 0)
                {
                    GameObject obj = GameObject.Instantiate(new Vector3(x, 0, 0), Quaternion.Identity, 1);
                    MeshRender mesh = obj.AddMeshRender(new MeshRender(obj, AssetsManager.instance.GetMesh("Cube"), new Shader(AssetsManager.instance.GetShader("Default"))));
                    //obj.AddBoxCollider(new BoxCollider(obj, new Vector3(100,1,100)));
                    obj.AddMeshCollider();
                    mesh._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("NewGrassTeste", "png")));
                }
                else
                {
                    GameObject obj = GameObject.Instantiate(new Vector3(x, 0.01f, 0), Quaternion.Identity, 1);
                    MeshRender meshh = obj.AddMeshRender(new MeshRender(obj, AssetsManager.instance.GetMesh("Cube2"), new Shader(AssetsManager.instance.GetShader("Default"))));
                    meshh._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("RockBrick_01", "png")));
                    obj.AddMeshCollider();
                }
            }

            for (int i = 0; i < 1; i++)
            {
                GameObject wall = GameObject.Instantiate(new Vector3(0, 0, -100), new Quaternion(90,0,0,0), 1);
                MeshRender mesh = wall.AddMeshRender(new MeshRender(wall, AssetsManager.instance.GetMesh("Cube"), new Shader(AssetsManager.instance.GetShader("Default"))));
                wall.AddBoxCollider(new BoxCollider(wall, new Vector3(100, 1, 100)));
                mesh._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("NewGrassTeste", "png")));
            }

            GameObject obj2 = GameObject.Instantiate(new Vector3(0, 20, 0), new Quaternion(-90,0,0,0), 1);
            MeshRender mesh2 = obj2.AddMeshRender(new MeshRender(obj2, _assetsManager.GetErrorMesh, new Shader(AssetsManager.instance.GetShader("Default"))));
            mesh2._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("RedTexture", "png")));


            LaucherPed = GameObject.Instantiate(new Vector3(50,1,0), new Quaternion(0, 0, 0, 0), 1);
            LaucherPed.AddScript(new ScriptTest());
            MeshRender meshLaucherPed = LaucherPed.AddMeshRender(new MeshRender(LaucherPed, new Mesh(AssetsManager.instance.GetMesh("Monkey")), new Shader(AssetsManager.instance.GetShader("Default"))));
            meshLaucherPed._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("woodplank01", "png")));
            LaucherPed.AddRigidBody();
            meshLaucherPed.Transparency = true;
            EngineReady = true;
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Input.GetKey(Key.C))
            {
                GameObject monkeyRigid = GameObject.Instantiate(Camera.Main.gameObject._transform.Position, new Quaternion(0, 0, 0, 0), 1);
                ///monkeyRigid.AddScript(new ScriptTest());
                MeshRender meshLaucherPed = monkeyRigid.AddMeshRender(new MeshRender(monkeyRigid, new Mesh(AssetsManager.instance.GetMesh("Monkey")), new Shader(AssetsManager.instance.GetShader("Default"))));
                meshLaucherPed._shader.AddTexture(new Texture(AssetsManager.instance.GetTexture("woodplank01", "png")));
                RigidBody body = monkeyRigid.AddRigidBody();
                meshLaucherPed.Transparency = true;
            }

            if (Input.GetKeyDown(Key.F11))
            {
                if (WindowState == WindowState.Fullscreen)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Fullscreen;
                }
            }

            if (Input.GetKeyDown(Key.Escape))
            {
                Exit();
            }
            _UIManager.Text = "EvllyEngine FPS: " + FPS + " Tick: " + Time._Tick % 60 + " Objects: " + GameObjects.Count + " CameraPosition: " + Camera.Main.gameObject._transform.Position.ToString() + " CameraRotation: " + Camera.Main.gameObject._transform.Rotation.ToString();
            Time._Time = (float)e.Time;
            Time._Tick++;
            _physics.UpdatePhisics((float)e.Time);
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.CullFace);
                FPS = (int)(1f / e.Time);
                DrawUpdate.Invoke(e);
            GL.Disable(EnableCap.CullFace);
                if (EngineReady)
                {
                    //_UIManager.DrawUI();
                }
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Console.WriteLine("MousePressed: "+ e.Button);
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Console.WriteLine("MouseUnPressed: " + e.Button);
            base.OnMouseUp(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Console.WriteLine("MouseWheel: " + e.Delta);
            base.OnMouseWheel(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Input.GetMouse = e;
            base.OnMouseMove(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            for (int i = 0; i < SceneManager.GetSceneArray.Count; i++)
            {
                SceneManager.GetSceneArray[i].OnUnloadScene();
            }

            _assetsManager.UnloadAll();
            _UIManager.Dispose();
            _physics.Dispose();
            base.OnUnload(e);
        }

        public void RemoveObject(GameObject obj)
        {
            if (GameObjects.Contains(obj))
            {
                obj.OnDestroy();
                GameObjects.Remove(obj);
                obj = null;
            }
        }
        public void AddObject(GameObject obj)
        {
            GameObjects.Add(obj);
        }
    }

    public static class Time
    {
        public static float _Time;
        public static float _Tick;
    }

    public static class Cursor
    {
        public static Vector2 MousePosition;
        protected void ResetCursorPosition()
        {
            Mouse.SetPosition(Engine.Instance.Width / 2, Engine.Instance.Height / 2);
            _lastMousePos = MousePosition;
        }

        protected void LockMouse()
        {
            _lockMouse = true;
            _origCursorPosition = Cursor.Position;
            CursorVisible = false;
            ResetCursorPosition();
        }

        protected void UnlockMouse()
        {
            _lockMouse = false;
            CursorVisible = true;
            Cursor.Position = _origCursorPosition;
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_lockMouse) LockMouse();
        }

        void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_lockMouse) UnlockMouse();
                else Exit();
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (_lockMouse)
            {
                var mouseDelta = Cursor.Position - new Size(_lastMousePos);
                if (mouseDelta != Point.Empty)
                {
                    _lookDir.X += mouseDelta.X * _mouseSensitivity;
                    _lookDir.Y -= mouseDelta.Y * _mouseSensitivity;
                    ResetCursorPosition();
                }
            }

            var target = _pos + new Vector3((float)Math.Cos(_lookDir.X), (float)Math.Sin(_lookDir.Y / 2), (float)Math.Sin(_lookDir.X));
            _viewMat = Matrix4.LookAt(_pos, target, _up);
            _projUniform.Mat4(_viewMat * _projMat);
        }
    }

    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Point : IEquatable<Point>
    {
        #region Private Fields

        private static readonly Point zeroPoint = new Point();

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Point"/>.
        /// </summary>
        public int X;

        /// <summary>
        /// The y coordinate of this <see cref="Point"/>.
        /// </summary>
        public int Y;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a <see cref="Point"/> with coordinates 0, 0.
        /// </summary>
        public static Point Zero
        {
            get { return zeroPoint; }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a point with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a point with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Point(int value)
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="value1">Source <see cref="Point"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Point"/> on the right of the add sign.</param>
        /// <returns>Sum of the points.</returns>
        public static Point operator +(Point value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        /// <summary>
        /// Subtracts a <see cref="Point"/> from a <see cref="Point"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Point"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Point"/> on the right of the sub sign.</param>
        /// <returns>Result of the subtraction.</returns>
        public static Point operator -(Point value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }

        /// <summary>
        /// Multiplies the components of two points by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Point"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Point"/> on the right of the mul sign.</param>
        /// <returns>Result of the multiplication.</returns>
        public static Point operator *(Point value1, Point value2)
        {
            return new Point(value1.X * value2.X, value1.Y * value2.Y);
        }

        /// <summary>
        /// Divides the components of a <see cref="Point"/> by the components of another <see cref="Point"/>.
        /// </summary>
        /// <param name="source">Source <see cref="Point"/> on the left of the div sign.</param>
        /// <param name="divisor">Divisor <see cref="Point"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the points.</returns>
        public static Point operator /(Point source, Point divisor)
        {
            return new Point(source.X / divisor.X, source.Y / divisor.Y);
        }

        /// <summary>
        /// Compares whether two <see cref="Point"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="Point"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="Point"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="Point"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="Point"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="Point"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Point) && Equals((Point)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Point other)
        {
            return ((X == other.X) && (Y == other.Y));
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Point"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Point"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }

        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Point"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="Point"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Vector2"/> representation for this object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// Deconstruction method for <see cref="Point"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        #endregion
    }
}
