using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.VoiceChat.UI
{
    public class VoiceChatElementUI : MonoBehaviour
    {
        [SerializeField]
        private Image _avatar;
        [SerializeField]
        private TextMeshProUGUI _username;
        
        public void SetAvatar(Sprite avatar)
        {
            _avatar.sprite = avatar;
        }
        
        public void SetUsername(string username)
        {
            _username.text = username;
        }
    }
}