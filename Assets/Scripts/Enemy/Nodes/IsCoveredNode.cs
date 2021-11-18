using UnityEngine;

public class IsCoveredNode : Node
{
    private Transform targetTransform;
    private Transform originTransform;

    public IsCoveredNode(Transform target, Transform origin)
    {
        targetTransform = target;
        originTransform = origin;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.Raycast(originTransform.position, targetTransform.position - originTransform.position, out hit))
        {
            if (hit.collider.transform != targetTransform)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
