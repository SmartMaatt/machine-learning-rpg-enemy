using UnityEngine;

public class RangeNode : Node
{
    private Transform targetTransform;
    private Transform originTransform;
    private GetFloatValue Range;

    public RangeNode(Transform target, Transform origin, GetFloatValue Range)
    {
        targetTransform = target;
        originTransform = origin;

        this.Range = Range;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(targetTransform.position, originTransform.position);
        return distance <= Range() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
