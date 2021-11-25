using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicShield : MagicShield
{
    [Header("Entity References")]
    [SerializeField] private PlayerController playerStats;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        if(CheckIfTimeIsUp())
        {
            EndShield();
        }
    }

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, PlayerController playerHealth, PanelControll uiPanelController)
    {
        this.shieldLastingTime = shieldLastingTime;
        this.maxShieldLastingTime = maxShieldLastingTime;

        this.shieldSpellNode = shieldSpellNode;
        GetReactionBasedOnProtection();

        this.entityModel = entityModel;
        SetupShieldObject(shieldSpellNode.prefab);

        this.playerStats = playerHealth;
        ChangeArmour(shieldSpellNode.armour);

        this.uiPanelController = uiPanelController;
        uiPanelController.SetShield(armour);
    }

    public override void CollisionWithSpell(CastSpellNode attackSpellNode, Vector3 ballMoveVector)
    {
        if (attackSpellNode.spell == currentShield)
        {
            playerStats.ChangeHealth(attackSpellNode.damage / 2);
            playerStats.GetPlayerMovement().ExplodePush(ballMoveVector, attackSpellNode.pushForce / 2);
        }
        else if (attackSpellNode.spell != currentProtection)
        {
            playerStats.ChangeHealth(attackSpellNode.damage);
            ChangeArmour(-attackSpellNode.armourDamage);
            uiPanelController.SetShield(armour);

            playerStats.GetPlayerMovement().ExplodePush(ballMoveVector, attackSpellNode.pushForce);
            if (armour == 0)
            {
                EndShield();
            }
        }
    }
}
