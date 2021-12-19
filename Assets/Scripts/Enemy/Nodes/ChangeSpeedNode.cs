using System;
using UnityEngine;

public class ChangeSpeedNode : Node
{
    private AbstractEntity entity;
    private SpeedController speedController;

    private GetFloatValue Speed;
    private GetFloatValue Acceleration;

    public ChangeSpeedNode(AbstractEntity entity, GetFloatValue[] delegates)
    {
        this.entity = entity;
        speedController = entity.GetSpeedController();

        try
        {
            Speed = delegates[0];
            Acceleration = delegates[1];
        }
        catch (ArgumentOutOfRangeException err)
        {
            Speed = null;
            Acceleration = null;
            Debug.Log("Attack node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        speedController.SetCurrentMaxSpeed(Speed());
        speedController.SetAcceleration(Acceleration());
        return NodeState.SUCCESS;
    }
}
