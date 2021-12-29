using UnityEngine;
using Unity.MLAgents.Sensors;

public class RLMagicAgentSelfPlay : RLMagicAgent
{
    [Header("Enemy observations references")]
    [SerializeField] private GameObject enemy;
    [SerializeField] protected Mage enemyController;
    [SerializeField] protected SpellController enemySpellController;
    [SerializeField] private MagicShield enemyMagicShield;
    [SerializeField] private HealSpell enemyHealSpell;

    private float secondCounterTime = 0f;
    private float secondIntervalTime = 1f;

    protected override void Start()
    {
        base.Start();
        enemy = entity.GetEnemy();
        enemyController = enemy.GetComponent<Mage>();
        enemySpellController = enemyController.GetSpellController();
        SetupBrainModel();
    }

    protected virtual void Update()
    {
        secondCounterTime += Time.deltaTime;
        if (secondCounterTime >= secondIntervalTime)
        {
            AddRLReward(entity.GetMageRLParameters().timeReward * enemyController.GetNormalizedHealth() * 100);
            secondCounterTime -= secondIntervalTime;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(enemyController.GetNormalizedHealth());
        sensor.AddObservation(enemyController.GetSpellController().GetNormalizedMana());
        EnemyShieldObservations(sensor);
        EnemyHealObservations(sensor);
    }

    private void EnemyShieldObservations(VectorSensor sensor)
    {
        if (enemyController.IsBlocking() == false)
        {
            sensor.AddOneHotObservation(0, numberOfShields);   //Shield type
            sensor.AddObservation(0f);                         //Shield time
        }
        else
        {
            if (enemyMagicShield == null)
            {
                enemyMagicShield = enemy.GetComponent<MagicShield>();
            }
            sensor.AddOneHotObservation((int)enemyMagicShield.GetShieldType() + 1, numberOfShields);   //Shield type
            sensor.AddObservation(enemyMagicShield.GetNormalizedShieldTime());                         //Shield time
        }
    }

    private void EnemyHealObservations(VectorSensor sensor)
    {
        if (enemyController.IsHealing() == false)
        {
            sensor.AddObservation(0f); //Heal time
        }
        else
        {
            if (enemyHealSpell == null)
            {
                enemyHealSpell = enemy.GetComponent<HealSpell>();
            }
            sensor.AddObservation(enemyHealSpell.GetNormalizedHealTime()); //Heal time
        }
    }
}