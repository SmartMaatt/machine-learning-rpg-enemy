using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellController : MonoBehaviour
{
    [Header("Available spells")]
    [SerializeField] private CastSpellNode[] avaliableCastSpells;
    [SerializeField] private ShieldSpellNode[] avaliableShieldSpells;
    [SerializeField] private CustomSpellNode[] avaliableCustomSpells;

    [Header("Correction vectors")]
    public Vector3 chestPositionCorrection;
    public float distanceFromInvoke;

    [Header("References")]
    [SerializeField] private Transform orientation;

    private Dictionary<CastSpell, CastSpellNode> avaliableCastSpellsDict;
    private Dictionary<ShieldSpell, ShieldSpellNode> avaliableShieldSpellsDict;
    private Dictionary<CustomSpell, CustomSpellNode> avaliableCustomSpellsDict;

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
    public void CastFireball()
    {
        try
        {
            CastSpellNode fireSpell = avaliableCastSpellsDict[CastSpell.FIRE];
            Vector3 checkPosition = transform.position + chestPositionCorrection;

            if (fireSpell.isAvaliable)
            {
                if (UseMana(fireSpell.cost))
                {
                    GameObject fireball = Instantiate(fireSpell.prefab, checkPosition + orientation.forward * distanceFromInvoke, Quaternion.identity);
                    EffectSettings fireballSettings = fireball.GetComponent<EffectSettings>();
                    fireballSettings.UseMoveVector = true;
                    fireballSettings.MoveVector = orientation.forward;
                    fireballSettings.damage = fireSpell.damage;
                    Debug.Log(gameObject.name + " casted Fireball!");
                }
                else
                {
                    Debug.Log(gameObject.name + " can't spell Fireball! Cost: " + fireSpell.cost + ", Mana: " + GetMana());
                }
            }
            else
            {
                Debug.Log("Spell fireball is not avaliable right now!");
            }
        }
        catch (KeyNotFoundException err)
        {
            Debug.LogError("Fireball hasn't been declared in inspector!");
            Debug.LogError(err);
        }
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
                    switch((CastSpell)spellID)
                    {
                        case CastSpell.FIRE:
                            CastFireball();
                            break;
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
