using System;
using UnityEngine;

public class ChaseNode : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private Transform targetTransform;
    private Transform navAgentTransform;

    private GetFloatValue RunSpeed;
    private GetFloatValue RestSpeed;
    private GetFloatValue AccelerationChaseBonus;
    private GetFloatValue Acceleration;

    public ChaseNode(AbstractEntity entity, Transform target, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();

        targetTransform = target;
        navAgentTransform = entity.GetNavMeshAgentTransform();

        try
        {
            RunSpeed = delegates[0];
            RestSpeed = delegates[1];
            AccelerationChaseBonus = delegates[2];
            Acceleration = delegates[3];
        }
        catch (ArgumentOutOfRangeException err)
        {
            RunSpeed = null;
            RestSpeed = null;
            AccelerationChaseBonus = null;
            Acceleration = null;
            Debug.Log("ChaseNode: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        SetState();
        Debug.Log("Chase !!!");

        float distance = Vector3.Distance(targetTransform.position, navAgentTransform.position);

        if (distance > 0.2f)
        {
            speedController.SetCurrentMaxSpeed(RunSpeed());
            speedController.SetAcceleration(AccelerationChaseBonus());

            entity.SetCurrentDestination(targetTransform.position);
            entity.SetNavAgentDestination(targetTransform.position);
            return NodeState.RUNNING;
        }
        else
        {
            speedController.SetCurrentMaxSpeed(RestSpeed());
            speedController.SetAcceleration(entity.breakAcceleration);
            return NodeState.SUCCESS;
        }
    }

    private void SetState()
    {
        if (entity.GetEntityState() != EntityState.CHASE)
        {
            entity.SetEntityState(EntityState.CHASE);
        }
    }
}
