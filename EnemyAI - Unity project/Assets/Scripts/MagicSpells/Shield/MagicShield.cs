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
    protected PanelControll uiPanelController;


    /*>>> Setters <<<*/
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
        uiPanelController.SetShieldClock(shieldLastingTime);
    }

    public void ChangeArmour(int change)
    {
        armour += change;
        armour = Mathf.Clamp(armour, 0, 100);
    }

    
    /*>>> Getters <<<*/
    public ShieldSpellNode GetShieldSpellNode()
    {
        return shieldSpellNode;
    }

    public ShieldSpell GetShieldType()
    {
        return shieldSpellNode.spell;
    }

    public float GetShieldTime()
    {
        return shieldLastingTime;
    }

    public float GetNormalizedShieldTime()
    {
        return (shieldLastingTime / maxShieldLastingTime);
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


    /*>>> Abstract methods <<<*/
    public virtual void EndShield()
    {
        uiPanelController.ResetShield();
        uiPanelController.ResetShieldClock();

        Destroy(shieldObject);
        Destroy(this);
    }
    public abstract void CollisionWithSpell(SpellInfo spellInfo, Vector3 ballMoveVector);
}
