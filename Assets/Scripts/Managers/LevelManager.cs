using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private GameLevelType levelType;

    [Header("Spawn point")]
    [SerializeField] private Vector3 spawnPoint;
    [Space]
    [SerializeField] private float spawnPointMinWidth;
    [SerializeField] private float spawnPointMaxWidth;
    [Space]
    [SerializeField] private float spawnPointMinHeight;
    [SerializeField] private float spawnPointMaxHeight;

    [Header("References")]
    [SerializeField] private GameObject jackPrefab;
    [SerializeField] private GameObject playerPrefab;

    private GameObject jack;
    private Mage jackController;
    private int jackScore;

    private GameObject player;
    private PlayerController playerController;
    private int playerScore;


    public void Startup()
    {
        Debug.Log("Starting Level manager");

        if(levelType == GameLevelType.TRAINING)
        {
            TrainingLevelSetup();
        }

        status = ManagerStatus.Started;
    }

    public void ValidateLevelEntities(GameObject entity)
    {
        if(levelType == GameLevelType.TRAINING)
        {
            if (entity != jack && entity != player)
            {
                Destroy(entity);
            }
        }
    }

    private void TrainingLevelSetup()
    {
        player = Instantiate(playerPrefab, SpawnPointRandomLocation(), Quaternion.identity).transform.GetChild(0).gameObject;
        playerController = player.GetComponent<PlayerController>();
        playerScore = 0;

        jack = Instantiate(jackPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        jackController = jack.GetComponent<Mage>();
        jackController.SetPlayer(player);
        jackScore = 0;

        jackController.SetRLAgent(jack.AddComponent<RLMagicAgentPlayerTraining>());
        jack.AddComponent<Unity.MLAgents.DecisionRequester>();
    }

    public void TrainingLevelReload()
    {
        // Jack reload
        jack.transform.position = SpawnPointRandomLocation();
        jackController.RefilHealth();
        jackController.GetSpellController().RefilMana();
        try
        {
            jackController.GetSpellController().GetCurrentShield().EndShield();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }

        try
        {
            jackController.GetSpellController().GetCurrentHealSpell().EndHeal();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }
        jackController.GetSpellController().ResetLastHittedSpellID();

        // Player reload
        player.transform.position = SpawnPointRandomLocation();
        playerController.ReloadHealth();
        playerController.GetSpellController().RefilMana();
        try
        {
            playerController.GetSpellController().GetCurrentShield().EndShield();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }

        try
        {
            playerController.GetSpellController().GetCurrentHealSpell().EndHeal();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }

        // Managers.UI.SetupGenerationLabel(jackController.GetRLAgent().CompletedEpisodes);
        jackController.GetUIPanelControll().SetupScore(jackScore);
        playerController.GetUIPanelControll().SetupScore(playerScore);
    }

    public void EndEpisode(GameObject dead)
    {
        if (levelType == GameLevelType.TRAINING)
        {
            if (dead == player)
            {
                jackScore++;
                jackController.AddRLReward(jackController.GetMageRLParameters().winEpisode);
                jackController.GetRLAgent().EndEpisode();
            }

            if (dead == jack)
            {
                playerScore++;
                jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
                jackController.GetRLAgent().EndEpisode();
            }
        }
    }

    private Vector3 SpawnPointRandomLocation()
    {
        float xValue = UnityEngine.Random.Range(spawnPointMinWidth, spawnPointMaxWidth);
        float zValue = UnityEngine.Random.Range(spawnPointMinHeight, spawnPointMaxHeight);
        return spawnPoint + new Vector3(xValue, 0f, zValue);
    }
}