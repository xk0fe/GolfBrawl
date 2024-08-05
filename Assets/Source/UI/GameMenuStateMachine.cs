using Source.Network;
using Source.UI.States;
using UnityEngine;
using StateMachineBehaviour = Source.States.StateMachineBehaviour;

namespace Source.UI
{
    public class MenuStateMachine : StateMachineBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private NetworkService _networkService;
        
        [Header("Game Objects")]
        [SerializeField]
        private GameObject _menuScreen;
        [SerializeField]
        private GameObject _gameplayScreen;
        
        private void Start()
        {
            _networkService.OnPlayerJoinedServer += OnPlayerJoinedServer;
            _networkService.OnPlayerLeftServer += OnPlayerLeftServer;
            
            OnPlayerLeftServer();
        }
        
        private void OnPlayerJoinedServer()
        {
            ChangeState(new GameObjectSwitchState(_gameplayScreen, _menuScreen));
        }
        
        private void OnPlayerLeftServer()
        {
            ChangeState(new GameObjectSwitchState(_menuScreen, _gameplayScreen));
        }

        private void OnDestroy()
        {
            _networkService.OnPlayerJoinedServer -= OnPlayerJoinedServer;
            _networkService.OnPlayerLeftServer -= OnPlayerLeftServer;
        }
    }
}