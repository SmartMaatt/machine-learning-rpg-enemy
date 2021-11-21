using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EnemySpellController : SpellController
{
    private Mage entity;
    private float maxMana;
    private float manaRestoreRate;

    private void Start()
    {
        entity = GetComponent<Mage>();
        maxMana = entity.maxMana;
        manaRestoreRate = entity.manaRestoreRate;

        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    protected override float GetMaxMana()
    {
        return entity.maxMana;
    }

    protected override float GetManaRestoreRate()
    {
        return entity.manaRestoreRate;
    }

    public override void ExecuteSpell()
    {
        if (!alreadyAttacked)
        {
            StartCoroutine(AttackDesorientation(entity.minTimeAttackStartDelay, entity.maxTimeAttackStartDelay));
            alreadyAttacked = true;
            StartCoroutine(ResetAttack(entity.timeBetweenAttacks));
        }
        else
        {
            Debug.LogError("Can't attack right now!!!");
        }
    }

    private IEnumerator AttackDesorientation(float minTimeDelay, float maxTimeDelay)
    {
        float DesorientationTime = UnityEngine.Random.Range(minTimeDelay, maxTimeDelay);
        yield return new WaitForSeconds(DesorientationTime);
        try
        {
            UseSpellType();
        }
        catch(UnsetSpellException err)
        {
            Debug.LogError(err);
        }
    }

    protected override void SetupShieldObject(ShieldSpellNode shieldSpellNode)
    {
        MagicShield currentShield = GetComponent<MagicShield>();
        if (currentShield == null)
        {
            EntityMagicShield magicShieldScript = gameObject.AddComponent<EntityMagicShield>() as EntityMagicShield;
            magicShieldScript.SetupShield(shieldSpellNode.time, entity.maxShieldTime, shieldSpellNode, shieldParent, entity);
        }
        else
        {
            ChargeShieldSpell(shieldSpellNode, currentShield);
        }
    }
}