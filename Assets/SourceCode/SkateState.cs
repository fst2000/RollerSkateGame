public class SkateState : IHumanState
{
    IHit groundHit;
    IAnimator animator;
    IFloat animationBlend;

    public SkateState(IHit groundHit, IAnimator animator, IFloat animationBlend)
    {
    }

    public void Enter()
    {
        animator.StartAnimation("Skate");
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public IHumanState NextState()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}