using Source.States.Interfaces;

namespace Source.States
{
    public class StateMachine
    {
        private IState _currentState;

        public void Update()
        {
            _currentState?.Update();
        }

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
    }
}