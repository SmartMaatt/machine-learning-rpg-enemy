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
            EndHeal();
        }
    }

    public void SetupShield(float healLastingTime, float maxHealLastingTime, HealSpellNode healSpellNode, GameObject entityModel, AbstractEntity entity)
    {
        this.healLastingTime = healLastingTime;
        this.maxHealLastingTime = maxHealLastingTime;

        this.healSpellNode = healSpellNode;

        this.entityModel = entityModel;
        SetupShieldObject(healSpellNode.prefab);

        this.entity = entity;
    }

    public override void CollisionWithSpell()
    {
        EndHeal();
    }
}
