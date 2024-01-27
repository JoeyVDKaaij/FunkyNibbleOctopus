using UnityEngine;
using Zenject;

namespace Game.Sprites
{
    public class CameraFacer : MonoBehaviour
    {
        [Inject]
        private UnityEngine.Camera _camera;

        private Transform _cameraTransform;

        private void Awake ()
        {
            _cameraTransform = _camera.transform;
        }

        private void LateUpdate ()
        {
            Quaternion rotation = _cameraTransform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}
