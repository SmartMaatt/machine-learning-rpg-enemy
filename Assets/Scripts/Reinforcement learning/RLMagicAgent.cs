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
    [SerializeField] protected Mage entity;
    [SerializeField] protected SpellController spellController;
    [SerializeField] protected MagicShield magicShield;
    [SerializeField] protected HealSpell healSpell;

    protected virtual void Start()
    {
        entity = GetComponent<Mage>();
        spellController = GetComponent<SpellController>();
    }

    public virtual void CollectObservations(VectorSensor sensor)
    {
        // Self observations
        sensor.AddObservation(entity.GetHealth());
        sensor.AddObservation(spellController.GetMana());
        ShieldObservations(sensor);
        HealObservations(sensor);
        sensor.AddObservation(spellController.GetCanAttack());
        sensor.AddObservation(entity.IsAttacking());
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
        if(entity.IsAttacking())
        {
            entity.SetSpellType((SpellType)actions.DiscreteActions[0], actions.DiscreteActions[1]);
            entity.Attack();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}