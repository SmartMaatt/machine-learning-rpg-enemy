using System;
using UnityEngine;

public class AreaExplosionNode : Node
{
    private AbstractEntity entity;

    public AreaExplosionNode(AbstractEntity entity)
    {
        this.entity = entity;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Area Explosion!!!");
        return NodeState.SUCCESS;
    }
}
