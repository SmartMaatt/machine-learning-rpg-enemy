using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EntitySpellController : SpellController
{
    private Mage entity;
    private float maxMana;
    private float manaRestoreRate;

    private void Start()
    {
        entity = GetComponent<Mage>();
        maxMana = entity.maxMana;
        manaRestoreRate = entity.manaRestoreRate;

        uiPanelType = entity.GetUIPanelType();
        uiPanelController = Managers.UI.SetupUIPanelController(this.gameObject, uiPanelType);
        uiPanelController.SetupMana(maxMana, maxMana);

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
        MagicShield currentShieldComponent = GetComponent<MagicShield>();
        if (currentShieldComponent == null)
        {
            EntityMagicShield magicShieldScript = gameObject.AddComponent<EntityMagicShield>() as EntityMagicShield;
            magicShieldScript.SetupShield(shieldSpellNode.time, entity.maxShieldTime, shieldSpellNode, shieldParent, entity);
        }
        else
        {
            ShieldSpellNode currentShieldSpellNode = currentShieldComponent.GetShieldSpellNode();
            if(currentShieldSpellNode.spell == shieldSpellNode.spell)
            {
                ChargeShieldSpell(shieldSpellNode, currentShieldComponent);
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
            EntityHealSpell healSpellScript = gameObject.AddComponent<EntityHealSpell>() as EntityHealSpell;
            healSpellScript.SetupShield(healSpellNode.time, entity.maxHealTime, healSpellNode, shieldParent, entity);
        }
        else
        {
            ChargeHealSpell(healSpellNode, currentHealComponent); 
        }
    }

    protected override void RunCastSpellAnimation(float time, Transform castSpell)
    {
        entity.GetAnimationRiggingController().ThrowCastSpell(time, castSpell);
    }

    protected override void RunAreaExplosionAnimation()
    {
    }

    protected override void LoadOwnerOfExplosion(AreaExplosionBullet owner)
    {
        owner.LoadEntity(entity);
    }

    protected override void SetElementUIBarValue(float value) {}
}