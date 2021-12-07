using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RlCsvManager : MonoBehaviour, IGameManager
{
    /*Params*/
    public ManagerStatus status { get; private set; }

    [Header("Configuration folder")]
    public string fileName;
    public string fileDirectoryPath;
    public bool persistentDataPath;
    private string fullFilePath;

    [Header("CSV configuration")]
    public int columnsCount = 6;

    private List<string[]> learningData = new List<string[]>();
    private string[] learningDataCSVHeader = { "#", "Time", "RL points", "Agent name", "Learning type", "Won Episode" };

    /*Startup*/
    public void Startup()
    {
        Debug.Log("Starting RL CSV manager");

        GenerateFullPath();
        ReadCSV();

        status = ManagerStatus.Started;
    }

    private void OnApplicationQuit()
    {
        WriteCSV();
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
        if (!Directory.Exists(fileDirectoryPath))
        {
            Debug.LogWarning("No directory: " + fileDirectoryPath);
            if(!string.IsNullOrEmpty(fileDirectoryPath))
            {
                DirectoryManager.CreateDirectory(fileDirectoryPath);
            }
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
            if(string.IsNullOrEmpty(fileDirectoryPath))
            {
                fullFilePath = Application.persistentDataPath + "/" + fileName;
            }
            else
            {
                fullFilePath = Application.persistentDataPath + "/" + fileDirectoryPath + "/" + fileName;
            }
        }
        else
        {
            fullFilePath = fileDirectoryPath + "/" + fileName;
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