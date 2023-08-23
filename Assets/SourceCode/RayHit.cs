using System;
using UnityEngine;

public class RayHit : IHit
{
    Vector3Func origin;
    Vector3Func direction;
    FloatFunc distance;
    RaycastHit hit;
    public RayHit(Vector3Func origin, Vector3Func direction, FloatFunc distance, IEvent fixedUpdate)
    {
        this.origin = origin;
        this.direction = direction;
        this.distance = distance;
        fixedUpdate.Subscribe(FixedUpdate);
        hit = new RaycastHit();
    }
    public void Accept(Action<RaycastHit> action)
    {
        action(hit);
    }
    void FixedUpdate()
    {
        origin(origin =>
        {
            direction(direction =>
            {
                distance(distance =>
                {
                    Ray ray = new Ray(origin, direction);
                    if(!Physics.Raycast(ray, out hit, distance)) hit.distance = 100;
                });
            });
        });
    }
}