using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpMessager : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private Image background;
    [SerializeField] private float timeOfDisplay;

    private IEnumerator popUpLifeCoroutine;

    private void Start()
    {
        SetupMessage("");
        SetPopUpActive(false);
        popUpLifeCoroutine = null;
    }

    public void DisplayMessage(string msg)
    {
        if (popUpLifeCoroutine != null)
        {
            StopCoroutine(popUpLifeCoroutine);
        }

        popUpLifeCoroutine = RunPopUp(msg);
        StartCoroutine(popUpLifeCoroutine);
    }

    private void SetPopUpActive(bool state)
    {
        background.enabled = state;
        message.gameObject.SetActive(state);
    }

    public void SetTimeOfDisplay(float time)
    {
        timeOfDisplay = time;
    }

    private void SetupMessage(string msg)
    {
        message.text = msg;
    }

    private IEnumerator RunPopUp(string msg)
    {
        SetPopUpActive(true);
        SetupMessage(msg);

        yield return new WaitForSeconds(timeOfDisplay);

        SetupMessage("");
        SetPopUpActive(false);
        popUpLifeCoroutine = null;
    }
}