using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

[RequireComponent(typeof(SpellController))]
public class RLMagicAgent : RLAgent
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

    protected virtual void Update()
    {
        AddRLReward(entity.GetMageRLParameters().everyFrameReward);
    }

    public override void OnEpisodeBegin()
    {
        Managers.Level.LevelReload();
    }

    public virtual void CollectObservations(VectorSensor sensor)
    {
        // Self observations
        sensor.AddObservation(entity.GetHealth());
        sensor.AddObservation(spellController.GetMana());

        ShieldObservations(sensor);
        HealObservations(sensor);

        sensor.AddObservation(spellController.GetCanAttack());
        sensor.AddObservation(spellController.GetLastHittedSpellID());
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
        int spellType = actions.DiscreteActions[0];
        int spellElement = actions.DiscreteActions[1];

        if (spellType == 0)
        {
            if (!entity.IsAttacking())
            {
                AddRLReward(entity.GetMageRLParameters().useSpellsWhenNotInAttackMode);
            }
        }

        if (!(spellType == 2 && spellElement == 2))
        {
            entity.SetSpellType((SpellType)spellType, spellElement);
            entity.Attack();
        }
    }

    public override void GenerateCSVData(bool won)
    {
        Managers.RlCsv.AddEpisodeData(
                new string[6]
                {
                    Managers.RlCsv.GetEpisodeCount().ToString(),
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    currentReward.ToString(),
                    entity.GetEntityName(),
                    Managers.Level.GetLevelTypeName(),
                    won.ToString()
                }
            );
    }
}