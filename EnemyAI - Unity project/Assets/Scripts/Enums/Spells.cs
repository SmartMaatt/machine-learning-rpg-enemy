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
    SNOW = 2,
    NUMBER_OF_SPELLS = 3
}

[System.Serializable]
public enum ShieldSpell
{
    FIRE = 0,
    WATER = 1,
    SNOW = 2,
    NUMBER_OF_SHIELDS = 3
}

[System.Serializable]
public enum CustomSpell
{
    HEAL = 0,
    AREA_EXPLOSION = 1
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

[System.Serializable]
public class AreaExplosionNode : SpellNode
{
    public float damage;
    public float radius;
    public float pushForce;
    public CustomSpell spell;
}