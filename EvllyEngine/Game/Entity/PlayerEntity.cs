using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class PlayerEntity : ScriptBase
    {
        public RigidBody body;
        public float MoveSpeed = 0.6f;

        public override void Start()
        {
            body = gameObject.GetRigidBody();
            base.Start();
        }

        public override void Update()
        {
            var moveVector = new Vector3(0, 0, 0);


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
                MoveSpeed = 5;
            }
            else
            {
                MoveSpeed = 3;
            }

            AddToCameraPosition(moveVector);
            base.Update();
        }

        private Vector3 moveVector;
        private Vector3 inputDirection;

        public Vector3 Move(Vector3 directionVector)
        {
            inputDirection = directionVector;
            if (directionVector != Vector3.Zero)
            {
                var directionLength = directionVector.Length;
                directionVector = directionVector / directionLength;
                directionLength = Math.Min(1, directionLength);
                directionLength = directionLength * directionLength;
                directionVector = directionVector * directionLength;
            }

            Quaternion rotation = gameObject._transform.Rotation;

            /*Vector3 angle = rotation.eulerAngles;
            angle.X = 0;
            angle.Z = 0;
            MathHelper.DegreesToRadians
            rotation.eulerAngles = angle;*/
            return rotation * directionVector;
        }

        private void AddToCameraPosition(Vector3 moveVector)
        {
            var camRotation = Matrix3.CreateRotationX(gameObject._transform.Rotation.X) * Matrix3.CreateRotationY(gameObject._transform.Rotation.Y) * Matrix3.CreateRotationZ(gameObject._transform.Rotation.Z);
            var rotatedVector = Vector3.Transform(moveVector, camRotation);
            //gameObject.GetRigidBody().Move(rotatedVector * moveSpeed);

            body.Move(rotatedVector * MoveSpeed);
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

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
