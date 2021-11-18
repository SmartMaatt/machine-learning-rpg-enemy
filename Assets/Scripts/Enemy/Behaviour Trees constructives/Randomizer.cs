using System;
using System.Collections.Generic;
using UnityEngine;

class Randomizer : Node
{
    private System.Random rand;
    private List<RandomizerNode> nodes;
    private int maxNumberOfTicks;

    private Node currentNode;
    private int currentTicks;

    public Randomizer(List<RandomizerNode> nodes, int maxNumberOfTicks)
    {
        rand = new System.Random();
        this.nodes = nodes;
        this.maxNumberOfTicks = maxNumberOfTicks;

        currentNode = null;
        currentTicks = maxNumberOfTicks;

        if (CheckIfHundedPercent())
        {
            currentNode = ChooseNode();
            currentTicks = maxNumberOfTicks;
        }
        else
        {
            throw new ProbabilityNotEqualHundredPercentException("Probability of all random nodes doesn't sum up to 100%!");
        }
    }

    public override NodeState Evaluate()
    {
        if(currentTicks == 0)
        {
            currentNode = ChooseNode();
            currentTicks = maxNumberOfTicks;
        }
       
        currentTicks--;
        return currentNode.Evaluate();
    }

    private Node ChooseNode()
    {
        float choice = (rand.Next(0, 2)) / 100;
        float currentProb = 0;

        for (int i = 0; i < nodes.Capacity; i++)
        {
            currentProb += nodes[i].probability;
            if(choice <= currentProb)
            {
                return nodes[i].node;
            }
        }
        return nodes[nodes.Capacity - 1].node;
    }

    private bool CheckIfHundedPercent()
    {
        float sum = 0;
        foreach (RandomizerNode node in nodes)
        {
            sum += node.probability;
        }
        return sum == 1f;
    }
}

[System.Serializable]
class RandomizerNode
{
    public Node node;
    [Range(0, 1)]
    public float probability;
}