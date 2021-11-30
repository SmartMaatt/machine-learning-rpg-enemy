using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(LevelManager))]
public class Managers : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    public static LevelManager Level { get; private set; }
    public static bool allLoaded { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        allLoaded = false;
        UI = GetComponent<UIManager>();
        Level = GetComponent<LevelManager>();

        _startSequence = new List<IGameManager>();

        _startSequence.Add(UI);
        _startSequence.Add(Level);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModels = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModels)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                    numReady++;
            }

            yield return null;
        }
        allLoaded = true;
    }
}
