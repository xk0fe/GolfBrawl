using UnityEngine;

namespace Source.Clubs
{
    [CreateAssetMenu(fileName = "PlayerClubsScriptableObject", menuName = "SO/Game/PlayerClubsScriptableObject", order = 0)]
    public class PlayerClubsScriptableObject : ScriptableObject
    {
        public ClubScriptableObject[] Clubs => _clubs;
        
        [SerializeField]
        private ClubScriptableObject[] _clubs;
        
        public bool TryGetByName(string name, out ClubScriptableObject club)
        {
            club = null;
            
            foreach (var iterationClub in _clubs)
            {
                if (iterationClub.Name != name) continue;
                club = iterationClub;
                return true;
            }
            return false;
        }
        
        public ClubScriptableObject GetFirst()
        {
            return _clubs[0];
        }

        public ClubScriptableObject GetNextOrDefault(ClubScriptableObject currentClub)
        {
            foreach (var club in _clubs)
            {
                if (club != currentClub) continue;
                var index = System.Array.IndexOf(_clubs, club);
                var nextIndex = (index + 1) % _clubs.Length;
                return _clubs[nextIndex];
            }
            
            return _clubs[0];
        }
        
        public ClubScriptableObject GetPreviousOrDefault(ClubScriptableObject currentClub)
        {
            foreach (var club in _clubs)
            {
                if (club != currentClub) continue;
                var index = System.Array.IndexOf(_clubs, club);
                var previousIndex = (index - 1 + _clubs.Length) % _clubs.Length;
                return _clubs[previousIndex];
            }
            
            return _clubs[0];
        }

        private void OnValidate()
        {
            if (_clubs == null || _clubs.Length == 0)
            {
                Debug.LogError($"No clubs found in {nameof(PlayerClubsScriptableObject)}");
            }
        }
    }
}