using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    new Rigidbody rigidbody;
    Animator animator;
    [SerializeField] Rigidbody[] ragdoll;
    [SerializeField] Transform hipR;
    [SerializeField] Transform kneeR;
    [SerializeField] Transform footR;

    [SerializeField] Transform hipL;
    [SerializeField] Transform kneeL;
    [SerializeField] Transform footL;
    [SerializeField] float maxSpeed;
    [SerializeField] float dampingForce;
    [SerializeField] float dampingClamp;
    [SerializeField] CollisionCheck[] collisionsCheck;
    IEvent fixedUpdateEvent;
    IKLegSystem legRIK;
    IKLegSystem legLIK;
    Vector2Func moveInput;
    Vector2Func torqueInput;
    FloatFunc footLength = FastAction => FastAction(Input.GetKey(KeyCode.Space)? 0.5f : 1f);
    BoolFunc isRagdoll;
    RayHit groundHit;
    float skateHeight = 0.185f;
    bool hasCollision;
    
    void Start()
    {
        fixedUpdateEvent = new Event();

        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 60;
        rigidbody.centerOfMass = new Vector3(0,0.9f,0);
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.angularDrag = 5f;
        animator = gameObject.GetComponent<Animator>();
        foreach(var r in ragdoll)
        {
            //r.isKinematic = true;
        }
        moveInput = v2Action => v2Action(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        torqueInput = v2Action => v2Action(new Vector2(Input.GetKey(KeyCode.L)? 1f : Input.GetKey(KeyCode.J)? -1f : 0, Input.GetKey(KeyCode.I)? 1f : Input.GetKey(KeyCode.K)? -1f : 0));
        groundHit = new RayHit(v3A => v3A(transform.position + transform.up), v3A => v3A(-transform.up), fA => fA(1f), fixedUpdateEvent);
    }
    void Update()
    {
        moveInput(input =>
        {
            animator.SetFloat("skate", input.y);
        });
    }
    void FixedUpdate()
    {
        fixedUpdateEvent.Call();

        groundHit.Accept(hit =>
        {
            Vector3 originPosition = transform.TransformPoint(rigidbody.centerOfMass);
            Vector3 touchPoint = hit.point;
            float velocityDot = Vector3.Dot(rigidbody.GetPointVelocity(originPosition), hit.normal /* transform.up */);
            Vector3 forwardDirection = Vector3.Cross(transform.right, hit.normal);
            Vector3 rightDirection = Vector3.Cross(-transform.forward, hit.normal);
            Vector3 groundTilt = new Vector3(Vector3.Dot(hit.normal, transform.forward), 0, Vector3.Dot(hit.normal, -transform.right));
            footLength(footLength =>
            {
                float dampingImmersion = (footLength - hit.distance) / footLength;
                rigidbody.AddForceAtPosition(hit.normal /* transform.up */ * Mathf.Max(dampingImmersion - velocityDot * dampingClamp,0) * dampingForce, originPosition, ForceMode.VelocityChange);
            });
            rigidbody.AddForceAtPosition(rightDirection * rigidbody.mass * 5f * Vector3.Dot(-transform.right, rigidbody.velocity), originPosition);
            torqueInput(input =>
            {
                rigidbody.AddRelativeTorque(new Vector3(input.y, 0, -input.x) * 8f);
            });
            moveInput(input =>
            {
                float moveForce = maxSpeed - Math.Min(rigidbody.velocity.magnitude, 0);
                rigidbody.AddForceAtPosition(forwardDirection * input.y * moveForce * rigidbody.mass * 0.1f, originPosition);
            });
            Vector3 velocity = rigidbody.velocity;
            float turnSpeed = Mathf.Clamp((Mathf.Sqrt(velocity.magnitude) - 0.1f * velocity.magnitude) * 10, 1, 10);
            rigidbody.AddRelativeTorque(new Vector3(0, groundTilt.z * turnSpeed * Mathf.Clamp(Vector3.Dot(transform.forward, rigidbody.velocity), -1, 1), 0));
            rigidbody.AddRelativeTorque(groundTilt * 5);
        });
    }
    void LateUpdate()
    {
        foreach(var collisionCheck in collisionsCheck)
        {
            collisionCheck.Accept(b => hasCollision = b);
            if(hasCollision) break;
        }
        Debug.Log(hasCollision);
    }
}

