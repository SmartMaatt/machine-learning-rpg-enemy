using System;
using UnityEngine;

class SightNode : Node
{
    private Transform targetTransform;
    private Transform originTransform;
    private GetFloatValue SightRange;
    private GetFloatValue SightConeRange;

    public SightNode(Transform targetTransform, Transform originTransform, GetFloatValue[] delegates)
    {
        this.targetTransform = targetTransform;
        this.originTransform = originTransform;
        try
        {
            SightRange = delegates[0];
            SightConeRange = delegates[1];
        }
        catch (ArgumentOutOfRangeException err)
        {
            SightRange = null;
            SightConeRange = null;
            Debug.LogError("SightNode: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        //[1] Check sight sphere
        if (Vector3.Distance(targetTransform.position, originTransform.position) < SightRange())
        {
            Vector3 originToTarget = targetTransform.position - originTransform.position;
            Vector3 originToTargetSightLevel = (targetTransform.position + new Vector3(0, 1.5f, 0)) - (originTransform.position + new Vector3(0, 1.5f, 0));

            float cosOfMaxSightAngle = Mathf.Cos(Mathf.Deg2Rad * SightConeRange() / 2);
            float cosOfCurrentAngle = Vector3.Dot(originToTarget, originTransform.forward) / (originToTarget.magnitude * originTransform.forward.magnitude);

            //[2] Check sight angle
            if (cosOfCurrentAngle > cosOfMaxSightAngle)
            {
                Ray ray = new Ray(originTransform.position, originToTargetSightLevel);
                RaycastHit hit;

                //[3] Check for obsticles with RayCast
                if (Physics.SphereCast(ray, 0.75f, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;

                    if (hitObject.GetComponent<PlayerMovement>())
                    {
                        return NodeState.SUCCESS;
                    }
                }
            }
        }
        return NodeState.FAILURE;
    }
}
