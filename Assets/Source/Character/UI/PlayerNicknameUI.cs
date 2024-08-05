using Fusion;
using Source.Network;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace Source.Character.UI
{
    public class PlayerNicknameUI : NetworkBehaviour
    {
        [SerializeField]
        private TextMeshPro _nicknameText;
        [SerializeField]
        private LookAtConstraint _lookAtConstraint;
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_ShareNickname(string nickname)
        {
            RPC_UpdateNickname(nickname);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_UpdateNickname(string nickname)
        {
            _nicknameText.text = nickname;
        }

        private void Start()
        {
            SetCamera();
            if (!HasStateAuthority)
            {
                return;
            }
            RPC_ShareNickname(LocalPlayerService.GetLocalPlayerNickname());
        }

        private void SetCamera()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }
            
            _lookAtConstraint.AddSource(new ConstraintSource
            {
                sourceTransform = mainCamera.transform,
                weight = 1
            });
        }
    }
}