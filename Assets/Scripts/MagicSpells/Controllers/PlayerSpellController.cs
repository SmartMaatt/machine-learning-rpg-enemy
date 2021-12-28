using UnityEngine;

public class PlayerSpellController : SpellController
{
    [Header("Player Mana")]
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    [SerializeField] private float timeBetweenAttacks;

    [Header("Player timings")]
    [SerializeField] private float maxShieldTime;
    [SerializeField] private float maxHealTime;

    private PlayerController playerController;

    private CastSpell currentSpell;
    private ElementBar elementBar;

    private PlayerMagicShield currentShield;
    private PlayerHealSpell currentHealSpell;


    /*>>> Unity methods <<<*/
    protected override void Start()
    {
        playerController = GetComponent<PlayerController>();

        uiPanelType = playerController.GetPanelType();
        uiPanelController = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanelController.SetupMana(maxMana, maxMana);

        elementBar = Managers.UI.SetupElementBar(this.gameObject);
        elementBar.SetupBarValues(1f, 0f, currentSpell);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        ListenToScrollInput();

        if (currentSpell == CastSpell.FIRE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.FIRE);
                ExecuteSpell();
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.FIRE);
                ExecuteSpell();
            }
        }

        if (currentSpell == CastSpell.WATER)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.WATER);
                ExecuteSpell();
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.WATER);
                ExecuteSpell();
            }
        }

        if (currentSpell == CastSpell.SNOW)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.SNOW);
                ExecuteSpell();
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.SNOW);
                ExecuteSpell();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetSpellType(SpellType.CUSTOM, (int)CustomSpell.HEAL);
            ExecuteSpell();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SetSpellType(SpellType.CUSTOM, (int)CustomSpell.AREA_EXPLOSION);
            ExecuteSpell();
        }
    }

    private void ListenToScrollInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentSpell++;
            if ((int)currentSpell == 3)
            {
                currentSpell = CastSpell.FIRE;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentSpell--;
            if ((int)currentSpell == -1)
            {
                currentSpell = CastSpell.SNOW;
            }
        }
        elementBar.ChangeElement(currentSpell);
    }


    /*>>> Abstract methods <<<*/
    public override void ExecuteSpell()
    {
        if (canAttack)
        {
            UseSpellType();
            canAttack = false;
            StartCoroutine(ResetAttack(timeBetweenAttacks));
        }
        else
        {
            LogMessage("Can't attack right now!!!");
        }
    }

    protected override void SetElementUIBarValue(float value)
    {
        elementBar.ChangeValue(value);
    }

    protected override void LogMessage(string msg)
    {
        Managers.UI.DisplayPopUpMessageWithTime(msg, 4f);
        Debug.Log(msg);
    }


    /*>>> Spell casting <<<*/
    protected override void SetupCastballSpellInfo(SpellInfo spellInfo, CastSpellNode spellNode)
    {
        spellInfo.castSpellNode = spellNode;
        spellInfo.SetupSpellInfoOwner(null);
    }
    protected override void RunCastSpellAnimation(float time, Transform castSpell) { }
    protected override void NoManaRLReward() { }
    protected override void NoSpellRlReward() { }


    /*>>> Shield casting <<<*/
    protected override void SetupShieldObject(ShieldSpellNode shieldSpellNode)
    {
        if (currentShield == null)
        {
            currentShield = gameObject.AddComponent<PlayerMagicShield>() as PlayerMagicShield;
            currentShield.SetupShield(shieldSpellNode.time, maxShieldTime, shieldSpellNode, shieldParent, playerController, uiPanelController);
        }
        else
        {
            ShieldSpellNode currentShieldSpellNode = currentShield.GetShieldSpellNode();
            if (currentShieldSpellNode.spell == shieldSpellNode.spell)
            {
                ChargeShieldSpell(shieldSpellNode, currentShield);
            }
            else
            {
                LogMessage(gameObject.name + " can't use " + shieldSpellNode.name + " while using " + currentShieldSpellNode.name + "!");
            }
        }
    }


    /*>>> Heal casting <<<*/
    protected override void SetupHealObject(HealSpellNode healSpellNode)
    {
        if (currentHealSpell == null)
        {
            currentHealSpell = gameObject.AddComponent<PlayerHealSpell>() as PlayerHealSpell;
            currentHealSpell.SetupShield(healSpellNode.time, maxHealTime, healSpellNode, shieldParent, playerController, uiPanelController);
        }
        else
        {
            ChargeHealSpell(healSpellNode, currentHealSpell);
        }
    }


    /*>>> Area spell casting <<<*/
    protected override void LoadOwnerOfExplosion(AreaExplosionBullet owner)
    {
        owner.LoadPlayer(playerController);
    }

    protected override void AreaExplosionAdditionalConfiguration() { }


    /*>>> Getters <<<*/
    protected override float GetMaxMana()
    {
        return maxMana;
    }

    protected override float GetManaRestoreRate()
    {
        return manaRestoreRate;
    }

    public PlayerMagicShield GetCurrentShield()
    {
        return currentShield;
    }

    public PlayerHealSpell GetCurrentHealSpell()
    {
        return currentHealSpell;
    }
}