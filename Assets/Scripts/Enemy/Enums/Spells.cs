using UnityEngine;

[System.Serializable]
public enum SpellType
{
    NONE = -1,
    CAST = 0,
    SHIELD = 1,
    CUSTOM = 2
}

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
    public string name;
}

[System.Serializable]
public class CastSpellNode : SpellNode
{
    public float damage;
    public int armourDamage;
    public float pushForce;
    public CastSpell spell;
}

[System.Serializable]
public class ShieldSpellNode : SpellNode
{
    public float time;
    public int armour;
    public ShieldSpell spell;
}

[System.Serializable]
public class HealSpellNode : SpellNode
{
    public float healPointsPerSecond;
    public float time;
    public CustomSpell spell;
}