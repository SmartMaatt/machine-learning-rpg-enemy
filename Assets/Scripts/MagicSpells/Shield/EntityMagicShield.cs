using UnityEngine;

public class EntityMagicShield : MagicShield
{
    [Header("Entity References")]
    [SerializeField] private Mage entity;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        if (CheckIfTimeIsUp())
        {
            EndShield();
        }
    }

    public void SetupShield(float shieldLastingTime, float maxShieldLastingTime, ShieldSpellNode shieldSpellNode, GameObject entityModel, Mage entity, PanelControll uiPanelController)
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

    public override void CollisionWithSpell(SpellInfo spellInfo, Vector3 ballMoveVector)
    {
        CastSpellNode attackSpellNode = spellInfo.castSpellNode;

        if (attackSpellNode.spell == currentShield)
        {
            entity.GetMagicHit(attackSpellNode.damage / 2, (int)attackSpellNode.spell);
            entity.GetSpeedController().ExplodePush(ballMoveVector, attackSpellNode.pushForce / 2);

            //RL rewarding
            if (spellInfo.IsAI())
            {
                spellInfo.AddRLReward(spellInfo.rlParams.useSpellSameAsShield);
            }
        }
        else if (attackSpellNode.spell != currentProtection)
        {
            entity.GetMagicHit(attackSpellNode.damage, (int)attackSpellNode.spell);
            ChangeArmour(-attackSpellNode.armourDamage);
            uiPanelController.SetShield(armour);

            //RL rewarding
            if (spellInfo.IsAI())
            {
                spellInfo.AddRLReward(spellInfo.rlParams.useStrongSpell);
            }

            entity.GetSpeedController().ExplodePush(ballMoveVector, attackSpellNode.pushForce);
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
