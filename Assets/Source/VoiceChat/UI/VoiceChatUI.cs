using System.Collections.Generic;
using UnityEngine;

namespace Source.VoiceChat.UI
{
    public class VoiceChatUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatStarted;
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatEnded;
        
        [Header("Settings")]
        [SerializeField]
        private VoiceChatElementUI _voiceChatElementPrefab;
        [SerializeField]
        private Transform _voiceChatElementsParent;
        
        private Dictionary<string, VoiceChatElementUI> _activeVoiceChatElements = new();

        private void Awake()
        {
            _onVoiceChatStarted.OnEventRaised += OnVoiceChatStarted;
            _onVoiceChatEnded.OnEventRaised += OnVoiceChatEnded;
        }

        private void OnVoiceChatStarted(VoiceChatUser user)
        {
            if (_activeVoiceChatElements.ContainsKey(user.UserId))
            {
                return;
            }
            
            var voiceChatElement = Instantiate(_voiceChatElementPrefab, _voiceChatElementsParent);
            voiceChatElement.SetAvatar(user.Avatar);
            voiceChatElement.SetUsername(user.Username);
            _activeVoiceChatElements.Add(user.UserId, voiceChatElement);
        }

        private void OnVoiceChatEnded(VoiceChatUser user)
        {
            if (!_activeVoiceChatElements.TryGetValue(user.UserId, out var element))
            {
                return;
            }
            
            Destroy(element.gameObject);
            _activeVoiceChatElements.Remove(user.UserId);
        }

        private void OnDestroy()
        {
            _onVoiceChatStarted.OnEventRaised -= OnVoiceChatStarted;
            _onVoiceChatEnded.OnEventRaised -= OnVoiceChatEnded;
        }
    }
}