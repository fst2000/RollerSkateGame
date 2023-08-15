using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    new Rigidbody rigidbody;
    [SerializeField] Rigidbody[] ragdoll;
    [SerializeField] Transform hipR;
    [SerializeField] Transform kneeR;
    [SerializeField] Transform footR;

    [SerializeField] Transform hipL;
    [SerializeField] Transform kneeL;
    [SerializeField] Transform footL;

    IKLegSystem legRIK;
    IKLegSystem legLIK;
    HitFunc hitFunc;
    FromToDirFunc footRayDirFunc;
    FootPositionFunc footPositionFunc;
    FootRotaitonFunc footRotaitonFunc;
    void Start()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 60;
        rigidbody.centerOfMass = new Vector3(0,0.9f,0);
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.isKinematic = true;
        foreach(var r in ragdoll)
        {
            r.isKinematic = true;
        }
        hitFunc = (origin, direction) =>
        {
            Ray ray = new Ray();
            RaycastHit hit;
            origin(origin =>
            {
                direction(direction =>
                {
                    ray = new Ray(origin, direction);
                });
            });
            if(Physics.Raycast(ray, out hit)) return hit;
            else return new RaycastHit();
        };
        footRayDirFunc = (hip, foot) => qAction => qAction(foot.position - hip.position);
        footPositionFunc = (hip, foot) =>
        {
            return vAction =>
            {
                float skateHeight = 0.185f;
                footRayDirFunc(hip, foot);
                Ray ray = new Ray(hip.position, rayDir);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit) && hit.distance <= rayDir.magnitude + skateHeight)
                {
                    vAction(hip.position + rayDir.normalized * (hit.distance - skateHeight));
                }
                else vAction(foot.position);
            };
        };
        footRotaitonFunc = (foot, hit) =>
        {
            return qAction =>
            {
                if()
                qAction(Quaternion.identity);
            };    
        };
        legRIK = new IKLegSystem(transform, hipR, kneeR, footR, footRotationFunc(footR, hitR), footPositionFunc(hipR, footR));
        legLIK = new IKLegSystem(transform, hipL, kneeL, footL, footRotationFunc(footL, hitL), footPositionFunc(hipL, footL));
    }
    void Update()
    {
        rigidbody.AddForceAtPosition(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical") * 5f), transform.position);
        Vector3 tiltInput = new Vector3((Input.GetKey(KeyCode.J)? 0 : 1) - (Input.GetKey(KeyCode.L)? 0 : 1) , 0, (Input.GetKey(KeyCode.K)? 0 : 1) - (Input.GetKey(KeyCode.I)? 0 : 1));
        rigidbody.AddTorque(transform.TransformDirection(tiltInput) * 10);
    }
    void FixedUpdate()
    {
        
    }
    void LateUpdate()
    {
        legRIK.Move();
        legLIK.Move();
    }
}
public delegate Vector3Func FromToDirFunc(Transform from, Transform to);
public delegate Vector3Func FootPositionFunc(Transform hip, Transform foot);
public delegate RotationFunc FootRotaitonFunc(Transform foot, RaycastHit hit);
public delegate RaycastHit HitFunc(Vector3Func origin, Vector3Func direction);
