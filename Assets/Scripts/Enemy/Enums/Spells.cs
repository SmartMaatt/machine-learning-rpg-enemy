using UnityEngine;


[System.Serializable]
public enum CastSpell
{
    FIRE = 0,
    WATER = 1,
    SNOW = 2
}

public enum ShieldSpell
{
    FIRE = 0,
    WATER = 1,
    SNOW = 2
}

public enum CustomSpell
{
    HEAL = 0
}

[System.Serializable]
public abstract class SpellNode
{
    public bool isAvaliable;
    public float cost;
    public GameObject prefab;
}

[System.Serializable]
public class CastSpellNode : SpellNode
{
    public CastSpell spell;
}

[System.Serializable]
public class ShieldSpellNode : SpellNode
{
    public ShieldSpell spell;
}

[System.Serializable]
public class CustomSpellNode : SpellNode
{
    public CustomSpell spell;
}