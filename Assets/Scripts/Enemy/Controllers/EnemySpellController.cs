using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EnemySpellController : SpellController
{
    private Mage _entity;
    private float maxMana;
    private float manaRestoreRate;

    private void Start()
    {
        _entity = GetComponent<Mage>();
        maxMana = _entity.maxMana;
        manaRestoreRate = _entity.manaRestoreRate;

        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    protected override float GetMaxMana()
    {
        return _entity.maxMana;
    }

    protected override float GetManaRestoreRate()
    {
        return _entity.manaRestoreRate;
    }

    public override void ExecuteSpell()
    {
        if (!alreadyAttacked)
        {
            StartCoroutine(AttackDesorientation(_entity.minTimeAttackStartDelay, _entity.maxTimeAttackStartDelay));
            alreadyAttacked = true;
            StartCoroutine(ResetAttack(_entity.timeBetweenAttacks));
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
}