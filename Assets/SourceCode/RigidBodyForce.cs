using UnityEngine;
public class RigidbodyForce : IForce
{
    Rigidbody rigidbody;

    public RigidbodyForce(Rigidbody rigidbody)
    {
        this.rigidbody = rigidbody;
    }

    public void AddForce(Vector3Func point, Vector3Func force)
    {
        point(point => force(force => rigidbody.AddForceAtPosition(force, point)));
    }
}