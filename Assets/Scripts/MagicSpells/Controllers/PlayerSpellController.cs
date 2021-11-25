using UnityEngine;

class PlayerSpellController : SpellController
{
    [Header("Player Mana")]
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    [SerializeField] private float timeBetweenAttacks;

    [Header("Player timings")]
    [SerializeField] private float maxShieldTime;
    [SerializeField] private float maxHealTime;

    [Header("Player references")]
    [SerializeField] private Mage tmpEntity;

    private PlayerController playerController;

    private CastSpell currentSpell;
    private ElementBar elementBar;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        uiPanelType = playerController.uiPanelType;
        uiPanelController = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanelController.SetupMana(maxMana, maxMana);

        elementBar = Managers.UI.SetupElementBar(this.gameObject);
        elementBar.SetupBarValues(1f, 0f, currentSpell);

        base.Start();
    }

    private void Update()
    {
        base.Update();

        ListenToScrollInput();

        if(currentSpell == CastSpell.FIRE)
        {
            if(Input.GetMouseButtonDown(0))
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
        

        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tmpEntity.SetSpellType(SpellType.CAST, (int)CastSpell.FIRE);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tmpEntity.SetSpellType(SpellType.CAST, (int)CastSpell.WATER);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tmpEntity.SetSpellType(SpellType.CAST, (int)CastSpell.SNOW);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tmpEntity.SetSpellType(SpellType.SHIELD, (int)ShieldSpell.FIRE);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            tmpEntity.SetSpellType(SpellType.SHIELD, (int)ShieldSpell.WATER);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            tmpEntity.SetSpellType(SpellType.SHIELD, (int)ShieldSpell.SNOW);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            tmpEntity.SetSpellType(SpellType.CUSTOM, (int)CustomSpell.HEAL);
            tmpEntity.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            tmpEntity.SetSpellType(SpellType.CUSTOM, (int)CustomSpell.AREA_EXPLOSION);
            tmpEntity.Attack();
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

    protected override float GetMaxMana()
    {
        return maxMana;
    }

    protected override float GetManaRestoreRate()
    {
        return manaRestoreRate;
    }

    public override void ExecuteSpell()
    {
        if (!alreadyAttacked)
        {
            UseSpellType();
            alreadyAttacked = true;
            StartCoroutine(ResetAttack(timeBetweenAttacks));
        }
        else
        {
            Debug.LogError("Can't attack right now!!!");
        }
    }

    protected override void SetupShieldObject(ShieldSpellNode shieldSpellNode)
    {
        MagicShield currentShield = GetComponent<MagicShield>();
        if (currentShield == null)
        {
            PlayerMagicShield magicShieldScript = gameObject.AddComponent<PlayerMagicShield>() as PlayerMagicShield;
            magicShieldScript.SetupShield(shieldSpellNode.time, maxShieldTime, shieldSpellNode, shieldParent, playerController);
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
                Debug.LogError("Can't use " + shieldSpellNode.name + " while using " + currentShieldSpellNode.name + "!");
            }
        }
    }

    protected override void SetupHealObject(HealSpellNode healSpellNode)
    {
        HealSpell currentHealComponent = GetComponent<HealSpell>();
        if (currentHealComponent == null)
        {
            PlayerHealSpell healSpellScript = gameObject.AddComponent<PlayerHealSpell>() as PlayerHealSpell;
            healSpellScript.SetupShield(healSpellNode.time, maxHealTime, healSpellNode, shieldParent, playerController);
        }
        else
        {
            ChargeHealSpell(healSpellNode, currentHealComponent);
        }
    }

    protected override void RunCastSpellAnimation(float time, Transform castSpell)
    {
    }

    protected override void RunAreaExplosionAnimation()
    {
    }

    protected override void LoadOwnerOfExplosion(AreaExplosionBullet owner)
    {
        owner.LoadPlayer(playerController);
    }

    protected override void SetElementUIBarValue(float value)
    {
        elementBar.ChangeValue(value);
    }
}