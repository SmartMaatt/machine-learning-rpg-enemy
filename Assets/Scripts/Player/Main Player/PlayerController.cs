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

    [Header("Other")]
    [SerializeField] private float headPosition;

    [Header("References")]
    [SerializeField] private GameObject enemy;

    private PlayerMovement playerMovement;
    private PlayerSpellController playerSpellController;
    private PanelControll uiPanel;

    private void Start()
    {
        health = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        playerSpellController = GetComponent<PlayerSpellController>();

        Managers.Level.ValidateLevelEntities(gameObject);

        uiPanel = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanel.SetupHealth(maxHealth, health);
        uiPanel.SetupName(playerName);
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Managers.Level.EndEpisode(gameObject);
    }


    /*>>> Getters <<<*/
    public float GetPlayerHealth()
    {
        return health;
    }

    public float GetPlayerMaxHealth()
    {
        return maxHealth;
    }

    public float GetHeadPosition()
    {
        return headPosition;
    }

    public bool IsBlocking()
    {
        return blocking;
    }

    public bool IsHealing()
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

    public PlayerSpellController GetSpellController()
    {
        return playerSpellController;
    }

    public PanelType GetPanelType()
    {
        return uiPanelType;
    }

    public PanelControll GetUIPanelControll()
    {
        return uiPanel;
    }


    /*>>> Setters <<<*/
    public void ChangeHealth(float hurt)
    {
        health += hurt;
        health = Mathf.Clamp(health, 0f, maxHealth);
        uiPanel.ChangeHealth(health);
    }

    public void InstantKill()
    {
        ChangeHealth(-maxHealth);
    }

    public void ReloadHealth()
    {
        health = maxHealth;
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
