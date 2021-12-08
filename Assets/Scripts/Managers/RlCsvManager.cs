using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RlCsvManager : MonoBehaviour, IGameManager
{
    /*Params*/
    public ManagerStatus status { get; private set; }

    [Header("Configuration folder")]
    public string fileName;
    public string fileDirectoryName;
    [Space]
    public bool persistentDataPath;
    public bool getPathFromAppManagerSession;

    private string fullFileDirectoryPath;
    private string fullFilePath;

    private List<string[]> learningData = new List<string[]>();
    private string[] learningDataCSVHeader = {
                                                "#",
                                                "Time",
                                                "RL points",
                                                "Agent name",
                                                "Learning type",
                                                "Won Episode",
                                                "Low health threshold",
                                                "Critical low health threshold",
                                                "Walk speed",
                                                "Run speed",
                                                "Sight range",
                                                "Hear range",
                                                "Attack range"
                                            };

    /*Startup*/
    public void Startup()
    {
        Debug.Log("Starting RL CSV manager");

        if (getPathFromAppManagerSession)
        {
            fileDirectoryName = Managers.App.GetSessionName();
        }

        GenerateFullPath();

        if (!Managers.App.IsAppLocked())
        {
            ReadCSV();
        }

        status = ManagerStatus.Started;
    }

    public void WriteEmptyRLCSV()
    {
        learningData.Add(learningDataCSVHeader);
        WriteCSV();
    }

    public void WriteCSV()
    {
        string currentLine = "";
        using (StreamWriter sw = new StreamWriter(fullFilePath))
        {
            for (int i = 0; i < learningData.Count; i++)
            {
                for (int j = 0; j < learningData[i].Length; j++)
                {
                    currentLine += learningData[i][j];
                    if (j != (learningData[i].Length - 1))
                    {
                        currentLine += ";";
                    }
                }

                sw.WriteLine(currentLine);
                currentLine = "";
            }
        }
    }

    public void ReadCSV()
    {
        //No csv directory
        if (!Directory.Exists(fullFileDirectoryPath))
        {
            Debug.LogWarning("No directory: " + fullFileDirectoryPath);
            DirectoryManager.CreateDirectory(fullFileDirectoryPath);
        }

        //No csv file
        if (!File.Exists(fullFilePath))
        {
            Debug.LogWarning("No file: " + fullFilePath);
            WriteEmptyRLCSV();
        }

        using (StreamReader sr = new StreamReader(fullFilePath))
        {
            learningData = new List<string[]>();
            while (!sr.EndOfStream)
            {
                learningData.Add(sr.ReadLine().Split(';'));
            }
        }
    }

    public void GenerateFullPath()
    {
        if (persistentDataPath)
        {
            if (string.IsNullOrEmpty(fileDirectoryName))
            {
                fullFilePath = Application.persistentDataPath + "/" + fileName;
                fullFileDirectoryPath = Application.persistentDataPath;
            }
            else
            {
                fullFilePath = Application.persistentDataPath + "/" + fileDirectoryName + "/" + fileName;
                fullFileDirectoryPath = Application.persistentDataPath + "/" + fileDirectoryName;
            }
        }
        else
        {
            fullFilePath = fileDirectoryName + "/" + fileName;
            fullFileDirectoryPath = fileDirectoryName;
        }
    }

    public void AddEpisodeData(string[] episodeData)
    {
        learningData.Add(episodeData);
        WriteCSV();
    }

    public int GetEpisodeCount()
    {
        return learningData.Count;
    }
}