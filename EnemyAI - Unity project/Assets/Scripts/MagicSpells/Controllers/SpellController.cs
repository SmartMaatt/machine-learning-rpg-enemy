using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellController : MonoBehaviour
{
    [Header("Available spells")]
    [SerializeField] protected CastSpellNode[] avaliableCastSpells;
    [SerializeField] protected ShieldSpellNode[] avaliableShieldSpells;
    [SerializeField] protected HealSpellNode healSpellNode;
    [SerializeField] protected AreaExplosionNode areaExplosionSpellNode;

    [Header("Correction vectors")]
    public Vector3 chestPositionCorrection;
    public float distanceFromInvoke;

    [Header("References")]
    [SerializeField] protected Transform orientation;
    [SerializeField] protected GameObject shieldParent;

    protected Dictionary<CastSpell, CastSpellNode> avaliableCastSpellsDict;
    protected Dictionary<ShieldSpell, ShieldSpellNode> avaliableShieldSpellsDict;

    protected bool canAttack;
    protected float mana;

    protected bool spellTypeSet;
    protected SpellType spellType;
    protected int spellID;

    protected int lastHittedSpellID;

    protected PanelType uiPanelType;
    protected PanelControll uiPanelController;


    /*>>> Unity methods <<<*/
    protected virtual void Start()
    {
        ConvertSpellsToDicts();
        SetMana(GetMaxMana());

        canAttack = true;
        spellTypeSet = false;

        lastHittedSpellID = -1;
    }

    protected virtual void Update()
    {
        ChargeMana();
    }


    /*>>> Spell casting <<<*/
    public void CastBallSpell(CastSpell castSpell)
    {
        try
        {
            CastSpellNode spellNode = avaliableCastSpellsDict[castSpell];
            Vector3 checkPosition = transform.position + chestPositionCorrection;

            if (spellNode.isAvaliable)
            {
                if (UseMana(spellNode.cost))
                {
                    GameObject castball = Instantiate(spellNode.prefab, checkPosition + orientation.forward * distanceFromInvoke, Quaternion.identity);
                    EffectSettings fireballSettings = castball.GetComponent<EffectSettings>();
                    SpellInfo ballSpellInfo = castball.GetComponent<SpellInfo>();

                    fireballSettings.UseMoveVector = true;
                    fireballSettings.MoveVector = orientation.forward;

                    SetupCastballSpellInfo(fireballSettings, ballSpellInfo, spellNode);
                    RunCastSpellAnimation(0.4f, castball.transform);
                }
                else
                {
                    LogMessage(gameObject.name + " nie może użyć zaklęcia " + spellNode.name + "! Koszt: " + spellNode.cost + ", Mana: " + GetMana());
                    NoManaRLReward();
                }
            }
            else
            {
                LogMessage("Zaklęcie " + spellNode.name + " nie jest aktualnie dostępne!");
                NoSpellRlReward();
            }
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

    protected abstract void SetupCastballSpellInfo(EffectSettings effectSettings, SpellInfo spellInfo, CastSpellNode spellNode);
    protected abstract void RunCastSpellAnimation(float time, Transform castSpell);



    /*>>> Shield casting <<<*/
    public void CastShieldSpell(ShieldSpell shieldSpell)
    {
        try
        {
            ShieldSpellNode spellNode = avaliableShieldSpellsDict[shieldSpell];

            if (spellNode.isAvaliable)
            {
                if (UseMana(spellNode.cost))
                {
                    SetupShieldObject(spellNode);
                }
                else
                {
                    LogMessage(gameObject.name + " nie może użyć zaklęcia " + spellNode.name + "! Koszt: " + spellNode.cost + ", Mana: " + GetMana());
                    NoManaRLReward();
                }
            }
            else
            {
                LogMessage("Zaklęcie " + spellNode.name + " nie jest aktualnie dostępne!");
                NoSpellRlReward();
            }
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

    protected void ChargeShieldSpell(ShieldSpellNode shieldSpell, MagicShield shieldScript)
    {
        Debug.Log("Shield charge!");
        shieldScript.ChangeArmour(shieldSpell.armour);
        shieldScript.ChangeTime(shieldSpell.time);
    }

    protected abstract void SetupShieldObject(ShieldSpellNode shieldSpellNode);


    /*>>> Heal casting <<<*/
    public void CastHealSpell()
    {
        try
        {
            if (healSpellNode.isAvaliable)
            {
                if (UseMana(healSpellNode.cost))
                {
                    SetupHealObject(healSpellNode);
                }
                else
                {
                    LogMessage(gameObject.name + " nie może użyć zaklęcia " + healSpellNode.name + "! Koszt: " + healSpellNode.cost + ", Mana: " + GetMana());
                    NoManaRLReward();
                }
            }
            else
            {
                LogMessage("Zaklęcie " + healSpellNode.name + " nie jest aktualnie dostępne!");
                NoSpellRlReward();
            }
        }
        catch (NullReferenceException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

    protected void ChargeHealSpell(HealSpellNode healSpell, HealSpell healScript)
    {
        healScript.ChangeTime(healSpell.time);
    }

    protected abstract void SetupHealObject(HealSpellNode healSpellNode);



    /*>>> Area spell casting <<<*/
    public void CastAreaExplosionSpell()
    {
        try
        {
            if (areaExplosionSpellNode.isAvaliable)
            {
                if (UseMana(areaExplosionSpellNode.cost))
                {
                    GameObject castSpell = Instantiate(areaExplosionSpellNode.prefab, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);
                    EffectSettings fireballSettings = castSpell.GetComponent<EffectSettings>();
                    AreaExplosionBullet areaExplosionBullet = castSpell.GetComponent<AreaExplosionBullet>();
                    SpellInfo ballSpellInfo = castSpell.GetComponent<SpellInfo>();

                    fireballSettings.UseMoveVector = true;
                    fireballSettings.MoveVector = new Vector3(0f, -1f, 0f);

                    areaExplosionBullet.LoadParams(transform.position, areaExplosionSpellNode);
                    LoadOwnerOfExplosion(areaExplosionBullet);
                    areaExplosionBullet.ExecuteExplosion();

                    ballSpellInfo.areaExplosionNode = areaExplosionSpellNode;
                    AreaExplosionAdditionalConfiguration();
                }
                else
                {
                    LogMessage(gameObject.name + " nie może użyć zaklęcia " + areaExplosionSpellNode.name + "! Koszt: " + areaExplosionSpellNode.cost + ", Mana: " + GetMana());
                    NoManaRLReward();
                }
            }
            else
            {

                LogMessage("Zaklęcie " + areaExplosionSpellNode.name + " nie jest aktualnie dostępne!");
                NoSpellRlReward();
            }
        }
        catch (NullReferenceException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

    protected abstract void LoadOwnerOfExplosion(AreaExplosionBullet areaExplosionBullet);
    protected abstract void AreaExplosionAdditionalConfiguration();


    /*>>> Spell validation and convertion <<<*/
    private void ConvertSpellsToDicts()
    {
        avaliableCastSpellsDict = new Dictionary<CastSpell, CastSpellNode>();
        foreach (CastSpellNode node in avaliableCastSpells)
        {
            avaliableCastSpellsDict.Add(node.spell, node);
        }

        avaliableShieldSpellsDict = new Dictionary<ShieldSpell, ShieldSpellNode>();
        foreach (ShieldSpellNode node in avaliableShieldSpells)
        {
            avaliableShieldSpellsDict.Add(node.spell, node);
        }
    }

    public bool CheckSpellType(SpellType spellType, int spellID)
    {
        switch (spellType)
        {
            case SpellType.CAST:
                if (!Enum.IsDefined(typeof(CastSpell), spellID))
                    return false;
                break;

            case SpellType.SHIELD:
                if (!Enum.IsDefined(typeof(ShieldSpell), spellID))
                    return false;
                break;

            case SpellType.CUSTOM:
                if (!Enum.IsDefined(typeof(CustomSpell), spellID))
                    return false;
                break;

            default:
                return false;
        }
        return true;
    }

    public void SetSpellType(SpellType spellType, int spellID)
    {
        if (CheckSpellType(spellType, spellID))
        {
            this.spellType = spellType;
            this.spellID = spellID;
            this.spellTypeSet = true;
        }
        else
        {
            throw new UnknownSpellException("Spell " + spellType + " doesn't have id " + spellID + "!");
        }
    }

    public void ResetSpellType()
    {
        spellType = SpellType.NONE;
        spellID = -1;
        spellTypeSet = false;
    }

    public void UseSpellType()
    {
        if (spellTypeSet)
        {
            switch (spellType)
            {
                case SpellType.CAST:
                    CastBallSpell((CastSpell)spellID);
                    break;

                case SpellType.SHIELD:
                    CastShieldSpell((ShieldSpell)spellID);
                    break;

                case SpellType.CUSTOM:
                    if ((CustomSpell)spellID == CustomSpell.HEAL)
                    {
                        CastHealSpell();
                    }
                    else if ((CustomSpell)spellID == CustomSpell.AREA_EXPLOSION)
                    {
                        CastAreaExplosionSpell();
                    }
                    break;
            }
            ResetSpellType();
        }
        else
        {
            throw new UnsetSpellException("Didn't set current spell! Use SetSpellType before executing magic!");
        }
    }


    /*>>> Getters <<<*/
    public bool GetCanAttack()
    {
        return canAttack;
    }

    public int GetOneHotCanAttack()
    {
        return canAttack ? 1 : 0;
    }

    public int GetLastHittedSpellID()
    {
        return lastHittedSpellID;
    }


    /*>>> Setters <<<*/
    public void SetLastHittedSpellID(int spellID)
    {
        lastHittedSpellID = (int)spellID;
    }

    public void ResetLastHittedSpellID()
    {
        lastHittedSpellID = -1;
    }


    /*>>> Mana mehtods <<<*/
    public float GetMana()
    {
        return mana;
    }

    public float GetNormalizedMana()
    {
        return (mana / GetMaxMana());
    }

    public bool SetMana(float newMana)
    {
        if (newMana > 0 && newMana <= GetMaxMana())
        {
            mana = newMana;
            uiPanelController.ChangeMana(mana);
            return true;
        }
        return false;
    }

    public bool AddMana(float additionalMana)
    {
        if (additionalMana > 0)
        {
            mana += additionalMana;
            if (mana > GetMaxMana())
            {
                mana = GetMaxMana();
            }
            uiPanelController.ChangeMana(mana);
            return true;
        }
        return false;
    }

    public bool UseMana(float manaToUse)
    {
        if ((manaToUse > 0) && (manaToUse < mana))
        {
            mana -= manaToUse;
            uiPanelController.ChangeMana(mana);
            return true;
        }
        return false;
    }

    public void ChargeMana()
    {
        AddMana(Time.deltaTime * GetManaRestoreRate());
    }

    public void RefilMana()
    {
        mana = GetMaxMana();
        uiPanelController.ChangeMana(mana);
    }

    protected IEnumerator ResetAttack(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / time;
            SetElementUIBarValue(elapsedTime);
            yield return new WaitForEndOfFrame();
        }
        canAttack = true;
    }


    /*>>> Abstract methods <<<*/
    protected abstract float GetMaxMana();
    protected abstract float GetManaRestoreRate();
    public abstract void ExecuteSpell();
    protected abstract void SetElementUIBarValue(float value);
    protected abstract void LogMessage(string msg);
    protected abstract void NoManaRLReward();
    protected abstract void NoSpellRlReward();
}
