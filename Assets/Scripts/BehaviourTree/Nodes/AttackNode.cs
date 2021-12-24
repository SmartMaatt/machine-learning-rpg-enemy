using System;
using UnityEngine;

public class AttackNode : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private Transform targetTransform;
    private Transform originTransform;

    private GetFloatValue RestSpeed;
    private GetFloatValue Acceleration;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public AttackNode(AbstractEntity entity, Transform target, Transform origin, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();

        targetTransform = target;
        originTransform = origin;
        smoothDamp = 1f;

        try
        {
            RestSpeed = delegates[0];
            Acceleration = delegates[1];
        }
        catch (ArgumentOutOfRangeException err)
        {
            RestSpeed = null;
            Acceleration = null;
            Debug.LogError("Attack node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        SetState();

        speedController.SetCurrentMaxSpeed(RestSpeed());
        speedController.SetAcceleration(Acceleration());
        entity.SetCurrentDestination(targetTransform.position);

        return NodeState.RUNNING;
    }

    private void SetState()
    {
        if (entity.GetEntityState() != EntityState.ATTACK)
        {
            entity.SetEntityState(EntityState.ATTACK);
        }
    }
}
