using System.Collections.Generic;
using Source.Clubs;
using Source.Events;
using UnityEngine;

namespace Source.UI
{
    public class ClubDataUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] 
        private ClubEventChannel _onClubChanged;

        [Header("View")]
        [SerializeField]
        private Color _activeClub;
        [SerializeField]
        private Color _inactiveClub;
        
        [Header("Settings")]
        [SerializeField]
        private PlayerClubsScriptableObject _playerClubs;
        [SerializeField]
        private ClubElementUI _clubNamePrefab;
        [SerializeField]
        private Transform _clubNameParent;

        private List<ClubElementUI> _clubNameInstances;
        
        private void Awake()
        {
            _onClubChanged.OnEventRaised += UpdateClub;
            
            _clubNameInstances = new List<ClubElementUI>();
            foreach (var club in _playerClubs.Clubs)
            {
                var instance = Instantiate(_clubNamePrefab, _clubNameParent);
                instance.SetClub(club);
                instance.SetColor(_inactiveClub);
                _clubNameInstances.Add(instance);
            }
        }

        public void UpdateClub(ClubScriptableObject currentClub)
        {
            foreach (var instance in _clubNameInstances)
            {
                var isCurrent = instance.Club == currentClub;
                instance.SetColor(isCurrent ? _activeClub : _inactiveClub);
            }
        }
        
        private void OnDestroy()
        {
            _onClubChanged.OnEventRaised -= UpdateClub;
        }
    }
}