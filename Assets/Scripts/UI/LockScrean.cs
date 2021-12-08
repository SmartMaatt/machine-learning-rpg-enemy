using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockScrean : MonoBehaviour
{
    [SerializeField] private TMP_Text headerTMP;
    [SerializeField] private TMP_Text reasonTMP;

    public void SetReason(string reason)
    {
        reasonTMP.text = "Reason: " + reason;
    }
}
