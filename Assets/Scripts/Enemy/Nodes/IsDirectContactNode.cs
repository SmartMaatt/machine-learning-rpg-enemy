using UnityEngine;

public class IsDirectContactNode : Node
{
    private Transform targetTransform;
    private Transform originTransform;
    private LayerMask targetMask;

    public IsDirectContactNode(Transform target, Transform origin, LayerMask mask)
    {
        targetTransform = target;
        originTransform = origin;
        targetMask = mask;
    }

    public override NodeState Evaluate()
    {
        Ray ray = new Ray(originTransform.position, targetTransform.position - originTransform.position);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 1f, out hit))
        {
            if (hit.collider.transform != targetTransform)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
