using UnityEngine;
using Unity.MLAgents.Sensors;

public class RLMagicAgentPlayerTraining : RLMagicAgent
{
    [Header("Player observations references")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpellController playerSpellController;
    [SerializeField] private MagicShield playerMagicShield;
    [SerializeField] private HealSpell playerHealSpell;

    protected override void Start()
    {
        base.Start();
        player = entity.GetEnemy();
        playerController = player.GetComponent<PlayerController>();
        playerSpellController = playerController.GetSpellController();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(playerController.GetNormalizedHealth());
        sensor.AddObservation(playerController.GetSpellController().GetNormalizedMana());
        PlayerShieldObservations(sensor);
        PlayerHealObservations(sensor);
    }

    private void PlayerShieldObservations(VectorSensor sensor)
    {
        if (playerController.IsBlocking() == false)
        {
            sensor.AddOneHotObservation(0, numberOfShields);   //Shield type
            sensor.AddObservation(0f);                         //Shield time
        }
        else
        {
            if (playerMagicShield == null)
            {
                playerMagicShield = player.GetComponent<MagicShield>();
            }
            sensor.AddOneHotObservation((int)playerMagicShield.GetShieldType() + 1, numberOfShields);   //Shield type
            sensor.AddObservation(playerMagicShield.GetNormalizedShieldTime());                         //Shield time
        }
    }

    private void PlayerHealObservations(VectorSensor sensor)
    {
        if (playerController.IsHealing() == false)
        {
            sensor.AddObservation(0f); //Heal time
        }
        else
        {
            if (playerHealSpell == null)
            {
                playerHealSpell = player.GetComponent<HealSpell>();
            }
            sensor.AddObservation(playerHealSpell.GetNormalizedHealTime()); //Heal time
        }
    }
}