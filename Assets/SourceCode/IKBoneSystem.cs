using UnityEngine;
public class IKLegSystem
{
    Transform human;
    Transform hip;
    Transform knee;
    Transform foot;
    RotationFunc footRotation;
    Vector3Func footPosition;

    public IKLegSystem(
        Transform human,
        Transform hip,
        Transform knee,
        Transform foot,
        RotationFunc footRotation,
        Vector3Func footPosition)
    {
        this.human = human;
        this.hip = hip;
        this.knee = knee;
        this.foot = foot;
        this.footRotation = footRotation;
        this.footPosition = footPosition;
    }
    public void Move()
    {
        float hipLength = (hip.position - knee.position).magnitude;
        float kneeLength = (knee.position - foot.position).magnitude;
        footPosition(footPos =>
        {
            Vector3 hipToFoot = footPos - hip.position;
            //Debug.DrawLine(hip.position, hip.position + hipToFoot);
            float hipAngle = 0;
            if(hipToFoot.magnitude <= hipLength + kneeLength)
            {
                hipAngle = (180 / Mathf.PI) * Mathf.Acos(((hipLength * hipLength) + (hipToFoot.magnitude * hipToFoot.magnitude) - (kneeLength * kneeLength)) / (2 * hipLength * hipToFoot.magnitude));
            }
            Vector3 hipToKnee = human.rotation * Quaternion.Euler(-hipAngle, 0, 0) * Vector3.ClampMagnitude(human.InverseTransformDirection(hipToFoot), hipLength);
            Vector3 kneePos = hip.position + hipToKnee;
            //Debug.DrawLine(hip.position, kneePos);
            Debug.DrawLine(kneePos, kneePos + Vector3.ClampMagnitude(footPos - kneePos, kneeLength));
            hip.rotation =  Quaternion.LookRotation(hipToKnee, -hip.right) * Quaternion.Euler(0, -90, -90);
            knee.rotation = Quaternion.LookRotation(footPos - kneePos, -knee.right) * Quaternion.Euler(0, -90, -90);
            foot.position = footPos;

            footRotation(footRot =>
            {
                Debug.DrawLine(foot.position, foot.position + footRot * Vector3.forward * 0.2f);
            });
        });
    }
}