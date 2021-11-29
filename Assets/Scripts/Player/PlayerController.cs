using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [Header("Spell states")]
    [SerializeField] private bool blocking;
    [SerializeField] private bool healing;

    [Header("UI configuration")]
    [SerializeField] private PanelType uiPanelType;
    [SerializeField] private string playerName;

    private PlayerMovement playerMovement;
    private PlayerSpellController playerSpellController;
    private PanelControll uiPanel;

    private void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        playerSpellController = GetComponent<PlayerSpellController>();

        uiPanel = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanel.SetupHealth(maxHealth, health);
        uiPanel.SetupName(playerName);
    }

    private void Update()
    {
        if(health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("I've never died before!");
        Managers.UI.RemovePanelOwner(this.gameObject, uiPanelType);
    }


    /*Getters*/
    public float GetPlayerHealth()
    {
        return health;
    }

    public float GetPlayerMaxHealth()
    {
        return maxHealth;
    }

    public bool GetBlocking()
    {
        return blocking;
    }

    public bool GetHealing()
    {
        return healing;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public PlayerMovement GetPlayerMovement()
    {
        return playerMovement;
    }
    
    public PanelType GetPanelType()
    {
        return uiPanelType;
    }

    public PanelControll GetPanelControll()
    {
        return uiPanel;
    }


    /*Setters*/
    public void ChangeHealth(float hurt)
    {
        health += hurt;
        health = Mathf.Clamp(health, 0f, maxHealth);
        uiPanel.ChangeHealth(health);
    }

    public void SetBlocking(bool blocking)
    {
        this.blocking = blocking;
    }

    public void SetHealing(bool healing)
    {
        this.healing = healing;
    }
}
