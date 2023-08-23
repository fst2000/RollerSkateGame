public class StateMachine
{
    IHumanState state;

    public StateMachine(IHumanState state)
    {
        this.state = state;
    }

    public void Update()
    {
        if(state.NextState() != state)
        {
            state.Exit();
            state = state.NextState();
            state.Enter();
        }
        state.Update();
    }
}