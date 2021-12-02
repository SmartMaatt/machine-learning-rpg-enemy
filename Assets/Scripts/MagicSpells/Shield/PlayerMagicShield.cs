using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicShield : MagicShield
{
    [Header("Entity References")]
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        if(CheckIfTimeIsUp())
        {
            EndShield();
        }
    }

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, PlayerController playerController, PanelControll uiPanelController)
    {
        this.shieldLastingTime = shieldLastingTime;
        this.maxShieldLastingTime = maxShieldLastingTime;

        this.shieldSpellNode = shieldSpellNode;
        GetReactionBasedOnProtection();

        this.entityModel = entityModel;
        SetupShieldObject(shieldSpellNode.prefab);

        this.playerController = playerController;
        this.playerController.SetBlocking(true);
        ChangeArmour(shieldSpellNode.armour);

        this.uiPanelController = uiPanelController;
        uiPanelController.SetShield(armour);
    }

    public override void EndShield()
    {
        playerController.SetBlocking(false);
        base.EndShield();
    }

    public override void CollisionWithSpell(SpellInfo spellInfo, Vector3 ballMoveVector)
    {
        CastSpellNode attackSpellNode = spellInfo.castSpellNode;

        if (attackSpellNode.spell == currentShield)
        {
            playerController.ChangeHealth(attackSpellNode.damage / 2);
            playerController.GetPlayerMovement().ExplodePush(ballMoveVector, attackSpellNode.pushForce / 2);

            //RL rewarding
            if (spellInfo.IsAI())
            {
                spellInfo.AddRLReward(spellInfo.rlParams.useSpellSameAsShield);
            }
        }
        else if (attackSpellNode.spell != currentProtection)
        {
            playerController.ChangeHealth(attackSpellNode.damage);
            ChangeArmour(-attackSpellNode.armourDamage);
            uiPanelController.SetShield(armour);

            //RL rewarding
            if (spellInfo.IsAI())
            {
                spellInfo.AddRLReward(spellInfo.rlParams.useStrongSpell);
            }

            playerController.GetPlayerMovement().ExplodePush(ballMoveVector, attackSpellNode.pushForce);
            if (armour == 0)
            {
                EndShield();
            }
        }
        else if (attackSpellNode.spell == currentProtection)
        {
            //RL rewarding
            if (spellInfo.IsAI())
            {
                spellInfo.AddRLReward(spellInfo.rlParams.useWeekSpell);
            }
        }
    }
}
