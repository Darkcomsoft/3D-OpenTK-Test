using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class Transform
    {
        public Vector3 _Position;
        public Quaternion _Rotation;
        public Vector3 _Size;

        public GameObject _gameObject;
        private List<Transform> _Child = new List<Transform>();

        public Transform() { }
        public Transform(Vector3 position, Quaternion rotation, Vector3 size)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = size;
        }
        public Transform(Vector3 position, Quaternion rotation)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = Vector3.One;
        }

        public Matrix4 PositionMatrix { get { return Matrix4.CreateTranslation(_Position); } }
        public Matrix4 RotationMatrix { get { return Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_Rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_Rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_Rotation.Z)); } }
        public Matrix4 GetTransformWorld 
        { 
            get 
            { 
                if (_gameObject.HaveRigid)
                {
                    return _gameObject.GetRigidBody().GetWorld * Matrix4.CreateScale(_Size);
                }
                else
                {
                    return RotationMatrix * PositionMatrix * Matrix4.CreateScale(_Size);
                }
            } 
        }

        public void SetChild(Transform Child)
        {
            if (!_Child.Contains(Child))
            {
                _Child.Add(Child);
            }
        }
        public void RemoveChild(Transform Child)
        {
            if (_Child.Contains(Child))
            {
                _Child.Remove(Child);
            }
        }
    }
}
