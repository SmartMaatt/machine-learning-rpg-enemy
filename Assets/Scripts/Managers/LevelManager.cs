using System;
using UnityEngine;
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

        if (getLevelTypeFromAppManager)
        {
            levelType = Managers.App.GetLevelType();
        }
        SetupLevel();

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

    public void SetupLevel()
    {
        if (levelType == GameLevelType.TRAINING)
        {
            TrainingLevelSetup();
        }
        else if (levelType == GameLevelType.SELF_PLAY_TRAINING)
        {
            SelfPlayTrainingLevelSetup();
        }
        else if (levelType == GameLevelType.PLAY)
        {
            PlayingLevelSetup();
        }
        else if (levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayLevelSetup();
        }

        SetupEpisodeTimeBar();
    }

    public void LevelReload()
    {
        if (levelType == GameLevelType.TRAINING)
        {
            TrainingLevelReload();
        }
        else if (levelType == GameLevelType.SELF_PLAY_TRAINING)
        {
            SelfPlayTrainingLevelReload();
        }
        else if (levelType == GameLevelType.PLAY)
        {
            PlayingLevelReload();
        }
        else if (levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayLevelReload();
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
        else if (levelType == GameLevelType.SELF_PLAY_TRAINING)
        {
            SelfPlayTrainingEndEpisode(dead);
        }
        else if (levelType == GameLevelType.PLAY)
        {
            PlayingEndEpisode(dead);
        }
        else if (levelType == GameLevelType.SELF_PLAY)
        {
            SelfPlayEndEpisode(dead);
        }
    }

    public void ValidateLevelEntities(GameObject entity)
    {
        if (levelType == GameLevelType.TRAINING)
        {
            if (entity != jack && entity != player)
            {
                Destroy(entity);
            }
        }
    }


    /*>>> Training methods <<<*/
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

        Managers.UI.ActivateGiveUpButton(true);
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
        else if (dead == jack)
        {
            playerScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }
        else if (dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().drawEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");
        }
        LevelReload();
    }


    /*>>> Self play training methods <<<*/
    private void SelfPlayTrainingLevelSetup()
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

    private void SelfPlayTrainingLevelReload()
    {
        JackReload();
        MadoxReload();
        UIReload();
    }

    private void SelfPlayTrainingEndEpisode(GameObject dead)
    {
        if (dead == madox)
        {
            madoxController.AddRLReward(madoxController.GetMageRLParameters().loseEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Fail");

            jackScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().winEpisode);
            jackController.GetRLAgent().EndRLEpisode("Win");
        }
        else if (dead == jack)
        {
            madoxScore++;
            madoxController.AddRLReward(madoxController.GetMageRLParameters().winEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Win");

            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }
        else if (dead == null)
        {
            madoxController.AddRLReward(madoxController.GetMageRLParameters().drawEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Draw");

            jackController.AddRLReward(jackController.GetMageRLParameters().drawEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");
        }
        LevelReload();
    }


    /*>>> Playing methods <<<*/
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

        Managers.UI.ActivateGiveUpButton(true);
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
        else if (dead == jack)
        {
            playerScore++;
            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }
        else if (dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().drawEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");
        }
        LevelReload();
    }


    /*>>> Self play methods <<<*/
    private void SelfPlayLevelSetup()
    {
        madox = Instantiate(madoxPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        madoxController = madox.GetComponent<Mage>();
        madoxScore = 0;

        jack = Instantiate(jackPrefab, SpawnPointRandomLocation(), Quaternion.identity);
        jackController = jack.GetComponent<Mage>();
        jackScore = 0;

        madoxController.SetEnemy(jack);
        madoxController.SetRLAgent(madox.AddComponent<RLMagicAgentSelfPlay>());
        madox.AddComponent<DecisionRequester>();
        BehaviorParameters madoxBF = madox.GetComponent<BehaviorParameters>();

        jackController.SetEnemy(madox);
        jackController.SetRLAgent(jack.AddComponent<RLMagicAgentSelfPlay>());
        jack.AddComponent<DecisionRequester>();
        BehaviorParameters jackBF = jack.GetComponent<BehaviorParameters>();

        BrainModelOverrider brainModelReader = new BrainModelOverrider();
        brainModelReader.OverrideModel(jack.GetComponent<Agent>(), Managers.App.GetBrainPath(), Managers.App.GetBehaviourName(), true);
        brainModelReader.OverrideModel(madox.GetComponent<Agent>(), Managers.App.GetBrainPath(), Managers.App.GetBehaviourName(), true);

        jackBF.BehaviorType = BehaviorType.InferenceOnly;
        jackBF.BehaviorName = Managers.App.GetBehaviourName();

        madoxBF.BehaviorType = BehaviorType.InferenceOnly;
        madoxBF.BehaviorName = Managers.App.GetBehaviourName();

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
        else if (dead == jack)
        {
            madoxScore++;
            madoxController.AddRLReward(madoxController.GetMageRLParameters().winEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Win");

            jackController.AddRLReward(jackController.GetMageRLParameters().loseEpisode);
            jackController.GetRLAgent().EndRLEpisode("Fail");
        }
        else if (dead == null)
        {
            jackController.AddRLReward(jackController.GetMageRLParameters().drawEpisode);
            jackController.GetRLAgent().EndRLEpisode("Draw");

            madoxController.AddRLReward(madoxController.GetMageRLParameters().drawEpisode);
            madoxController.GetRLAgent().EndRLEpisode("Draw");
        }
        LevelReload();
    }


    /*>>> Reloads <<<*/
    private void JackReload()
    {
        // Jack reload
        jack.transform.position = SpawnPointRandomLocation();
        jackController.RefilHealth();
        jackController.ResetEnemyInterestTime();

        jackController.GetSpellController().RefilMana();
        jackController.GetSpellController().ResetLastHittedSpellID();
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
    }

    private void MadoxReload()
    {
        // Madox reload
        madox.transform.position = SpawnPointRandomLocation();
        madoxController.RefilHealth();
        madoxController.ResetEnemyInterestTime();

        madoxController.GetSpellController().RefilMana();
        madoxController.GetSpellController().ResetLastHittedSpellID();
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

    public void PlayerGiveUp()
    {
        try
        {
            playerController.InstantKill();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void UIReload()
    {
        if (Managers.App.IsPlayerInGame())
        {
            jackController.GetUIPanelControll().SetupScore(jackScore);
            playerController.GetUIPanelControll().SetupScore(playerScore);
        }
        else
        {
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

    public float GetEpisodeTimeLimit()
    {
        return episodeTime;
    }

    public float GetMaxEpisodeTimeLimit()
    {
        return maxEpisodeTime;
    }

    public float GetNormalizedEpisodeTimeLimit()
    {
        return (episodeTime / maxEpisodeTime);
    }

    private void WorldBorderLimit()
    {
        if (levelType == GameLevelType.TRAINING || levelType == GameLevelType.PLAY)
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
        float xValue = 0f;
        float zValue = 0f;
        bool correctLocation = false;

        RaycastHit hit;
        Vector3 location = spawnPoint;
        Vector3 positionCorrection = new Vector3(0f, 1f, 0f);

        while (!correctLocation)
        {
            xValue = UnityEngine.Random.Range(spawnPointMinWidth, spawnPointMaxWidth);
            zValue = UnityEngine.Random.Range(spawnPointMinHeight, spawnPointMaxHeight);

            location = spawnPoint + new Vector3(xValue, 200f, zValue);
            Physics.Raycast(location, -transform.up, out hit);

            correctLocation = (hit.collider.gameObject.GetComponent<SolidGround>() != null);
            location = hit.point + positionCorrection;
        }
        return location;
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
            Debug.Log(err.Message);
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
            Debug.Log(err.Message);
        }
    }

    public string GetLevelTypeName()
    {
        if (levelType == GameLevelType.LOCKED)
        {
            return "Locked";
        }
        else if (levelType == GameLevelType.TRAINING)
        {
            return "Training";
        }
        else if (levelType == GameLevelType.SELF_PLAY_TRAINING)
        {
            return "Self play";
        }
        else
        {
            return "Play";
        }
    }
}