using Photon.Voice.Unity;
using Source.Network;
using UnityEngine;

namespace Source.VoiceChat
{
    public class VoiceChatController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private Recorder _recorder;
        
        [Header("Events")]
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatStarted;
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatEnded;

        private void Awake()
        {
            _onVoiceChatStarted.OnEventRaised += OnVoiceChatStarted;
            _onVoiceChatEnded.OnEventRaised += OnVoiceChatEnded;
        }
        
        private void OnVoiceChatStarted(VoiceChatUser user)
        {
            if (LocalPlayerService.GetLocalPlayerNickname() != user.Username)
            {
                return;
            }
            _recorder.RecordingEnabled = true;
            _recorder.TransmitEnabled = true;
        }
        
        private void OnVoiceChatEnded(VoiceChatUser user)
        {
            if (LocalPlayerService.GetLocalPlayerNickname() != user.Username)
            {
                return;
            }
            _recorder.RecordingEnabled = false;
            _recorder.TransmitEnabled = false;
        }
        
        private void OnDestroy()
        {
            _onVoiceChatStarted.OnEventRaised -= OnVoiceChatStarted;
            _onVoiceChatEnded.OnEventRaised -= OnVoiceChatEnded;
        }
    }
}