using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // public static TestManager Test { get; private set; } // Example line

    public static bool allLoaded { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        allLoaded = false;
        // Test = GetComponent<TestManager>(); // Example line

        _startSequence = new List<IGameManager>();
        
        // _startSequence.Add(Test); // Example line

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
