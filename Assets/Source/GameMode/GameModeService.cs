using System.Collections.Generic;
using Fusion;
using Source.Events;
using Source.Network;
using Source.Player;
using Source.Settings;
using UnityEngine;

namespace Source.GameMode
{
    public class GameModeService : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [Header("Events")]
        [SerializeField]
        private VoidEventChannel _onBallHitEventChannel;
        [SerializeField]
        private VoidEventChannel _onBallHitHoleChannel;
        
        private Dictionary<int, PlayerData> _players = new();

        private bool _isLocalPlayerReady;
        
        private void Awake()
        {
            _onBallHitEventChannel.OnEventRaised += OnBallHit;
            _onBallHitHoleChannel.OnEventRaised += OnBallHitHole;
        }

        private void OnBallHitHole()
        {
            RPC_CountScore();
        }

        private void OnBallHit()
        {
            RPC_CountHit();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_CountScore()
        {
            var localPlayerId = Runner.LocalPlayer.PlayerId;
            RPC_CalculateScore(localPlayerId);
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_CountHit()
        {
            var localPlayerId = Runner.LocalPlayer.PlayerId;
            RPC_AddHits(localPlayerId);
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_CalculateScore(int winnerId)
        {
            foreach (var (id, data) in _players)
            {
                var isWinner = winnerId == id ? 1 : 0;
                var hits = data.Hits == 0 ? GameSettingsConstants.BASE_SCORE : data.Hits;
                data.Score = (GameSettingsConstants.BASE_SCORE / hits) +
                             (GameSettingsConstants.HOLE_IN_SCORE * isWinner);
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_AddHits(int playerId)
        {
            if (_players.TryGetValue(playerId, out var playerData))
            {
                playerData.Hits++;
            }
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (player != Runner.LocalPlayer)
            {
                return;
            }
            
            var nickname = LocalPlayerService.GetLocalPlayerNickname();
            RPC_AddPlayerRequest(player, nickname);
        }

        [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
        private void RPC_AddPlayerRequest(PlayerRef player, string nickname)
        {
            var playerData = new PlayerData
            {
                PlayerRef = player,
                Name = nickname,
                Score = 0
            };
            var playerId = player.PlayerId;
            
            _players[playerId] = playerData;
        }
        
        public void PlayerLeft(PlayerRef player)
        {
            if (!HasStateAuthority)
            {
                return;
            }
            
            var playerId = player.PlayerId;
            
            RPC_RemovePlayer(playerId);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_RemovePlayer(int playerId)
        {
            _players.Remove(playerId);
        }
        
        
        private void OnDestroy()
        {
            _onBallHitEventChannel.OnEventRaised -= OnBallHit;
            _onBallHitHoleChannel.OnEventRaised -= OnBallHitHole;
        }
    }
}