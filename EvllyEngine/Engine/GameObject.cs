using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class GameObject : ObjElement
    {
        public readonly int _SceneID;
        public string _name;
        public Transform _transform;
        private MeshRender _MeshRender;
        private Camera _camera;

        public GameObject(int scene) 
        {
            _transform = new Transform(new Vector3(0, 0, 0), Quaternion.Identity, new Vector3(1, 1, 1));
            _transform._gameObject = this;
            _SceneID = scene;

            StartObject();
        }
        public GameObject(string name, int scene) 
        {
            _transform = new Transform(new Vector3(0,0,0), Quaternion.Identity, new Vector3(1, 1, 1));
            _transform._gameObject = this;
            _name = name; _SceneID = scene;

            StartObject();
        }
        public GameObject(Vector3 position, Quaternion rotation, int scene) 
        { 
            _transform = new Transform(position, rotation, new Vector3(1, 1, 1));
            _transform._gameObject = this;
            _SceneID = scene;

            StartObject();
        }
        public GameObject(Transform parent)
        {
            _transform = new Transform(parent._Position, parent._Rotation, new Vector3(1, 1, 1));
            _transform._gameObject = this;
            parent.SetChild(_transform);
            _SceneID = parent._gameObject._SceneID;

            StartObject();
        }
        public GameObject(GameObject prefab, Vector3 position, Quaternion rotation, int scene) 
        { 
            _transform = new Transform(position, rotation, new Vector3(1, 1, 1));
            _transform._gameObject = this;
            _SceneID = scene;
            _name = prefab._name;

            StartObject();
        }

        private void StartObject()
        {

        }

        /// <summary>
        /// Script UpdateFreame
        /// </summary>
        public void Update(FrameEventArgs e)
        {
            if (_camera != null)
            {
                _camera.Update((float)e.Time);
            }
        }


        public void Draw(FrameEventArgs e)
        {
            if (_MeshRender != null)
            {
                _MeshRender.Draw(e);
            }
        }

        public void OnDestroy()
        {
            if (_MeshRender != null)
            {
                _MeshRender.OnDestroy();
            }

            if (_camera != null)
            {
                _camera.OnDestroy();
            }

            _transform = null;
            _MeshRender = null;
            _camera = null;
           
        }

        public static GameObject Instantiate(int scene)
        {
            GameObject obj = new GameObject("NewGameObject", scene);
            Engine.Instance.AddObject(obj);
            return obj;
        }
        public static GameObject Instantiate(string name, int scene)
        {
            GameObject obj = new GameObject(name, scene);
            Engine.Instance.AddObject(obj);
            return obj;
        }
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, int scene)
        {
            GameObject obj = new GameObject(prefab, position, rotation, scene);
            Engine.Instance.AddObject(obj);
            return obj;
        }
        public static GameObject Instantiate(Transform parent)
        {
            GameObject obj = new GameObject(parent);
            Engine.Instance.AddObject(obj);
            return obj;
        }

        public Camera AddCamera(Camera camera)
        {
            _camera = camera;
            return _camera;
        }
        public Camera AddCamera()
        {
            Camera cam = new Camera(this);
            _camera = cam;
            return _camera;
        }

        public MeshRender AddMeshRender(MeshRender meshrender)
        {
            _MeshRender = meshrender;
            return _MeshRender;
        }
        public MeshRender AddMeshRender()
        {
            MeshRender meshrender = new MeshRender(this);
            _MeshRender = meshrender;
            return _MeshRender;
        }

        public static void Destroy(GameObject gameObject)
        {
            Engine.Instance.RemoveObject(gameObject);
        }
    }

    public class ObjElement
    {

    }
}
