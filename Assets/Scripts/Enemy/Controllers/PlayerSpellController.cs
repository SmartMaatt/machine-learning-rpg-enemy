using UnityEngine;

class PlayerSpellController : SpellController
{
    [Header("Player Mana")]
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    [SerializeField] private float timeBetweenAttacks;

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
}