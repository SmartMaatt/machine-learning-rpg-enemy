using System;
using UnityEngine;

public class HealthNode : Node
{
    private GetFloatValue CurrentHealth;
    private GetFloatValue HealthThreshold;

    public HealthNode(GetFloatValue[] delegates)
    {
        try
        {
            CurrentHealth = delegates[0];
            HealthThreshold = delegates[1];
        }
        catch (ArgumentOutOfRangeException err)
        {
            CurrentHealth = null;
            HealthThreshold = null;
            Debug.Log("Health Node: " + err);
        }
    }

    public override NodeState Evaluate()
    {
        return CurrentHealth() <= HealthThreshold() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
