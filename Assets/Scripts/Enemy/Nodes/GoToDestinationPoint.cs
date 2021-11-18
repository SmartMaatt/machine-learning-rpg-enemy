using System;
using System.Collections;
using UnityEngine;

public class GoToDestinationPoint : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private Transform navAgentTransform;

    private GetFloatValue WalkSpeed;
    private GetFloatValue RestSpeed;
    private GetFloatValue AccelerationChaseBonus;
    private GetFloatValue Acceleration;

    public GoToDestinationPoint(AbstractEntity entity, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();

        navAgentTransform = entity.GetNavMeshAgentTransform();

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
            Debug.Log("Go to destination node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        Vector3 destPoint = entity.GetCurrentDestination();
        if (destPoint == null)
        {
            return NodeState.FAILURE;
        }

        Debug.Log("Go to point: " + destPoint);
        float distance = Vector3.Distance(destPoint, navAgentTransform.position);

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
            speedController.SetAcceleration(Acceleration());
            return NodeState.SUCCESS;
        }
    }
}
