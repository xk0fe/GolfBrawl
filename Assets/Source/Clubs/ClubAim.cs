using Fusion;
using Source.Events;
using UnityEngine;

namespace Source.Clubs
{
    public class ClubAim : NetworkBehaviour
    {
        [SerializeField]
        private ClubEventChannel _onClubChanged;
        [SerializeField]
        private FloatEventChannel _onPowerChanged;

        [SerializeField]
        private Transform _aim;
        [SerializeField]
        private Transform _clubParent;
        [SerializeField]
        private LayerMask _layerMask;
        
        private Vector3 GROUND_OFFSET = new(0, .01f, 0);

        private ClubScriptableObject _currentClub;

        private void Awake()
        {
            _onClubChanged.OnEventRaised += ChangeClub;
            _onPowerChanged.OnEventRaised += UpdatePower;
            _aim.gameObject.SetActive(false);
        }
        
        private void ChangeClub(ClubScriptableObject club)
        {
            _aim.localScale = Vector3.one * (club.HitRadius * 2);

            _currentClub = club;
            UpdatePosition();
        }
        
        private void UpdatePower(float power)
        {
            if (!HasStateAuthority)
            {
                return;
            }
            
            var hasPower = power > 0;

            if (hasPower != _aim.gameObject.activeSelf)
            {
                _aim.gameObject.SetActive(hasPower);
            }
            
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (_currentClub == null)
            {
                return;
            }
            
            var position = _clubParent.TransformPoint(_currentClub.Prefab.HitPosition);
            if (Physics.Raycast(position, Vector3.down, out var hit, _layerMask))
            {
                _aim.position = hit.point + GROUND_OFFSET;
            }
            else
            {
                _aim.position = position;
            }
        }
        
        private void OnDestroy()
        {
            _onClubChanged.OnEventRaised -= ChangeClub;
            _onPowerChanged.OnEventRaised -= UpdatePower;
        }
    }
}