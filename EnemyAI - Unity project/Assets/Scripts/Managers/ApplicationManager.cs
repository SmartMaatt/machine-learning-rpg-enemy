using System;
using System.IO;
using UnityEngine;

public class ApplicationManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public bool getInfoFromConfigFile;

    [Space]
    [SerializeField] private GameLevelType levelType;
    [SerializeField] private string configPath;
    [SerializeField] private string behaviourName;
    [SerializeField] private string mlBrainDirectoryPath;
    [SerializeField] private string mlBrainSessionName;
    [SerializeField] private string mlBrainName;

    [Space]
    public int timeWaitingForBrain;
    public float appTimeScale;

    private string brainPath;
    private bool newProfile;
    private bool levelTypeData, brainSessionNameData, timeScaleData, newProfileData;


    /*>>> Startup <<<*/
    public void Startup()
    {
        Debug.Log("Starting Application manager");

        if (getInfoFromConfigFile)
        {
            ReadConfigFile();
        }

        SetupBrainPath(mlBrainDirectoryPath, mlBrainName);
        if (!newProfile && !File.Exists(brainPath))
        {
            Managers.Self.LockApp("Model " + brainPath + " nie istnieje!");
        }

        if (IsPlaying())
        {
            Screen.fullScreen = true;
        }

        status = ManagerStatus.Started;
    }

    public void LockApp(string reason)
    {
        levelType = GameLevelType.LOCKED;
        enabled = false;
    }

    private void ReadConfigFile()
    {
        try
        {
            using (StreamReader sr = new StreamReader(configPath))
            {
                int line = 0;
                string[] configLine;
                while (!sr.EndOfStream)
                {
                    line++;
                    configLine = sr.ReadLine().Split('=');

                    if (configLine.Length != 2)
                    {
                        Managers.Self.LockApp("Niepoprawna składnia pliku konfiguracyjnego w linii " + line + "!\nLinia zawiera więcej niż dwie wartości!");
                        break;
                    }

                    if (line > 4)
                    {
                        Managers.Self.LockApp("Niepoprawna składnia pliku!\nWięcej niż cztery linie w pliku konfiguracyjnym!");
                        break;
                    }

                    if (configLine[0] == "LevelType")
                    {
                        levelTypeData = true;
                        if (configLine[1] == "Training")
                        {
                            levelType = GameLevelType.TRAINING;
                        }
                        else if (configLine[1] == "SelfPlayTraining")
                        {
                            levelType = GameLevelType.SELF_PLAY_TRAINING;
                        }
                        else if (configLine[1] == "Play")
                        {
                            levelType = GameLevelType.PLAY;
                        }
                        else if (configLine[1] == "SelfPlay")
                        {
                            levelType = GameLevelType.SELF_PLAY;
                        }
                        else
                        {
                            Managers.Self.LockApp("Niepoprawna składnia pliku konfiguracyjnego w linii " + line + "!\nNieznana wartość parametru LevelType!");
                            levelTypeData = false;
                            break;
                        }
                    }
                    else if (configLine[0] == "MLBrainSessionName")
                    {
                        SetupMlBrainDirectoryPath(configLine[1]);
                        brainSessionNameData = true;
                    }
                    else if (configLine[0] == "TimeScale")
                    {
                        try
                        {
                            appTimeScale = float.Parse(configLine[1]);
                            timeScaleData = true;
                        }
                        catch (FormatException err)
                        {
                            Managers.Self.LockApp(err.Message + "\n" + configLine[1] + " nie jest wartością typu float!");
                        }
                    }
                    else if (configLine[0] == "NewProfile")
                    {
                        try
                        {
                            newProfile = bool.Parse(configLine[1]);
                            newProfileData = true;
                        }
                        catch (FormatException err)
                        {
                            Managers.Self.LockApp(err.Message + "\n" + configLine[1] + " nie jest wartością typu bool!");
                        }
                    }
                    else
                    {
                        Managers.Self.LockApp("Niepoprawna składnia pliku konfiguracyjnego w linii " + line + "!\nNieznany parametr " + configLine[0] + "!");
                        break;
                    }
                }

                if (line != 4 || !levelTypeData || !brainSessionNameData || !timeScaleData || !newProfileData)
                {
                    Managers.Self.LockApp("Niepoprawna składnia pliku konfiguracyjnego!\nZbyt mała ilość parametrów lub są one powtórzone!");
                }
            }
        }
        catch (IOException err)
        {
            if(File.Exists("../../EnemyAILauncher.exe"))
            {
                Managers.Self.LockApp("To nie jest plik exe którego szukacie!");
            }
            else
            {
                Managers.Self.LockApp(err.Message + "\n Plik konfiguracyjny nie mógł zostać otwarty!");
            }
        }
    }

    private void SetupBrainPath(string brainDirectory, string brainName)
    {
        brainPath = brainDirectory + "/" + brainName + ".onnx";
    }

    private void SetupMlBrainDirectoryPath(string brainSessionName)
    {
        string rootDirectory = "";

        mlBrainDirectoryPath = rootDirectory + "results/" + brainSessionName;
        mlBrainSessionName = brainSessionName;
    }


    /*>>> Getters <<<*/
    public GameLevelType GetLevelType()
    {
        return levelType;
    }

    public string GetBehaviourName()
    {
        return behaviourName;
    }

    public string GetBrainPath()
    {
        return brainPath;
    }

    public string GetBrainDirectoryPath()
    {
        return mlBrainDirectoryPath;
    }

    public string GetBrainSessionName()
    {
        return mlBrainSessionName;
    }

    public bool IsAppLocked()
    {
        return levelType == GameLevelType.LOCKED;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = appTimeScale;
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public bool IsPlaying()
    {
        return (levelType == GameLevelType.PLAY || levelType == GameLevelType.SELF_PLAY);
    }

    public bool IsTraining()
    {
        return (levelType == GameLevelType.SELF_PLAY_TRAINING || levelType == GameLevelType.TRAINING);
    }

    public bool IsPlayerInGame()
    {
        return (levelType == GameLevelType.PLAY || levelType == GameLevelType.TRAINING);
    }
}