using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealSpell : HealSpell
{
    [Header("Entity References")]
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        playerController.ChangeHealth(Time.deltaTime * healSpellNode.healPointsPerSecond);
        if(CheckIfTimeIsUp())
        {
            EndHeal();
        }
    }

    public void SetupShield(float healLastingTime, float maxHealLastingTime, HealSpellNode healSpellNode, GameObject entityModel, PlayerController playerController, PanelControll uiPanelController)
    {
        this.healLastingTime = healLastingTime;
        this.maxHealLastingTime = maxHealLastingTime;

        this.healSpellNode = healSpellNode;

        this.entityModel = entityModel;
        SetupShieldObject(healSpellNode.prefab);

        this.playerController = playerController;
        playerController.SetHealing(true);

        this.uiPanelController = uiPanelController;
        uiPanelController.SetHeal("OK");
    }

    public override void EndHeal()
    {
        playerController.SetHealing(false);
        base.EndHeal();
    }

    public override void CollisionWithSpell()
    {
        EndHeal();
    }
}
