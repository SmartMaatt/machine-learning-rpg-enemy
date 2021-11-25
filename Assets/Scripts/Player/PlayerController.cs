using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public float maxHealth;

    [Header("UI configuration")]
    public PanelType uiPanelType;
    public string playerName;

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

    public void ChangeHealth(float hurt)
    {
        health += hurt;
        health = Mathf.Clamp(health, 0f, maxHealth);
        uiPanel.ChangeHealth(health);
    }

    private void Die()
    {
        Debug.Log("I've never died before!");
        Managers.UI.RemovePanelOwner(this.gameObject, uiPanelType);
    }

    public PlayerMovement GetPlayerMovement()
    {
        return playerMovement;
    }

    public PanelControll GetPanelControll()
    {
        return uiPanel;
    }
}
