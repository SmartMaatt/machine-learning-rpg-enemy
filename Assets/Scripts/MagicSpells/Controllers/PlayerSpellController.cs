using UnityEngine;

class PlayerSpellController : SpellController
{
    [Header("Player Mana")]
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    [SerializeField] private float timeBetweenAttacks;

    [Header("Player Shield")]
    [SerializeField] private float maxShieldTime;

    [Header("Player references")]
    [SerializeField] private Mage tmpEntity;

    private PlayerHealth playerHealth;
    private bool isPlayer = true;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
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
            magicShieldScript.SetupShield(shieldSpellNode.time, maxShieldTime, shieldSpellNode, shieldParent, playerHealth);
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
}