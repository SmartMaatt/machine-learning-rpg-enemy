using System;
using UnityEngine;

public class GoToDestinationPoint : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private Transform originTransform;

    private GetFloatValue WalkSpeed;
    private GetFloatValue RestSpeed;
    private GetFloatValue AccelerationChaseBonus;
    private GetFloatValue Acceleration;

    public GoToDestinationPoint(AbstractEntity entity, Transform origin, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();

        originTransform = origin;

        try
        {
            WalkSpeed = delegates[0];
            RestSpeed = delegates[1];
            AccelerationChaseBonus = delegates[2];
            Acceleration = delegates[3];
        }
        catch (ArgumentOutOfRangeException err)
        {
            WalkSpeed = null;
            RestSpeed = null;
            AccelerationChaseBonus = null;
            Acceleration = null;
            Debug.LogError("Go to destination node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        Vector3 destPoint = entity.GetCurrentDestination();
        if (destPoint == null || destPoint == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(destPoint, originTransform.position);

        if (distance > 0.2f)
        {
            speedController.SetCurrentMaxSpeed(WalkSpeed());
            speedController.SetAcceleration(AccelerationChaseBonus());

            entity.SetNavAgentDestination(destPoint);
            return NodeState.RUNNING;
        }
        else
        {
            speedController.SetCurrentMaxSpeed(RestSpeed());
            speedController.SetAcceleration(entity.breakAcceleration);
            return NodeState.SUCCESS;
        }
    }
}
