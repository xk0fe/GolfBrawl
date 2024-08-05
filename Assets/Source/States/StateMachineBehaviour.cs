using Source.States.Interfaces;
using UnityEngine;

namespace Source.States
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new StateMachine();
        }

        private void Update()
        {
            _stateMachine.Update();
        }
        
        public void ChangeState(IState newState)
        {
            _stateMachine.ChangeState(newState);
        }
    }
}