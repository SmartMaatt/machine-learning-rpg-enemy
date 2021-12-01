using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

[RequireComponent(typeof(AbstractEntity))]
public class TestAgent : Agent
{
    [Header("Self observations references")]
    [SerializeField] protected AbstractEntity entity;

    protected virtual void Start()
    {
        entity = GetComponent<AbstractEntity>();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start siema siema");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Self observations
        sensor.AddObservation(entity.GetHealth());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if(entity.IsAttacking())
        {
            Debug.Log(actions.DiscreteActions[0]);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}