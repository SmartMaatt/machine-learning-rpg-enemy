using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicShield : MagicShield
{
    [Header("Entity References")]
    [SerializeField] private PlayerHealth playerHealth;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        if(CheckIfTimeIsUp())
        {
            EndShield();
        }
    }

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, PlayerHealth playerHealth)
    {
        this.shieldLastingTime = shieldLastingTime;
        this.maxShieldLastingTime = maxShieldLastingTime;

        this.shieldSpellNode = shieldSpellNode;
        GetReactionBasedOnProtection();

        this.entityModel = entityModel;
        SetupShieldObject(shieldSpellNode.prefab);

        this.playerHealth = playerHealth;
        ChangeArmour(shieldSpellNode.armour);
    }

    public override void CollisionWithSpell(CastSpellNode attackSpell)
    {
        if (attackSpell.spell == currentShield)
        {
            playerHealth.ChangeHealth(attackSpell.damage / 2);
        }
        else if (attackSpell.spell != currentProtection)
        {
            playerHealth.ChangeHealth(attackSpell.damage);
            ChangeArmour(-attackSpell.armourDamage);
            if (armour == 0)
            {
                EndShield();
            }
        }
    }
}
