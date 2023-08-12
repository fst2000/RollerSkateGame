using System;
using UnityEngine;

public class TransformRightVector3 : IVector3
{
    Transform transform;

    public TransformRightVector3(Transform transform)
    {
        this.transform = transform;
    }

    public void Accept(Action<Vector3> action)
    {
        action(transform.right);
    }
}