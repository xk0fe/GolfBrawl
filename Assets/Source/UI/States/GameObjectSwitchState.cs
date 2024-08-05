using Source.States.Interfaces;
using UnityEngine;

namespace Source.UI.States
{
    public class GameObjectSwitchState : IState
    {
        public GameObjectSwitchState(GameObject gameObjectActivate, GameObject gameObjectDeactivate)
        {
            _gameObjectActivate = gameObjectActivate;
            _gameObjectDeactivate = gameObjectDeactivate;
        }
        
        private GameObject _gameObjectActivate;
        private GameObject _gameObjectDeactivate;
        
        public void Enter()
        {
            _gameObjectActivate.SetActive(true);
            _gameObjectDeactivate.SetActive(false);
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}