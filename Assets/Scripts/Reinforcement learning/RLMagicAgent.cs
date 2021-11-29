using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

[RequireComponent(typeof(AbstractEntity))]
[RequireComponent(typeof(SpellController))]
public class RLMagicAgent : Agent
{
    [Header("Self observations references")]
    [SerializeField] AbstractEntity entity;
    [SerializeField] SpellController spellController;
    [SerializeField] MagicShield magicShield;
    [SerializeField] HealSpell healSpell;

    public override void OnEpisodeBegin()
    {
        entity = GetComponent<AbstractEntity>();
        spellController = GetComponent<SpellController>();

        //transform.localPosition = new Vector3(Random.Range(-2f, +2f), 1, Random.Range(-2f, +2f));
        //targetTransform.localPosition = new Vector3(Random.Range(-2f, +2f), 1, Random.Range(-2f, +2f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Self observations
        sensor.AddObservation(entity.GetHealth());
        sensor.AddObservation(spellController.GetMana());
        ShieldObservations(sensor);
        HealObservations(sensor);
        sensor.AddObservation(spellController.GetCanAttack());
    }

    private void ShieldObservations(VectorSensor sensor)
    {
        if(entity.IsBlocking() == false)
        {
            sensor.AddObservation(0);   //Shield type
            sensor.AddObservation(0f);  //Shield time
        }
        else
        {
            if (magicShield == null)
            {
                magicShield = GetComponent<MagicShield>();
            }
            sensor.AddObservation((int)magicShield.GetShieldType());  //Shield type
            sensor.AddObservation(magicShield.GetShieldTime());       //Shield time
        }
    }

    private void HealObservations(VectorSensor sensor)
    {
        if (entity.IsHealing() == false)
        {
            sensor.AddObservation(0f); //Heal time
        }
        else
        {
            if (healSpell == null)
            {
                healSpell = GetComponent<HealSpell>();
            }
            sensor.AddObservation(healSpell.GetHealTime()); //Heal time
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    /*For testing purposes*/
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    float rewardScore = 0f;
    //    if (other.TryGetComponent<Reward>(out Reward reward))
    //    {
    //        rewardScore = +1f;
    //        SetReward(rewardScore);
    //        floorMeshRenderer.material = green;
    //        EndEpisode();
    //    }
    //    else if (other.TryGetComponent<Border>(out Border border))
    //    {
    //        rewardScore = -1f;
    //        SetReward(rewardScore);
    //        floorMeshRenderer.material = red;
    //        EndEpisode();
    //    }
    //}
}