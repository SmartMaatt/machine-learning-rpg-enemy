using UnityEngine;
using UnityEngine.AI;

public class IsInDestinationPoint : Node
{
    private AbstractEntity entity;
    private NavMeshAgent navAgent;

    private Transform originTransform;

    public IsInDestinationPoint(AbstractEntity entity, Transform origin)
    {
        this.entity = entity;
        navAgent = entity.GetNavMeshAgent();

        originTransform = origin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(entity.GetCurrentDestination(), originTransform.position);
        if (distance < 0.2f)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
