using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
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
        player = entity.GetPlayer();
        playerController = player.GetComponent<PlayerController>();
        playerSpellController = playerController.GetSpellController();
    }

    public override void OnEpisodeBegin()
    {
        Managers.Level.TrainingLevelReload();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(playerController.GetPlayerHealth());
        sensor.AddObservation(playerController.GetSpellController().GetMana());
        PlayerShieldObservations(sensor);
        PlayerHealObservations(sensor);
    }

    private void PlayerShieldObservations(VectorSensor sensor)
    {
        if (playerController.IsBlocking() == false)
        {
            sensor.AddObservation(0);   //Shield type
            sensor.AddObservation(0f);  //Shield time
        }
        else
        {
            if (playerMagicShield == null)
            {
                playerMagicShield = player.GetComponent<MagicShield>();
            }
            sensor.AddObservation((int)playerMagicShield.GetShieldType());  //Shield type
            sensor.AddObservation(playerMagicShield.GetShieldTime());       //Shield time
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
                playerHealSpell = GetComponent<HealSpell>();
            }
            sensor.AddObservation(playerHealSpell.GetHealTime()); //Heal time
        }
    }
}