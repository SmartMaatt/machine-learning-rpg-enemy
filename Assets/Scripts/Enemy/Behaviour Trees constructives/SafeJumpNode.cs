using System;
using UnityEngine;

class SafeJumpNode : Node
{
    private Node jumpNode;

    public SafeJumpNode()
    {
        jumpNode = null;
    }

    public SafeJumpNode(Node safeJumpNode)
    {
        this.jumpNode = safeJumpNode;
    }

    public override NodeState Evaluate()
    {
         return jumpNode.Evaluate();
    }

    public void SetJumpNode(Node safeJumpNode)
    {
        this.jumpNode = safeJumpNode;
    }
}