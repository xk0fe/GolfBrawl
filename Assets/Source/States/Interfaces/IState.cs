namespace Source.States.Interfaces
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
