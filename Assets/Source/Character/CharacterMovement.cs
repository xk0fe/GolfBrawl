using Fusion;
using Source.Character.SO;
using UnityEngine;
using NetworkInput = Source.GameInput.NetworkInput;

namespace Source.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : NetworkBehaviour
    {
        [SerializeField]
        private CharacterScriptableObject _settings;
        [SerializeField]
        private CharacterController _characterController;
        
        private Vector3 _moveDirection;
        private Vector3 _velocity;
        private Transform _cameraTransform;
        private float _rotationSpeed;

        private const float GROUNDED_POSITION_Y = -2f;
        
        private void Awake()
        {
            _rotationSpeed = _settings.RotationSpeed;
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput<NetworkInput>(out var input))
            {
                return;
            }
            
            var deltaTime = Runner.DeltaTime;
            var isGrounded = _characterController.isGrounded;
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = GROUNDED_POSITION_Y;
            }

            var forward = _cameraTransform.forward;
            var right = _cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            _moveDirection = (forward * input.Vertical + right * input.Horizontal).normalized;

            if (_moveDirection.magnitude >= 0.1f)
            {
                var targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationSpeed,
                    0.1f * deltaTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                var move = _moveDirection * (_settings.MoveSpeed * deltaTime);
                _characterController.Move(move);
            }

            var gravity = Physics.gravity.y;
            if (isGrounded && input.Jump)
            {
                _velocity.y = Mathf.Sqrt(_settings.JumpHeight * -2f * gravity);
            }

            _velocity.y += gravity * deltaTime;
            _characterController.Move(_velocity * deltaTime);
        }

        public override void Spawned()
        {
            if (!HasStateAuthority)
            {
                return;
            }

            if (Camera.main == null)
            {
                return;
            }
            
            _cameraTransform = Camera.main.transform;

            if (_cameraTransform.TryGetComponent<CharacterCamera>(out var characterCamera))
            {
                characterCamera.SetTarget(transform);
            }
        }
        
        private void RemoveCameraTarget()
        {
            if (_cameraTransform == null)
            {
                return;
            }
            
            if (_cameraTransform.TryGetComponent<CharacterCamera>(out var characterCamera))
            {
                characterCamera.SetTarget(null);
            }
        }
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            RemoveCameraTarget();
        }
    }
}