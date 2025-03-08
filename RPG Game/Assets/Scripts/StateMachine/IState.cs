namespace RPG.FSM
{
    public interface IState
    {
        void OnStateEnter();
        void Update();
        void OnStateExit();
    }
}