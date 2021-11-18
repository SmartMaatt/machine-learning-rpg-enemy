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

    private Dictionary<CastSpell, CastSpellNode> avaliableCastSpellsDict;
    private Dictionary<ShieldSpell, ShieldSpellNode> avaliableShieldSpellsDict;
    private Dictionary<CustomSpell, CustomSpellNode> avaliableCustomSpellsDict;

    protected virtual void Start()
    {
        ConvertSpellsToDicts();
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
            if (fireSpell.isAvaliable)
            {
                Debug.Log(gameObject.name + " casted Fireball!: " + fireSpell.cost);
            }
        }
        catch(KeyNotFoundException err)
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


    /*Spell getters*/
    public bool GetCastSpell(CastSpell spell)
    {
        foreach (CastSpellNode node in avaliableCastSpells)
        {
            if (node.spell == spell)
            {
                return node.isAvaliable;
            }
        }
        return false;
    }

    public bool GetShieldSpell(ShieldSpell spell)
    {
        foreach (ShieldSpellNode node in avaliableShieldSpells)
        {
            if (node.spell == spell)
            {
                return node.isAvaliable;
            }
        }
        return false;
    }

    public bool GetCustomSpell(CustomSpell spell)
    {
        foreach (CustomSpellNode node in avaliableCustomSpells)
        {
            if (node.spell == spell)
            {
                return node.isAvaliable;
            }
        }
        return false;
    }

    protected abstract void ChargeMana();
}
