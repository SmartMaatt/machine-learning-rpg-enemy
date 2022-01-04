using UnityEngine;

public class IsDirectContactNode : Node
{
    private Transform targetTransform;
    private Transform originTransform;

    public IsDirectContactNode(Transform target, Transform origin)
    {
        targetTransform = target;
        originTransform = origin;
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
