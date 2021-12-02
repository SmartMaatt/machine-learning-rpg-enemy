using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpellController : SpellController
{
    private Mage entity;
    private float maxMana;
    private float manaRestoreRate;

    private EntityMagicShield currentShield;
    private EntityHealSpell currentHealSpell;

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
        if (canAttack)
        {
            StartCoroutine(AttackDesorientation(entity.minTimeAttackStartDelay, entity.maxTimeAttackStartDelay));
            canAttack = false;
            StartCoroutine(ResetAttack(entity.timeBetweenAttacks));
        }
        else
        {
            LogMessage("Can't attack right now!!!");
            entity.AddRLReward(entity.GetMageRLParameters().tryShootWhenNoMana);
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
        if (currentShield == null)
        {
            currentShield = gameObject.AddComponent<EntityMagicShield>() as EntityMagicShield;
            currentShield.SetupShield(shieldSpellNode.time, entity.maxShieldTime, shieldSpellNode, shieldParent, entity, uiPanelController);
        }
        else
        {
            ShieldSpellNode currentShieldSpellNode = currentShield.GetShieldSpellNode();
            if(currentShieldSpellNode.spell == shieldSpellNode.spell)
            {
                ChargeShieldSpell(shieldSpellNode, currentShield);
                entity.AddRLReward(entity.GetMageRLParameters().rechargeCurrentShield);
            }
            else
            {
                LogMessage(gameObject.name + " Can't use " + shieldSpellNode.name + " while using " + currentShieldSpellNode.name + "!");
                entity.AddRLReward(entity.GetMageRLParameters().rechargeWrongShield);
            }
        }
    }

    protected override void SetupHealObject(HealSpellNode healSpellNode)
    {
        if (currentHealSpell == null)
        {
            currentHealSpell = gameObject.AddComponent<EntityHealSpell>() as EntityHealSpell;
            currentHealSpell.SetupShield(healSpellNode.time, entity.maxHealTime, healSpellNode, shieldParent, entity, uiPanelController);
        }
        else
        {
            ChargeHealSpell(healSpellNode, currentHealSpell); 
        }

        if(entity.IsHealthLow())
        {
            entity.AddRLReward(entity.GetMageRLParameters().healWhenHealthUserLow);
        }
    }


    /*Spell casting*/
    protected override void SetupCastballSpellInfo(SpellInfo spellInfo, CastSpellNode spellNode)
    {
        spellInfo.castSpellNode = spellNode;
        spellInfo.SetupSpellInfoOwner(entity);
    }

    protected override void RunCastSpellAnimation(float time, Transform castSpell)
    {
        entity.GetAnimationRiggingController().ThrowCastSpell(time, castSpell);
    }



    protected override void AreaExplosionAdditionalConfiguration()
    {
        if(entity.IsHealthCriticalLow())
        {
            entity.AddRLReward(entity.GetMageRLParameters().areaSpellWhenHealthUnderCriticalLow);
        }
    }

    protected override void LoadOwnerOfExplosion(AreaExplosionBullet owner)
    {
        owner.LoadEntity(entity);
    }

    protected override void SetElementUIBarValue(float value) {}

    protected override void LogMessage(string msg)
    {
        Debug.Log(msg);
    }


    /*Getters*/
    public EntityMagicShield GetCurrentShield()
    {
        return currentShield;
    }

    public EntityHealSpell GetCurrentHealSpell()
    {
        return currentHealSpell;
    }
}