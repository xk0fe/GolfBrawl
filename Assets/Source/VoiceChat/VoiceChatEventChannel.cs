using Source.Events;
using UnityEngine;

namespace Source.VoiceChat
{
    [CreateAssetMenu(fileName = "VoiceChatEventChannel", menuName = "SO/Events/VoiceChatEventChannel", order = 0)]
    public class VoiceChatEventChannel : GenericEventChannel<VoiceChatUser>
    {
    }
}