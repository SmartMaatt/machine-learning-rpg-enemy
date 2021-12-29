using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(AbstractEntity))]
public abstract class RLAgent : Agent
{
    protected float currentReward = 0f;

    protected string brainAssetName = null;
    protected NNModel brainModel = null;
    protected BehaviorParameters bp = null;

    public void AddRLReward(float value)
    {
        AddReward(value);
        currentReward += value;
    }

    public void EndRLEpisode(string endEpisodeStatus)
    {
        GenerateCSVData(endEpisodeStatus);
        currentReward = 0f;
        EndEpisode();
    }

    public void SetBrainModel(string brainAssetName, NNModel brainModel)
    {
        this.brainAssetName = brainAssetName;
        this.brainModel = brainModel;
    }

    protected void SetupBrainModel()
    {
        if(brainModel != null)
        {
            SetModel(brainAssetName, brainModel);
            bp = GetComponent<BehaviorParameters>();
            bp.BehaviorType = BehaviorType.InferenceOnly;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) { }
    public abstract void GenerateCSVData(string endEpisodeStatus);
}