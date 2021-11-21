using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicShield : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] protected float shieldLastingTime;
    [SerializeField] protected float maxShieldLastingTime;

    [Header("Spells")]
    [SerializeField] protected ShieldSpellNode shieldSpellNode;
    [SerializeField] protected CastSpell currentShield;
    [SerializeField] protected CastSpell currentProtection;

    [Header("Armour")]
    [Range(0, 100)]
    [SerializeField] protected int armour;

    [Header("References")]
    [SerializeField] protected GameObject entityModel;
    protected GameObject shieldObject;

    protected void SetupShieldObject(GameObject shield)
    {
        shieldObject = Instantiate(shield, transform.position, Quaternion.identity);
        shieldObject.GetComponent<SpellInfo>().shieldSpellNode = shieldSpellNode;
        shieldObject.transform.parent = entityModel.transform;
    }

    public void ChangeTime(float change)
    {
        shieldLastingTime += change;
        shieldLastingTime = Mathf.Clamp(shieldLastingTime, 0f, maxShieldLastingTime);
    }

    public void ChangeArmour(int change)
    {
        armour += change;
        armour = Mathf.Clamp(armour, 0, 100);
    }

    protected bool CheckIfTimeIsUp()
    {
        return (shieldLastingTime <= 0);
    }
    
    protected void GetReactionBasedOnProtection()
    {
        switch (shieldSpellNode.spell)
        {
            case ShieldSpell.FIRE:
                currentShield = CastSpell.FIRE;
                currentProtection = CastSpell.SNOW;
                break;

            case ShieldSpell.WATER:
                currentShield = CastSpell.WATER;
                currentProtection = CastSpell.FIRE;
                break;

            case ShieldSpell.SNOW:
                currentShield = CastSpell.SNOW;
                currentProtection = CastSpell.WATER;
                break;
        }
    }

    public void EndShield()
    {
        Destroy(shieldObject);
        Destroy(this);
    }

    public ShieldSpellNode GetShieldSpellNode()
    {
        return shieldSpellNode;
    }

    public abstract void CollisionWithSpell(CastSpellNode attackSpell);
}
