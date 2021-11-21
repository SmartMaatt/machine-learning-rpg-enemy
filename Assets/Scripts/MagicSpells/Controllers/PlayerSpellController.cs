using UnityEngine;

class PlayerSpellController : SpellController
{
    [Header("Player Mana")]
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    [SerializeField] private float timeBetweenAttacks;

    [SerializeField] private Mage tmpEntity;

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSpellType(SpellType.CAST, (int)CastSpell.FIRE);
            ExecuteSpell();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSpellType(SpellType.CAST, (int)CastSpell.WATER);
            ExecuteSpell();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSpellType(SpellType.CAST, (int)CastSpell.SNOW);
            ExecuteSpell();
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            tmpEntity.SetSpellType(SpellType.SHIELD, (int)ShieldSpell.FIRE);
            tmpEntity.Attack();
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

    protected override void SetupShieldObject(ShieldSpellNode shieldSpell)
    {
        throw new System.NotImplementedException();
    }
}