using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealSpell : HealSpell
{
    [Header("Entity References")]
    [SerializeField] private AbstractEntity entity;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        entity.ChangeHealth(Time.deltaTime * healSpellNode.healPointsPerSecond);
        if(CheckIfTimeIsUp())
        {
            entity.SetHealing(false);
            EndHeal();
        }
    }

    public void SetupShield(float healLastingTime, float maxHealLastingTime, HealSpellNode healSpellNode, GameObject entityModel, AbstractEntity entity, PanelControll uiPanelController)
    {
        this.healLastingTime = healLastingTime;
        this.maxHealLastingTime = maxHealLastingTime;

        this.healSpellNode = healSpellNode;

        this.entityModel = entityModel;
        SetupShieldObject(healSpellNode.prefab);

        this.entity = entity;
        entity.SetHealing(true);

        this.uiPanelController = uiPanelController;
        uiPanelController.SetHeal("OK");
    }

    public override void EndHeal()
    {
        entity.SetHealing(false);
        base.EndHeal();
    }

    public override void CollisionWithSpell(SpellInfo spellInfo)
    {
        if (spellInfo.IsAI())
        {
            spellInfo.AddRLReward(spellInfo.rlParams.destroyHealSpell);
        }

        EndHeal();
    }
}
