using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private NonePanelControll nonePanel;

    [Header("Left panel")]
    [SerializeField] private GameObject leftPanelOwner;
    [SerializeField] private RealPanelControll leftPanel;

    [Header("Right panel")]
    [SerializeField] private GameObject rightPanelOwner;
    [SerializeField] private RealPanelControll rightPanel;

    [Header("Element selector")]
    [SerializeField] private GameObject elementBarOwner;
    [SerializeField] private ElementBar elementBar;

    [Header("Elements sprites")]
    [SerializeField] private Sprite fire;
    [SerializeField] private Sprite water;
    [SerializeField] private Sprite snow;

    [Header("PopUp display panel")]
    [SerializeField] private PopUpMessager popUpMessager;


    public void Startup()
    {
        Debug.Log("Starting UI manager");

        nonePanel = new NonePanelControll();
        leftPanel.gameObject.SetActive(false);
        rightPanel.gameObject.SetActive(false);

        elementBar.SetupBarSprites(fire, water, snow);
        elementBar.gameObject.SetActive(false); 

        status = ManagerStatus.Started;
    }

    public PanelControll SetupUIPanelController(GameObject owner, PanelType type)
    {
        if(type == PanelType.LEFT)
        {
            if(leftPanelOwner == null)
            {
                SetLeftPanelOwner(owner);
                return leftPanel;
            }

            if(leftPanelOwner == owner)
            {
                return leftPanel;
            }
        }

        if(type == PanelType.RIGHT)
        {
            if(rightPanelOwner == null)
            {
                SetRightPanelOwner(owner);
                return rightPanel;
            }

            if(rightPanelOwner == owner)
            {
                return rightPanel;
            }
        }

        return nonePanel;
    }

    public ElementBar SetupElementBar(GameObject owner)
    {
        if(elementBarOwner == null)
        {
            SetElementBarOwner(owner);
            return elementBar;
        }

        if(elementBarOwner == owner)
        {
            return elementBar;
        }

        return null;
    }

    private void SetLeftPanelOwner(GameObject owner)
    {
        leftPanelOwner = owner;
        leftPanel.gameObject.SetActive(true);
    }

    private void SetRightPanelOwner(GameObject owner)
    {
        rightPanelOwner = owner;
        rightPanel.gameObject.SetActive(true);
    }

    private void SetElementBarOwner(GameObject owner)
    {
        elementBarOwner = owner;
        elementBar.gameObject.SetActive(true);
    }

    public void RemovePanelOwner(GameObject owner, PanelType panelType)
    {
        if(panelType == PanelType.LEFT && leftPanelOwner == owner)
        {
            leftPanelOwner = null;
            leftPanel.gameObject.SetActive(false);
        }

        if(panelType == PanelType.RIGHT && rightPanelOwner == owner)
        {
            rightPanelOwner = null;
            rightPanel.gameObject.SetActive(false);
        }
    }

    public void RemoveElementBarOwner(GameObject owner)
    {
        if(elementBarOwner == owner)
        {
            elementBar.gameObject.SetActive(false);
            elementBarOwner = null;
        }
    }

    public void DisplayPopUpMessage(string msg)
    {
        popUpMessager.DisplayMessage(msg);
    }
}

[System.Serializable]
public enum PanelType
{
    NONE = 0,
    LEFT = 1,
    RIGHT = 2
}