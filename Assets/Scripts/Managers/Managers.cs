using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(RlCsvManager))]
[RequireComponent(typeof(ApplicationManager))]
public class Managers : MonoBehaviour
{
    public static Managers Self { get; private set; }
    public static ApplicationManager App { get; private set; }
    public static RlCsvManager RlCsv { get; private set; }
    public static UIManager UI { get; private set; }
    public static LevelManager Level { get; private set; }
    public static bool allLoaded { get; private set; }

    private List<IGameManager> startSequence;
    private IEnumerator StartupManagersCoroutine;

    private void Awake()
    {
        allLoaded = false;
        Self = this;
        App = GetComponent<ApplicationManager>();
        RlCsv = GetComponent<RlCsvManager>();
        UI = GetComponent<UIManager>();
        Level = GetComponent<LevelManager>();

        startSequence = new List<IGameManager>();

        startSequence.Add(App);
        startSequence.Add(RlCsv);
        startSequence.Add(UI);
        startSequence.Add(Level);

        StartupManagersCoroutine = StartupManagers();
        StartCoroutine(StartupManagersCoroutine);
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in startSequence)
        {
            manager.Startup();
            yield return null;
        }

        int numModels = startSequence.Count;
        int numReady = 0;

        while (numReady < numModels)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                    numReady++;
            }

            yield return null;
        }
        allLoaded = true;
    }

    public void LockApp(string reason)
    {
        StopCoroutine(StartupManagersCoroutine);

        foreach (IGameManager manager in startSequence)
        {
            manager.LockApp(reason);
        }
    }
}
