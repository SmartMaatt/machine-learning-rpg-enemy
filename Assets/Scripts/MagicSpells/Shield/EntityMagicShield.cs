using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMagicShield : MagicShield
{
    [Header("Entity References")]
    [SerializeField] private AbstractEntity entity;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        if(CheckIfTimeIsUp())
        {
            EndShield();
        }
    }

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, AbstractEntity entity)
    {
        this.shieldLastingTime = shieldLastingTime;
        this.maxShieldLastingTime = maxShieldLastingTime;

        this.shieldSpellNode = shieldSpellNode;
        GetReactionBasedOnProtection();

        this.entityModel = entityModel;
        SetupShieldObject(shieldSpellNode.prefab);

        this.entity = entity;
        entity.AddBlocking();
        ChangeArmour(shieldSpellNode.armour);
    }

    public override void CollisionWithSpell(CastSpellNode attackSpell)
    {
        if (attackSpell.spell == currentShield)
        {
            entity.GetHit(attackSpell.damage / 2);
        }
        else if (attackSpell.spell != currentProtection)
        {
            entity.GetHit(attackSpell.damage);
            ChangeArmour(-attackSpell.armourDamage);
            if (armour == 0)
            {
                entity.RemoveBlocking();
                EndShield();
            }
        }
    }
}
