using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Source.Network
{
    public class NetworkService : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField]
        private List<NetworkBehaviour> _networkBehaviours = new();
        
        public static NetworkService Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            Debug.Log($"{nameof(NetworkService)} is null");
            return null;

        }
        private static NetworkService _instance;
        
        public event Action OnClientStartedConnection;
        public event Action OnClientConnected;
        public event Action OnLocalPlayerJoinedServer;
        public event Action OnLocalPlayerLeftServer;
        public event Action OnPlayerJoinedServer;
        public event Action OnPlayerLeftServer;
        public event Action<NetworkRunner> OnNetworkRunnerStarted; 

        public LocalPlayerService LocalPlayerService => _localPlayerService;

        private LocalPlayerService _localPlayerService;

        [SerializeField]
        private NetworkRunner _networkRunnerPrefab;
        
        private NetworkRunner _runner;

        private void Awake()
        {
            _instance = this;
            _localPlayerService = new LocalPlayerService();
        }

        public void BreakSession()
        {
            _runner.Shutdown();
            OnPlayerLeftServer?.Invoke();
            OnLocalPlayerLeftServer?.Invoke();
        }

        public async void JoinOrCreateSession(string roomName)
        {
            OnClientStartedConnection?.Invoke();

            if (_runner == null)
            {
                _runner = Instantiate(_networkRunnerPrefab);
            }
            else
            {
                _runner.RemoveCallbacks(this);
            }
            
            _runner.AddCallbacks(this);
            
            _runner.ProvideInput = true;

            var startGameArgs = new StartGameArgs
            {
                GameMode = Fusion.GameMode.Shared,
                Address = NetAddress.Any(),
                SessionName = roomName,
                PlayerCount = 6,
                SceneManager = _runner.GetComponent<INetworkSceneManager>(),
            };
            
            var result = await _runner.StartGame(startGameArgs);
            
            OnNetworkRunnerStarted?.Invoke(_runner);

            if (result.Ok)
            {
                OnClientConnected?.Invoke();
            }
            else
            {
                Debug.LogError($"Failed to start: {result.ShutdownReason}");
            }
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            var isFirstPlayer = player.PlayerId == 1;
            if (isFirstPlayer)
            {
                runner.SetMasterClient(player);
                foreach (var behaviour in _networkBehaviours)
                {
                    _runner.Spawn(behaviour, inputAuthority: player);
                }
            }

            OnPlayerJoinedServer?.Invoke();
            
            if (player == runner.LocalPlayer)
            {
                OnLocalPlayerJoinedServer?.Invoke();
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            OnPlayerLeftServer?.Invoke();

            if (player == runner.LocalPlayer)
            {
                OnLocalPlayerLeftServer?.Invoke();
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}