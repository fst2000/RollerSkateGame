public interface IHumanState
{
    void Enter();
    void Update();
    void Exit();
    IHumanState NextState();
}
public delegate IHumanState HumanState();