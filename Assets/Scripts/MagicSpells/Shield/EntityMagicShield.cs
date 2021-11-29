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

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, AbstractEntity entity, PanelControll uiPanelController)
    {
        this.shieldLastingTime = shieldLastingTime;
        this.maxShieldLastingTime = maxShieldLastingTime;

        this.shieldSpellNode = shieldSpellNode;
        GetReactionBasedOnProtection();

        this.entityModel = entityModel;
        SetupShieldObject(shieldSpellNode.prefab);

        this.entity = entity;
        entity.SetBlocking(true);
        ChangeArmour(shieldSpellNode.armour);

        this.uiPanelController = uiPanelController;
        uiPanelController.SetShield(armour);
    }

    public override void EndShield()
    {
        entity.SetBlocking(false);
        base.EndShield();
    }

    public override void CollisionWithSpell(CastSpellNode attackSpellNode, Vector3 ballMoveVector)
    {
        if (attackSpellNode.spell == currentShield)
        {
            entity.GetHit(attackSpellNode.damage / 2);
            entity.GetSpeedController().ExplodePush(ballMoveVector, attackSpellNode.pushForce / 2);
        }
        else if (attackSpellNode.spell != currentProtection)
        {
            entity.GetHit(attackSpellNode.damage);
            ChangeArmour(-attackSpellNode.armourDamage);
            uiPanelController.SetShield(armour);

            entity.GetSpeedController().ExplodePush(ballMoveVector, attackSpellNode.pushForce);
            if (armour == 0)
            {
                EndShield();
            }
        }
    }
}
