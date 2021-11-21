using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellController : MonoBehaviour
{
    [Header("Available spells")]
    [SerializeField] protected CastSpellNode[] avaliableCastSpells;
    [SerializeField] protected ShieldSpellNode[] avaliableShieldSpells;
    [SerializeField] protected CustomSpellNode[] avaliableCustomSpells;

    [Header("Correction vectors")]
    public Vector3 chestPositionCorrection;
    public float distanceFromInvoke;

    [Header("References")]
    [SerializeField] protected Transform orientation;
    [SerializeField] protected GameObject shieldParent;

    protected Dictionary<CastSpell, CastSpellNode> avaliableCastSpellsDict;
    protected Dictionary<ShieldSpell, ShieldSpellNode> avaliableShieldSpellsDict;
    protected Dictionary<CustomSpell, CustomSpellNode> avaliableCustomSpellsDict;

    protected bool alreadyAttacked;
    protected float mana;

    protected bool spellTypeSet;
    protected SpellType spellType;
    protected int spellID;

    protected virtual void Start()
    {
        ConvertSpellsToDicts();
        SetMana(GetMaxMana());

        alreadyAttacked = false;
        spellTypeSet = false;
    }

    protected virtual void Update()
    {
        ChargeMana();
    }

    /*Spell casting*/
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
                    ballSpellInfo.castSpellNode = spellNode;

                    Debug.Log(gameObject.name + " casted " + spellNode.name + "!");
                }
                else
                {
                    Debug.Log(gameObject.name + " can't spell " + spellNode.name + "! Cost: " + spellNode.cost + ", Mana: " + GetMana());
                }
            }
            else
            {
                Debug.Log("Spell " + spellNode.name + " is not avaliable right now!");
            }
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

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
                    Debug.Log(gameObject.name + " casted " + spellNode.name + "!");
                }
                else
                {
                    Debug.Log(gameObject.name + " can't spell " + spellNode.name + "! Cost: " + spellNode.cost + ", Mana: " + GetMana());
                }
            }
            else
            {
                Debug.Log("Spell " + spellNode.name + " is not avaliable right now!");
            }
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogError("Spell hasn't been declared in inspector!");
            Debug.LogError(err);
        }
    }

    protected abstract void SetupShieldObject(ShieldSpellNode shieldSpell);

    protected void ChargeShieldSpell(ShieldSpellNode shieldSpell, MagicShield shieldScript)
    {
        Debug.Log("Shield charge!");
        shieldScript.ChangeArmour(shieldSpell.armour);
        shieldScript.ChangeTime(shieldSpell.time);
    }

    private void ConvertSpellsToDicts()
    {
        avaliableCastSpellsDict = new Dictionary<CastSpell, CastSpellNode>();
        foreach(CastSpellNode node in avaliableCastSpells)
        {
            avaliableCastSpellsDict.Add(node.spell, node);
        }

        avaliableShieldSpellsDict = new Dictionary<ShieldSpell, ShieldSpellNode>();
        foreach (ShieldSpellNode node in avaliableShieldSpells)
        {
            avaliableShieldSpellsDict.Add(node.spell, node);
        }

        avaliableCustomSpellsDict = new Dictionary<CustomSpell, CustomSpellNode>();
        foreach (CustomSpellNode node in avaliableCustomSpells)
        {
            avaliableCustomSpellsDict.Add(node.spell, node);
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
        if(spellTypeSet)
        {
            switch(spellType)
            {
                case SpellType.CAST:
                    CastBallSpell((CastSpell)spellID);
                    break;

                case SpellType.SHIELD:
                    CastShieldSpell((ShieldSpell)spellID);
                    break;
            }
            ResetSpellType();
        }
        else
        {
            throw new UnsetSpellException("Didn't set current spell! Use SetSpellType before executing magic!");
        }
    }


    /*Mana mehtods*/
    public float GetMana()
    {
        return mana;
    }

    public bool SetMana(float newMana)
    {
        if (newMana > 0 && newMana <= GetMaxMana())
        {
            mana = newMana;
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
            return true;
        }
        return false;
    }

    public bool UseMana(float manaToUse)
    {
        if ((manaToUse > 0) && (manaToUse < mana))
        {
            mana -= manaToUse;
            return true;
        }
        return false;
    }

    public void ChargeMana()
    {
        AddMana(Time.deltaTime * GetManaRestoreRate());
    }

    protected IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        alreadyAttacked = false;
    }

    //private IEnumerator AttackDesorientation(float minTimeAttackStartDelay, float maxTimeAttackStartDelay)
    //{
    //    float DesorientationTime = UnityEngine.Random.Range(minTimeAttackStartDelay, maxTimeAttackStartDelay);
    //    yield return new WaitForSeconds(DesorientationTime);
    //    AttackDecision(spellType, spellID);
    //}

    /*Abstract*/
    protected abstract float GetMaxMana();
    protected abstract float GetManaRestoreRate();
    public abstract void ExecuteSpell();
}
