using Source.Character.SO;
using UnityEngine;

namespace Source.Character
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField]
        private CameraScriptableObject _settings;
        
        private Transform _target;
        private float _distance;

        private float _yaw = 0.0f;
        private float _pitch = 0.0f;

        private bool _hasTarget;

        private void Awake()
        {
            _hasTarget = _target != null;
            _distance = _settings.DistanceMin;
        }

        private void LateUpdate()
        {
            if (!_hasTarget)
            {
                return;
            }
            
            _yaw += Input.GetAxis("Mouse X") * _settings.RotationSpeed;
            _pitch -= Input.GetAxis("Mouse Y") * _settings.RotationSpeed;
            _pitch = Mathf.Clamp(_pitch, _settings.PitchMin, _settings.PitchMax);

            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                _distance -= scrollInput * _settings.ZoomSpeed;
            }
            _distance = Mathf.Clamp(_distance, _settings.DistanceMin, _settings.DistanceMax);

            var rotation = Quaternion.Euler(_pitch, _yaw, 0);
            var offset = rotation * new Vector3(0, 0, -_distance);

            transform.position = _target.position + offset;
            transform.LookAt(_target.position);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = target != null;
        }
    }
}