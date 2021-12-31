using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

[RequireComponent(typeof(SpellController))]
public abstract class RLMagicAgent : RLAgent
{
    [Header("Self observations references")]
    [SerializeField] protected Mage entity;
    [SerializeField] protected SpellController spellController;
    [SerializeField] protected MagicShield magicShield;
    [SerializeField] protected HealSpell healSpell;

    protected int numberOfEntityStates;
    protected int numberOfShields;
    protected int numberOfSpells;
    protected int numberOfCooldownOptions;

    protected virtual void Start()
    {
        entity = GetComponent<Mage>();
        spellController = GetComponent<SpellController>();

        numberOfEntityStates = (int)EntityState.NUMBER_OF_STATES;
        numberOfShields = (int)ShieldSpell.NUMBER_OF_SHIELDS + 1; //Additional "no shield"
        numberOfSpells = (int)CastSpell.NUMBER_OF_SPELLS + 1; //Additional "no spell"
        numberOfCooldownOptions = 2;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Self observations
        sensor.AddObservation(Managers.Level.GetNormalizedEpisodeTimeLimit());
        sensor.AddObservation(Managers.Level.GetNormalizedEpisodeTimeIteration());
        sensor.AddOneHotObservation((int)entity.GetEntityState(), numberOfEntityStates);

        sensor.AddObservation(entity.GetNormalizedHealth());
        sensor.AddObservation(spellController.GetNormalizedMana());

        ShieldObservations(sensor);
        HealObservations(sensor);

        sensor.AddOneHotObservation(spellController.GetOneHotCanAttack(), numberOfCooldownOptions);
        sensor.AddOneHotObservation(spellController.GetLastHittedSpellID() + 1, numberOfSpells);
    }

    private void ShieldObservations(VectorSensor sensor)
    {
        if (entity.IsBlocking() == false)
        {
            sensor.AddOneHotObservation(0, numberOfShields);   //Shield type
            sensor.AddObservation(0f);                         //Shield time
        }
        else
        {
            if (magicShield == null)
            {
                magicShield = GetComponent<MagicShield>();
            }
            sensor.AddOneHotObservation((int)magicShield.GetShieldType() + 1, numberOfShields);     //Shield type
            sensor.AddObservation(magicShield.GetNormalizedShieldTime());                           //Shield time
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
            sensor.AddObservation(healSpell.GetNormalizedHealTime()); //Heal time
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int spellType = actions.DiscreteActions[0];
        int spellElement = actions.DiscreteActions[1];

        if (!(spellType == 2 && spellElement == 2))
        {
            entity.SetSpellType((SpellType)spellType, spellElement);
            entity.Attack();
        }
        Debug.Log(entity.GetEntityName() + ": " + spellType + " - " + spellElement);
    }

    public override void GenerateCSVData(string endEpisodeStatus)
    {
        Managers.RlCsv.AddEpisodeData(
                new string[9]
                {
                    Managers.RlCsv.GetEpisodeCount().ToString(),
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    currentReward.ToString(),
                    entity.GetEntityName(),
                    Managers.Level.GetLevelTypeName(),
                    endEpisodeStatus,
                    (Managers.Level.GetEpisodeTimeIteration() + 1).ToString(),
                    ((int)entity.GetFullEpisodeTime()).ToString(),
                    ((int)(entity.GetPercentageEnemyInteresetTime())).ToString() + "%"
                }
            );
    }
}