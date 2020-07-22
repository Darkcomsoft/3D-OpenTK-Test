using OpenTK;
using BulletSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class RigidBody
    {
        public GameObject gameObject;
        public float _Mass;
        public bool _Static;
        private BulletSharp.RigidBody rigidBodyObject;
        public CollisionShape _Shape;

        public RigidBody(GameObject Object)
        {
            gameObject = Object;
            _Mass = 1;
            _Static = false;
            _Shape = new CapsuleShape(0.5f, 1);
            rigidBodyObject = LocalCreateRigidBody(_Mass, Matrix4.CreateTranslation(gameObject._transform._Position), _Shape);
        }

        public void Update()
        {
           
        }

        public BulletSharp.RigidBody LocalCreateRigidBody(float mass, Matrix4 startTransform, CollisionShape shape)
        {
            bool isDynamic = (mass != 0.0f);

            Vector3 localInertia = Vector3.Zero;
            if (isDynamic || _Static == false)
            {
                shape.CalculateLocalInertia(mass, out localInertia);
            }

            DefaultMotionState myMotionState = new DefaultMotionState(startTransform);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, shape, localInertia);
            BulletSharp.RigidBody body = new BulletSharp.RigidBody(rbInfo);

            Physics.AddRigidBody(body);

            return body;
        }

        public void Move(Vector3 direction)
        {
            rigidBodyObject.Translate(direction);
        }

        public void Force(Vector3 direction)
        {
            rigidBodyObject.ApplyForce(direction, gameObject._transform._Position);
        }

        public void OnDestroy()
        {
            Physics.RemoveRigidBody(rigidBodyObject);
            _Shape.Dispose();
            gameObject = null;
            _Shape = null;
        }

        public Matrix4 GetWorld { get { return rigidBodyObject.WorldTransform; } }
    }
}
