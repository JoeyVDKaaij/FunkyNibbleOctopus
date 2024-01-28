using UnityEngine;
using Zenject;

namespace Game.Character
{
    public class CharacterFlipper : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody rigidbody;
        
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;
        
        [SerializeField, Range(0, 10)]
        private float rotationSpeed = 1f;

        [SerializeField]
        private bool flip;

        [Inject]
        private UnityEngine.Camera _camera;

        private bool _reverseForward;

        private void Update ()
        {
            Vector3 cameraForward = _camera.transform.forward;
            if (flip)
                cameraForward = -cameraForward;

            float v = Vector3.Dot(rigidbody.velocity, _camera.transform.right);
            if (v > 0.01f)
                _reverseForward = true;
            else if (v < -0.01f)
                _reverseForward = false;
            if (_reverseForward)
                cameraForward = -cameraForward;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraForward, rotationAxis), Time.deltaTime * rotationSpeed);
        }
    }
}
