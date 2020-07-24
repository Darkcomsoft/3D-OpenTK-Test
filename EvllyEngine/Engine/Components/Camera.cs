using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class Camera
    {
        public GameObject gameObject;
        public static Camera Main;

        public float _fildOfView = 60;
        public float _nearPlane = 0.1f;
        public float _farPlane = 1000;

        private float _aspectRatio;

        //Camera Location
        public Matrix4 viewMatrix;
        //camera Lens
        public Matrix4 _projection;


        public Vector2 mouseVector;
        public Vector3 finalTarget;
        public Vector3 target;
		public Vector3 mouseRotationBuffer;
		public const float rotationSpeed = 4f;
		public const float moveSpeed = 3.0f;
		public float MoveSpeed = 0.6f;
        public bool MouseLook = true;

        MouseState laststate;


        public Camera(GameObject obj)
        {
            gameObject = obj;
            Main = this;
            _aspectRatio = (float)Engine.Instance.Width / (float)Engine.Instance.Height;

            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fildOfView), _aspectRatio, _nearPlane, _farPlane);

            Engine.Instance.CursorVisible = false;
            UpdateViewMatrix();
        }

        public void Update(float time)
        {
            _aspectRatio = (float)Engine.Instance.Width / (float)Engine.Instance.Height;
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fildOfView), _aspectRatio, _nearPlane, _farPlane);

            /*Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(gameObject._transform._Position - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);*/

            MouseState state = Mouse.GetState();
            var moveVector = new Vector3(0, 0, 0);

            var curMousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            if (curMousePos != Point.Zero)
            {
                mouseVector = (curMousePos - Point.Zero).ToVector2();
            }

            if (Input.GetKeyDown(Key.P))
            {
                if (MouseLook)
                {
                    MouseLook = false;
                    Engine.Instance.CursorVisible = true;
                }
                else
                {
                    MouseLook = true;
                    Engine.Instance.CursorVisible = false;
                }
            }

            if (Input.GetKey(Key.W))
            {
                moveVector.Z += MoveSpeed;
            }
            if (Input.GetKey(Key.S))
            {
                moveVector.Z -= MoveSpeed;
            }
            if (Input.GetKey(Key.A))
            {
                moveVector.X += MoveSpeed;
            }
            if (Input.GetKey(Key.D))
            {
                moveVector.X -= MoveSpeed;
            }

            if (Input.GetKey(Key.Q))
            {
                moveVector.Y -= MoveSpeed;
            }
            if (Input.GetKey(Key.E))
            {
                moveVector.Y += MoveSpeed;
            }

            if (Input.GetKey(Key.ShiftLeft))
            {
                MoveSpeed = 200;
            }
            else
            {
                MoveSpeed = 100;
            }

            if (MouseLook)
            {
                if (state != laststate)
                {
                    mouseRotationBuffer.X -= 0.1f * Input.GetMouse.XDelta * rotationSpeed * (float)time;
                    mouseRotationBuffer.Y -= 0.1f * Input.GetMouse.YDelta * rotationSpeed * (float)time;

                    if (mouseRotationBuffer.Y < MathHelper.DegreesToRadians(-75.0f))
                    {
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.DegreesToRadians(-75.0f));
                    }

                    if (mouseRotationBuffer.Y > MathHelper.DegreesToRadians(75.0f))
                    {
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.DegreesToRadians(75.0f));
                    }

                    gameObject._transform.Rotation = new Quaternion(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.DegreesToRadians(-75.0f), MathHelper.DegreesToRadians(75.0f)), 0, 0, 0);
                    gameObject._transform.Root.Rotation = new Quaternion(0, WrapAngle(mouseRotationBuffer.X), 0, 0);
                }
            }
            laststate = state;
            AddToCameraPosition(moveVector * (float)time);
            UpdateViewMatrix();
		}

        private void AddToCameraPosition(Vector3 moveVector)
        {
            var camRotation = Matrix3.CreateRotationX(gameObject._transform.Rotation.X) * Matrix3.CreateRotationY(gameObject._transform.Rotation.Y) * Matrix3.CreateRotationZ(gameObject._transform.Rotation.Z);
            var rotatedVector = Vector3.Transform(moveVector, camRotation);
            //gameObject.GetRigidBody().Move(rotatedVector * moveSpeed);
        }

        private void UpdateViewMatrix()
        {
            var camRotation = Matrix3.CreateRotationX(gameObject._transform.Rotation.X) * Matrix3.CreateRotationY(gameObject._transform.Rotation.Y) * Matrix3.CreateRotationZ(gameObject._transform.Rotation.Z);

            var camOriginalTarget = new Vector3(0, 0, 1);
            var camRotatedTarget = Vector3.Transform(camOriginalTarget, camRotation);
            finalTarget = new Vector3(gameObject._transform.Position) + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

            viewMatrix = Matrix4.LookAt(gameObject._transform.Position, finalTarget, camRotatedUpVector);
        }

        public void OnDestroy()
        {

        }

        public static float WrapAngle(float angle)
        {
            if ((angle > -MathHelper.Pi) && (angle <= MathHelper.Pi))
                return angle;
            angle %= MathHelper.TwoPi;
            if (angle <= -MathHelper.Pi)
                return angle + MathHelper.TwoPi;
            if (angle > MathHelper.Pi)
                return angle - MathHelper.TwoPi;
            return angle;
        }
    }
}