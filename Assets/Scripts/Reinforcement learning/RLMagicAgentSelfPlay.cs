using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RLMagicAgentSelfPlay : RLMagicAgent
{
    [Header("Enemy observations references")]
    [SerializeField] private GameObject enemy;
    [SerializeField] protected Mage enemyController;
    [SerializeField] protected SpellController enemySpellController;
    [SerializeField] private MagicShield playerMagicShield;
    [SerializeField] private HealSpell playerHealSpell;

    protected override void Start()
    {
        base.Start();
        enemy = entity.GetEnemy();
        enemyController = enemy.GetComponent<Mage>();
        enemySpellController = enemyController.GetSpellController();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(enemyController.GetHealth());
        sensor.AddObservation(enemyController.GetSpellController().GetMana());
        PlayerShieldObservations(sensor);
        PlayerHealObservations(sensor);
    }

    private void PlayerShieldObservations(VectorSensor sensor)
    {
        if (enemyController.IsBlocking() == false)
        {
            sensor.AddObservation(0);   //Shield type
            sensor.AddObservation(0f);  //Shield time
        }
        else
        {
            if (playerMagicShield == null)
            {
                playerMagicShield = enemy.GetComponent<MagicShield>();
            }
            sensor.AddObservation((int)playerMagicShield.GetShieldType());  //Shield type
            sensor.AddObservation(playerMagicShield.GetShieldTime());       //Shield time
        }
    }

    private void PlayerHealObservations(VectorSensor sensor)
    {
        if (enemyController.IsHealing() == false)
        {
            sensor.AddObservation(0f); //Heal time
        }
        else
        {
            if (playerHealSpell == null)
            {
                playerHealSpell = GetComponent<HealSpell>();
            }
            sensor.AddObservation(playerHealSpell.GetHealTime()); //Heal time
        }
    }
}