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
    private bool isPlayer = true;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        base.Start();
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isPlayer = !isPlayer;
        }

        if(isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.FIRE);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.WATER);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetSpellType(SpellType.CAST, (int)CastSpell.SNOW);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.FIRE);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.WATER);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetSpellType(SpellType.SHIELD, (int)ShieldSpell.SNOW);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetSpellType(SpellType.CUSTOM, (int)CustomSpell.HEAL);
                ExecuteSpell();
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetSpellType(SpellType.CUSTOM, (int)CustomSpell.AREA_EXPLOSION);
                ExecuteSpell();
            }
        }
        else
        {
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
}