using UnityEngine;
using Unity.MLAgents;

[RequireComponent(typeof(AbstractEntity))]
public abstract class RLAgent : Agent
{
    protected float currentReward = 0f;

    public void AddRLReward(float value)
    {
        AddReward(value);
        currentReward += value;
    }

    public void EndRLEpisode(bool won)
    {
        GenerateCSVData(won);
        currentReward = 0f;
        EndEpisode();
    }

    public abstract void GenerateCSVData(bool won);
}