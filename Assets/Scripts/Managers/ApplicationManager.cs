using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private GameLevelType levelType;
    [SerializeField] private string configPath;
    [SerializeField] private string behaviourName;
    [SerializeField] private string mlBrainDirectoryPath;
    [SerializeField] private string mlBrainSessionName;
    [SerializeField] private string mlBrainName;
    [Space]
    [SerializeField] private string lockageReason;
    [Space]
    public int timeWaitingForBrain;

    private string brainPath;


    /*Startup*/
    public void Startup()
    {
        Debug.Log("Starting Application manager");

        ReadConfigFile();
        brainPath = mlBrainDirectoryPath + "/" + mlBrainName + ".onnx";

        status = ManagerStatus.Started;
    }

    public void LockApp(string reason)
    {
        levelType = GameLevelType.LOCKED;
        lockageReason = reason;
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

                    if(configLine[0] == "LevelType")
                    {
                        if(configLine[1] == "Training")
                        {
                            levelType = GameLevelType.TRAINING;
                        }
                        else if(configLine[1] == "SelfPlay")
                        {
                            levelType = GameLevelType.SELF_PLAY;
                        }
                        else if(configLine[1] == "Play")
                        {
                            levelType = GameLevelType.PLAY;
                        }
                        else
                        {
                            Managers.Self.LockApp("Incorrect file syntax on line " + line + "!\nUnknown LevelType!");
                            break;
                        }
                    }
                    else if(configLine[0] == "MLBrainSessionName")
                    {
                        mlBrainDirectoryPath = "results/" + configLine[1];
                        mlBrainSessionName = configLine[1];
                    }
                    else
                    {
                        Managers.Self.LockApp("Incorrect file syntax on line " + line + "!\nUnknown argument " + configLine[0] + "!");
                        break;
                    }
                }
            }
        }
        catch (IOException err)
        {
            Managers.Self.LockApp(err.Message + "\n The config file cannot be opened!");
        }
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

    public string GetLockageReason()
    {
        return lockageReason;
    }
}