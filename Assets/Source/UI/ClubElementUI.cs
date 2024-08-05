using Source.Clubs;
using TMPro;
using UnityEngine;

namespace Source.UI
{
    public class ClubElementUI : MonoBehaviour
    {
        public ClubScriptableObject Club => _club;
        
        [SerializeField]
        private TextMeshProUGUI _name;

        private ClubScriptableObject _club;
        
        public void SetClub(ClubScriptableObject club)
        {
            _club = club;
            _name.text = club.Name;
        }

        public void SetColor(Color color)
        {
            _name.color = color;
        }
    }
}