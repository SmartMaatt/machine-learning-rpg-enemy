using UnityEngine;
using Unity.MLAgents;
using Unity.Barracuda;
using Unity.MLAgents.Policies;
using UnityEngine.Serialization;

[RequireComponent(typeof(AbstractEntity))]
public abstract class RLAgent : Agent
{
    protected float currentReward = 0f;
    protected bool executedFirstAction;

    public void AddRLReward(float value)
    {
        AddReward(value);
        currentReward += value;
    }

    public void EndRLEpisode(string endEpisodeStatus)
    {
        GenerateCSVData(endEpisodeStatus);
        currentReward = 0f;
        executedFirstAction = false;
        EndEpisode();
    }

    public abstract void GenerateCSVData(string endEpisodeStatus);
}