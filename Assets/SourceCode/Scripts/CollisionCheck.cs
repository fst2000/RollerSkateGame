using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionCheck : MonoBehaviour, IBool
{
    bool hasCollision;
    public void Accept(Action<bool> action)
    {
        action(hasCollision);
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != 2)
        {
            hasCollision = true;   
        }
    }
    void OnCollisionExit()
    {
        hasCollision = false;
    }
}
