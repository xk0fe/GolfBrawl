using System.Collections;
using Fusion;
using Source.Ball;
using Source.Events;
using UnityEngine;

namespace Source.Clubs
{
    public class ClubController : NetworkBehaviour
    {
        [Networked]
        public float Power { get; set; }
        
        [Header("Events")]
        [SerializeField]
        private VoidEventChannel _onBallHitEventChannel;
        [SerializeField]
        private FloatEventChannel _onPowerChanged;
        [SerializeField]
        private ClubEventChannel _onClubChanged;
        
        [Header("View")]
        [SerializeField]
        private Vector3 _defaultRotation;
        [SerializeField]
        private Vector3 _chargedRotation;
        [SerializeField]
        private LayerMask _layerMask;

        [Header("Settings")]
        [SerializeField]
        private PlayerClubsScriptableObject _playerClubs;
        [SerializeField]
        private Transform _clubParent;
        
        private ClubView _selectedClubView;
        private ClubScriptableObject _selectedClub;
        
        private Collider[] _hitColliders;
        private int _lastDirectionMultiplier;

        private const int MAX_COLLIDERS = 10;
        private const int FORWARD_MULTIPLIER = 1;
        private const int BACKWARD_MULTIPLIER = -1;
        
        private void Awake()
        {
            _hitColliders = new Collider[MAX_COLLIDERS];
        }

        private void Start()
        {
            SetClub(_playerClubs.GetFirst());
        }

        private void Update()
        {
            if (!HasStateAuthority)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                StartCoroutine(ChargeHitCoroutine(isForward: true));
                _lastDirectionMultiplier = FORWARD_MULTIPLIER;
            } else if (Input.GetMouseButtonDown(1))
            {
                StopAllCoroutines();
                StartCoroutine(ChargeHitCoroutine(isForward: false));
                _lastDirectionMultiplier = BACKWARD_MULTIPLIER;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                RPC_SetPower(default);
                RPC_SetNextClub();
            } else if (Input.GetKeyDown(KeyCode.Q))
            {
                RPC_SetPower(default);
                RPC_SetPreviousClub();
            };
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_SetNextClub()
        {
            var club = _playerClubs.GetNextOrDefault(_selectedClub);
            RPC_SetClub(club.Name);
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_SetPreviousClub()
        {
            var club = _playerClubs.GetPreviousOrDefault(_selectedClub);
            RPC_SetClub(club.Name);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetClub(string clubName)
        {
            if (!_playerClubs.TryGetByName(clubName, out var club))
            {
                return;
            }
            
            SetClub(club);
        }
        
        private void SetClub(ClubScriptableObject club)
        {
            _selectedClub = club;
            _onClubChanged.Invoke(club);
            
            if (_selectedClubView != null)
            {
                Destroy(_selectedClubView.gameObject);
            }
            
            _selectedClubView = Instantiate(club.Prefab, _clubParent);
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_SetPower(float value)
        {
            if (Power == value)
            {
                return;
            }
            
            Power = value;
            _onPowerChanged.Invoke(Power / _selectedClub.MaxPower);
            OnPowerChanged();
        }
        
        private IEnumerator ChargeHitCoroutine(bool isForward = true)
        {
            RPC_SetPower(default);
            var targetButton = isForward ? 0 : 1;
            var directionMultiplier = isForward ? FORWARD_MULTIPLIER : BACKWARD_MULTIPLIER;   
            
            while (!Input.GetMouseButtonUp(targetButton))
            {
                RPC_SetPower(Mathf.Min(Power + _selectedClub.PowerIncreaseSpeed * Time.deltaTime, _selectedClub.MaxPower));
                yield return null; 
            }

            ReleaseHit(_clubParent.forward * directionMultiplier);
        }

        private void ReleaseHit(Vector3 direction)
        {
            var position = _clubParent.TransformPoint(_selectedClub.Prefab.HitPosition);
            var collidersCount = Physics.OverlapSphereNonAlloc(position, _selectedClub.HitRadius, _hitColliders, _layerMask);
            
            for (var i = 0; i < collidersCount; i++)
            {
                var hitCollider = _hitColliders[i];
                if (hitCollider.transform.TryGetComponent<BallObject>(out var ball))
                {
                    ball.RPC_ApplyForce(position, direction, Power, Power / _selectedClub.MaxPower <= _selectedClub.GroundedThreshold);
                    _onBallHitEventChannel.Invoke();
                }
            }

            RPC_SetPower(default);
        }

        public void OnPowerChanged()
        {
            if (_selectedClubView == null)
            {
                return;
            }
            
            var t = Power / _selectedClub.MaxPower;
            var rotation = Vector3.Lerp(_defaultRotation, _chargedRotation * _lastDirectionMultiplier, t);
            RPC_UpdateClubRotation(rotation);
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_UpdateClubRotation(Vector3 rotation)
        {
            RPC_ApplyClubRotation(rotation);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ApplyClubRotation(Vector3 rotation)
        {
            _selectedClubView.SetRotation(rotation);
        }
    }
}