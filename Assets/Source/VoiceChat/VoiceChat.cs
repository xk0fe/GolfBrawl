using Fusion;
using Source.Network;
using Source.Player;
using UnityEngine;

namespace Source.VoiceChat
{
    public class VoiceChat : NetworkBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatStarted;
        [SerializeField]
        private VoiceChatEventChannel _onVoiceChatEnded;

        private VoiceChatUser _localVoiceUser;

        private void Start()
        {
            if (!HasStateAuthority)
            {
                return;
            }

            _localVoiceUser = new VoiceChatUser
            {
                Username = NetworkService.Instance().LocalPlayerService.LocalPlayer.Name,
                UserId = Runner.UserId
            };
        }

        private void Update()
        {
            if (!HasStateAuthority)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                RPC_SetActiveVoiceChat(_localVoiceUser.Username, _localVoiceUser.UserId, true);
            }
            
            if (Input.GetKeyUp(KeyCode.V))
            {
                RPC_SetActiveVoiceChat(_localVoiceUser.Username, _localVoiceUser.UserId, false);
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_SetActiveVoiceChat(string username, string userId, bool isActive)
        {
            RPC_SetActiveVoiceChatElement(username, userId, isActive);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetActiveVoiceChatElement(string username, string userId, bool setActive)
        {
            var user = new VoiceChatUser
            {
                Username = username,
                UserId = userId
            };
            if (setActive)
            {
                _onVoiceChatStarted.Invoke(user);
            }
            else
            {
                _onVoiceChatEnded.Invoke(user);
            }
        }
    }
}