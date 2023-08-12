using UnityEngine;
public class RigidbodyMoveSystem : IMoveSystem
{
    Rigidbody rigidbody;
    IVector3 velocityV3 = new ZeroVector3();
    public RigidbodyMoveSystem(Rigidbody rigidbody, IEvent fixedUpdate)
    {
        this.rigidbody = rigidbody;
        fixedUpdate.Subscribe(FixedUpdate);
    }
    public void Move(IVector3 velocity)
    {
        velocityV3 = velocity;
    }
    void FixedUpdate()
    {
        velocityV3.Accept(v3 =>
        {
            if(v3!= Vector3.zero) rigidbody.velocity = new Vector3(v3.x,v3.y + rigidbody.velocity.y, v3.z);
        });
    }
}