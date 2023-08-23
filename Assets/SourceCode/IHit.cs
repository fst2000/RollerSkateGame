using UnityEngine;
using System;
public interface IHit
{
    void Accept(Action<RaycastHit> action);
}
public delegate void HitFunc(Vector3Func origin, Vector3Func direction, Action<RaycastHit> action);