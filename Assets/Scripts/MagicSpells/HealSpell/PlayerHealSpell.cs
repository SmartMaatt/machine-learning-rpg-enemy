using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealSpell : HealSpell
{
    [Header("Entity References")]
    [SerializeField] private PlayerController playerStats;

    private void Update()
    {
        ChangeTime(-Time.deltaTime);
        playerStats.ChangeHealth(Time.deltaTime * healSpellNode.healPointsPerSecond);
        if(CheckIfTimeIsUp())
        {
            EndHeal();
        }
    }

    public void SetupShield(float healLastingTime, float maxHealLastingTime, HealSpellNode healSpellNode, GameObject entityModel, PlayerController playerHealth)
    {
        this.healLastingTime = healLastingTime;
        this.maxHealLastingTime = maxHealLastingTime;

        this.healSpellNode = healSpellNode;

        this.entityModel = entityModel;
        SetupShieldObject(healSpellNode.prefab);

        this.playerStats = playerHealth;
    }

    public override void CollisionWithSpell()
    {
        EndHeal();
    }
}
