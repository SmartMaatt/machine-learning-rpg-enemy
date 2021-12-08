using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private GameLevelType levelType;
    [SerializeField] private string sessionName;
    [SerializeField] private string mlBrainName;
    [Space]
    [SerializeField] private bool appLocked;
    [SerializeField] private string lockageReason;
    [Space]
    public int timeWaitingForBrain;

    private string savePath;
    private string saveDirectoryPath;
    private string brainPath;


    /*Startup*/
    public void Startup()
    {
        Debug.Log("Starting Application manager");

        savePath = Application.persistentDataPath + "/" + sessionName + "/" + mlBrainName + ".onnx";
        saveDirectoryPath = Application.persistentDataPath + "/" + sessionName;
        brainPath = "results/" + sessionName + "/" + mlBrainName + ".onnx";

        status = ManagerStatus.Started;
    }

    private void OnApplicationQuit()
    {
        try
        {
            CreateSaveDirectory();
            Managers.UI.DisplayPopUpMessageWithTime("Saving brain...", timeWaitingForBrain);
            StartCoroutine(CopyMLBrainToSaveDirectory(timeWaitingForBrain));
        }
        catch (IOException ex)
        {
            LockApp(ex.Message);
        }
    }

    private void CreateSaveDirectory()
    {
        if (!Directory.Exists(saveDirectoryPath))
        {
            Debug.LogWarning("No directory: " + saveDirectoryPath);
            DirectoryManager.CreateDirectory(saveDirectoryPath);
        }
    }

    private IEnumerator CopyMLBrainToSaveDirectory(int time)
    {
        int counter = time;
        while (counter != 0)
        {
            try
            {
                File.Copy(brainPath, savePath);
                break;
            }
            catch (IOException)
            {
                counter--;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void LockApp(string reason)
    {
        levelType = GameLevelType.LOCKED;
        lockageReason = reason;
    }

    /*Getters*/
    public GameLevelType GetLevelType()
    {
        return levelType;
    }

    public string GetSessionName()
    {
        return sessionName;
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