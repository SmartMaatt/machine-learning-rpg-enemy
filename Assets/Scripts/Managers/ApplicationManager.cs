using System;
using System.Collections;
using System.Collections.Generic;
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


    /*Startup*/
    public void Startup()
    {
        Debug.Log("Starting Application manager");

        SetupConfigFile();
        if (getInfoFromConfigFile)
        {
            ReadConfigFile();
        }

        SetupBrainPath(mlBrainDirectoryPath, mlBrainName);
        if(!newProfile && !File.Exists(brainPath))
        {
            Managers.Self.LockApp("Brain " + brainPath + " doesn't exist!");
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

                    if(configLine.Length != 2)
                    {
                        Managers.Self.LockApp("Incorrect file syntax on line " + line + "!\nMore then two values in one line!");
                        break;
                    }

                    if(line > 4)
                    {
                        Managers.Self.LockApp("Incorrect file syntax!\nMore then four parameters in config file!");
                        break;
                    }

                    if(configLine[0] == "LevelType")
                    {
                        levelTypeData = true;
                        if(configLine[1] == "Training")
                        {
                            levelType = GameLevelType.TRAINING;
                        }
                        else if(configLine[1] == "SelfPlayTraining")
                        {
                            levelType = GameLevelType.SELF_PLAY_TRAINING;
                        }
                        else if(configLine[1] == "Play")
                        {
                            levelType = GameLevelType.PLAY;
                        }
                        else if(configLine[1] == "SelfPlay")
                        {
                            levelType = GameLevelType.SELF_PLAY;
                        }
                        else
                        {
                            Managers.Self.LockApp("Incorrect file syntax on line " + line + "!\nUnknown LevelType!");
                            levelTypeData = false;
                            break;
                        }
                    }
                    else if(configLine[0] == "MLBrainSessionName")
                    {
                        SetupMlBrainDirectoryPath(configLine[1]);
                        brainSessionNameData = true;
                    }
                    else if(configLine[0] == "TimeScale")
                    {
                        try
                        {
                            appTimeScale = float.Parse(configLine[1]);
                            timeScaleData = true;
                        }
                        catch (FormatException err)
                        {
                            Managers.Self.LockApp(err.Message + "\n" + configLine[1] + " is not a float value!");
                        }
                    }
                    else if(configLine[0] == "NewProfile")
                    {
                        try
                        {
                            newProfile = bool.Parse(configLine[1]);
                            newProfileData = true;
                        }
                        catch (FormatException err)
                        {
                            Managers.Self.LockApp(err.Message + "\n" + configLine[1] + " is not a bool value!");
                        }
                    }
                    else
                    {
                        Managers.Self.LockApp("Incorrect file syntax on line " + line + "!\nUnknown argument " + configLine[0] + "!");
                        break;
                    }
                }

                if(line != 4 || !levelTypeData || !brainSessionNameData || !timeScaleData || !newProfileData)
                {
                    Managers.Self.LockApp("Incorrect file syntax!\nThere are too few parameters or they are repeated!");
                }
            }
        }
        catch (IOException err)
        {
            Managers.Self.LockApp(err.Message + "\n The config file cannot be opened!");
        }
    }

    private void SetupConfigFile()
    {
        #if UNITY_STANDALONE && !UNITY_EDITOR
            configPath = "../" + configPath;
        #endif
    }

    private void SetupBrainPath(string brainDirectory, string brainName)
    {
        brainPath = brainDirectory + "/" + brainName + ".onnx";
    }

    private void SetupMlBrainDirectoryPath(string brainSessionName)
    {
        string rootDirectory = "";

        #if UNITY_STANDALONE && !UNITY_EDITOR
            rootDirectory = "../";
        #endif

        mlBrainDirectoryPath = rootDirectory + "results/" + brainSessionName;
        mlBrainSessionName = brainSessionName;
    }

    /*Getters*/
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