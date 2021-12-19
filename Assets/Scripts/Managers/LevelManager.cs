using System;
using UnityEngine;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Policies;

public class LevelManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private GameLevelType levelType;
    public bool getLevelTypeFromAppManager;

    [Header("Spawn point")]
    [SerializeField] private Vector3 spawnPoint;
    [Space]
    [SerializeField] private float spawnPointMinWidth;
    [SerializeField] private float spawnPointMaxWidth;
    [Space]
    [SerializeField] private float spawnPointMinHeight;
    [SerializeField] private float spawnPointMaxHeight;
    [Space]
    [SerializeField] private Vector3 spectatorSpawnPoint;
    [SerializeField] private float yAxisLimit;

    [Header("Episode timing [s]")]
    [SerializeField] private float maxEpisodeTime;
    private float episodeTime;

    [Header("References")]
    [SerializeField] private GameObject jackPrefab;
    [SerializeField] private GameObject madoxPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spectatorPlayerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    private GameObject jack;
    private Mage jackController;
    private int jackScore;

    private GameObject madox;
    private Mage madoxController;
    private int madoxScore;

    private GameObject player;
    private PlayerController playerController;
    private PlayerLook playerLook;
    private int playerScore;

    private GameObject spectator;
    private SpectatorMoveCamera spectatorLook;
    private GameObject cameraObj;

    public void Startup()
    {
        Debug.Log("Starting Level manager");

        if(getLevelTypeFromAppManager)
        {
            levelType = Managers.App.GetLevelType();
        }

        if(levelType == GameLevelType.TRAINING)
        {
            TrainingLevelSetup();
        }
        
        if(levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayLevelSetup();
        }

        if(levelType == GameLevelType.PLAY)
        {
            PlayingLevelSetup();
        }

        SetupEpisodeTimeBar();
        status = ManagerStatus.Started;
    }

    public void LockApp(string reason)
    {
        levelType = GameLevelType.LOCKED;

        Destroy(jack);
        Destroy(madox);
        Destroy(player);
        Destroy(spectator);

        cameraObj = Instantiate(cameraPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        enabled = false;
    }

    private void Update()
    {
        EpisodeTimeLimit();
        WorldBorderLimit();
    }

    public void LevelReload()
    {
        if (levelType == GameLevelType.TRAINING)
        {
            TrainingLevelReload();
        }

        if (levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayLevelReload();
        }

        if(levelType == GameLevelType.PLAY)
        {
            PlayingLevelReload();
        }

        SetupEpisodeTimeBar();
        SetupGenerationLabel();
    }

    private void SetupGenerationLabel()
    {
        Managers.UI.SetupGenerationLabel(Managers.RlCsv.GetEpisodeCount());
    }

    public void EndEpisode(GameObject dead)
    {
        if (levelType == GameLevelType.TRAINING)
        {
            TrainingEndEpisode(dead);
        }

        if (levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayEndEpisode(dead);
        }

        if(levelType == GameLevelType.PLAY)
        {
            PlayingEndEpisode(dead);
        }
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

    /*Training methods*/
    private void TrainingLevelSetup()
    {
        player = Instantiate(playerPrefab, SpawnPointRandomLocation(), Quaternion.identity).transform.GetChild(0).gameObject;
        playerController = player.GetComponent<PlayerController>();
        playerLook = player.GetComponent<PlayerLook>();
        playerScore = 0;

        jack = Instantiate(jackPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        jackController = jack.GetComponent<Mage>();
        jackController.SetEnemy(player);
        jackScore = 0;

        jackController.SetRLAgent(jack.AddComponent<RLMagicAgentPlayerTraining>());
        jack.AddComponent<DecisionRequester>();
        jack.GetComponent<BehaviorParameters>().BehaviorName = Managers.App.GetBehaviourName();
    }

    private void TrainingLevelReload()
    {
        JackReload();
        PlayerReload();
        UIReload();
    }

    private void TrainingEndEpisode(GameObject dead)
    {
        if (dead == player)
        {
            jackScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().winEpisode);
            jackController.GetRLAgent().EndRLEpisode("Win");
        }

        if (dead == jack)
        {
            playerScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }

        if(dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");
        }
    }


    /*Self play methods*/
    private void SelfPlayLevelSetup()
    {
        madox = Instantiate(madoxPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        madoxController = madox.GetComponent<Mage>();
        madoxScore = 0;

        jack = Instantiate(jackPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        jackController = jack.GetComponent<Mage>();
        jackScore = 0;

        jackController.SetEnemy(madox);
        jackController.SetRLAgent(jack.AddComponent<RLMagicAgentSelfPlay>());
        jack.AddComponent<DecisionRequester>();
        jack.GetComponent<BehaviorParameters>().BehaviorName = Managers.App.GetBehaviourName();

        madoxController.SetEnemy(jack);
        madoxController.SetRLAgent(madox.AddComponent<RLMagicAgentSelfPlay>());
        madox.AddComponent<DecisionRequester>();
        madox.GetComponent<BehaviorParameters>().BehaviorName = Managers.App.GetBehaviourName();

        spectator = Instantiate(spectatorPlayerPrefab, spectatorSpawnPoint, Quaternion.identity);
        spectatorLook = spectator.transform.GetChild(0).GetComponent<SpectatorMoveCamera>();
    }

    private void SelfPlayLevelReload()
    {
        JackReload();
        MadoxReload();
        UIReload();
    }

    private void SelfPlayEndEpisode(GameObject dead)
    {
        if (dead == madox)
        {
            madoxController.AddRLReward(madoxController.GetMageRLParameters().loseEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Fail");

            jackScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().winEpisode);
            jackController.GetRLAgent().EndRLEpisode("Win");
        }

        if (dead == jack)
        {
            madoxScore++;
            madoxController.AddRLReward(madoxController.GetMageRLParameters().winEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Win");

            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }

        if(dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");

            madoxController.AddRLReward(madoxController.GetMageRLParameters().loseEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Draw");
        }
    }


    /*Playing methods*/
    private void PlayingLevelSetup()
    {
        player = Instantiate(playerPrefab, SpawnPointRandomLocation(), Quaternion.identity).transform.GetChild(0).gameObject;
        playerController = player.GetComponent<PlayerController>();
        playerLook = player.GetComponent<PlayerLook>();
        playerScore = 0;

        jack = Instantiate(jackPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        jackController = jack.GetComponent<Mage>();
        jackController.SetEnemy(player);
        jackScore = 0;

        jackController.SetRLAgent(jack.AddComponent<RLMagicAgentPlayerTraining>());
        jack.AddComponent<DecisionRequester>();

        BrainModelOverrider brainModelReader = new BrainModelOverrider();
        brainModelReader.OverrideModel(jack.GetComponent<Agent>(), Managers.App.GetBrainPath(), Managers.App.GetBehaviourName(), true);

        BehaviorParameters bf = jack.GetComponent<BehaviorParameters>();
        bf.BehaviorType = BehaviorType.InferenceOnly;
        bf.BehaviorName = Managers.App.GetBehaviourName();
    }

    private void PlayingLevelReload()
    {
        JackReload();
        PlayerReload();
        UIReload();
    }

    private void PlayingEndEpisode(GameObject dead)
    {
        if (dead == player)
        {
            jackScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().winEpisode);
            jackController.GetRLAgent().EndRLEpisode("Win");
        }

        if (dead == jack)
        {
            playerScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }

        if (dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");
        }
    }


    /*Reloads*/
    private void JackReload()
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
    }

    private void MadoxReload()
    {
        // Madox reload
        madox.transform.position = SpawnPointRandomLocation();
        madoxController.RefilHealth();
        madoxController.GetSpellController().RefilMana();
        try
        {
            madoxController.GetSpellController().GetCurrentShield().EndShield();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }

        try
        {
            madoxController.GetSpellController().GetCurrentHealSpell().EndHeal();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }
        madoxController.GetSpellController().ResetLastHittedSpellID();
    }

    private void PlayerReload()
    {
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
    }

    private void UIReload()
    {
        if (levelType == GameLevelType.TRAINING)
        {
            // Managers.UI.SetupGenerationLabel(jackController.GetRLAgent().CompletedEpisodes);
            jackController.GetUIPanelControll().SetupScore(jackScore);
            playerController.GetUIPanelControll().SetupScore(playerScore);
        }

        if (levelType == GameLevelType.SELF_PLAY)
        {
            // Managers.UI.SetupGenerationLabel(jackController.GetRLAgent().CompletedEpisodes);
            jackController.GetUIPanelControll().SetupScore(jackScore);
            madoxController.GetUIPanelControll().SetupScore(madoxScore);
        }
    }

    private void EpisodeTimeLimit()
    {
        episodeTime += Time.deltaTime;
        Managers.UI.ChangeEpisodeTimeBarValue(maxEpisodeTime - episodeTime);

        if (episodeTime >= maxEpisodeTime)
        {
            EndEpisode(null);
            SetupEpisodeTimeBar();
        }
    }

    private void WorldBorderLimit()
    {
        if(levelType == GameLevelType.TRAINING || levelType == GameLevelType.PLAY)
        {
            if (player.transform.position.y < yAxisLimit)
            {
                playerController.InstantKill();
            }
        }
    }


    private void SetupEpisodeTimeBar()
    {
        episodeTime = 0f;
        Managers.UI.SetupEpisodeTimeBar(maxEpisodeTime, maxEpisodeTime);
    }

    private Vector3 SpawnPointRandomLocation()
    {
        float xValue = UnityEngine.Random.Range(spawnPointMinWidth, spawnPointMaxWidth);
        float zValue = UnityEngine.Random.Range(spawnPointMinHeight, spawnPointMaxHeight);
        return spawnPoint + new Vector3(xValue, 0f, zValue);
    }

    public GameLevelType GetLevelType()
    {
        return levelType;
    }

    public void LockPlayerLook(bool locked)
    {
        try
        {
            playerLook.enabled = !locked;
        }
        catch (NullReferenceException err)
        {
            Debug.LogWarning(err.Message);
        }
    }

    public void LockSpectatorLook(bool locked)
    {
        try
        {
            spectatorLook.enabled = !locked;
        }
        catch (NullReferenceException err)
        {
            Debug.LogWarning(err.Message);
        }
    }

    public string GetLevelTypeName()
    {
        if(levelType == GameLevelType.LOCKED)
        {
            return "Locked";
        }
        else if (levelType == GameLevelType.TRAINING)
        {
            return "Training";
        }
        else if(levelType == GameLevelType.SELF_PLAY)
        {
            return "Self play";
        }
        else
        {
            return "Play";
        }
    }
}