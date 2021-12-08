using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private GameLevelType levelType;
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

        brainPath = mlBrainDirectoryPath + "/" + mlBrainName + ".onnx";

        status = ManagerStatus.Started;
    }

    public void LockApp(string reason)
    {
        levelType = GameLevelType.LOCKED;
        lockageReason = reason;
        enabled = false;
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