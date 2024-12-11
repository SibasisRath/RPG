public interface IState 
{
    void Enter();
    void HandleInput();
    void UpdateLogic();
    void UpdatePhysics();
    void Exit();
}
