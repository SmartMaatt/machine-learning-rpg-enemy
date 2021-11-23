using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntitySpellController))]
public class Mage : AbstractEntity
{
    /*Mage params*/
    [Header("Mana")]
    public float maxMana;
    public float manaRestoreRate;

    [Header("Spell timings")]
    public float maxShieldTime;
    public float maxHealTime;

    [Header("Behaviour tree time clock")]
    public float startTreeTime;
    public float repeatTreeTime;

    private EntitySpellController spellController;


    /*Unity methods*/
    protected override void Awake()
    {
        base.Awake();
        spellController = GetComponent<EntitySpellController>();

        ConstructBehaviourTree();
        InvokeRepeating("EvaluateBehaviourTree", startTreeTime, repeatTreeTime);
    }



    /*Behaviour tree methods*/
    private void EvaluateBehaviourTree()
    {
        decisionTreeTopNode.Evaluate();
    }

    protected override void ConstructBehaviourTree()
    {
        /*>>> Cover branch <<<*/
        /*Cover level 6*/
        IsCoverAvaliableNode coverAvaliableNode = new IsCoverAvaliableNode(this, avaliableCovers, player.transform, this.transform, new GetFloatValue(() => sightConeRange));
        GoToDestinationPoint goToCoverNode = new GoToDestinationPoint(this, new GetFloatValue[] {
            new GetFloatValue(() => runSpeed),
            new GetFloatValue(() => restSpeed),
            new GetFloatValue(() => accelerationChaseBonus),
            new GetFloatValue(() => acceleration)
        });
        HealSpellExecuteNode healSpellExecuteNode = new HealSpellExecuteNode(this, new GetFloatValue(() => coverHealProbability));

        /*Cover level 5*/
        // Go to cover (Sequence)
        // <Safe jump> => Sense decisions (Selector)

        /*Cover level 4*/
        IsDirectContactNode isCoveredNode = new IsDirectContactNode(player.transform, this.transform, PlayerLayer);
        // Find cover (Selector)

        /*Cover level 3*/
        HealthNode healthNode = new HealthNode(new GetFloatValue[] {
            new GetFloatValue(() => health),
            new GetFloatValue(() => lowHealthThreshold)
        });
        // Try to take cover (Selector)

        /*Cover level 2*/
        // Low health (Sequence)


        //===================================================================


        /*>>> Panic branch <<<*/
        /*Panic level 4*/
        AreaExplosionNode areaExplosionNode = new AreaExplosionNode(this);
        // <Safe jump> => Try to take cover (Selector)

        /*Panic level 3*/
        HealthNode criticHealthNode = new HealthNode(new GetFloatValue[] {
            new GetFloatValue(() => health),
            new GetFloatValue(() => criticalLowHealthThreshold)
        });
        // Panic reaction (Sequence)

        /*Panic level 2*/
        // Panic (Sequence)


        //===================================================================


        /*>>> Attact branch <<<*/
        /*Attack level 4*/
        RangeNode attackingRangeNode = new RangeNode(player.transform, this.transform, new GetFloatValue(() => attackRange));
        IsDirectContactNode isPlayerCovered = new IsDirectContactNode(player.transform, this.transform, PlayerLayer);
        Inverter isPlayerNotCovered = new Inverter(isPlayerCovered);

        /*Attack level 3*/
        // Clear spot (Sequence)
        AttackNode attackNode = new AttackNode(this, player.transform, this.transform, new GetFloatValue[] {
            new GetFloatValue(() => restSpeed),
            new GetFloatValue(() => breakAcceleration)
        });

        /*Attack level 2*/
        // Attack (Sequence)


        //===================================================================


        /*>>> Chanse branch <<<*/
        /*Chase level 4*/
        RangeNode hearRangeNode = new RangeNode(player.transform, this.transform, new GetFloatValue(() => hearRange));
        SightNode sightNode = new SightNode(player.transform, this.transform, new GetFloatValue[] {
            new GetFloatValue(() => sightRange),
            new GetFloatValue(() => sightConeRange)
        });

        /*Chase level 3*/
        // Senses (Selector)
        ChaseNode chaseNode = new ChaseNode(this, player.transform, new GetFloatValue[] {
            new GetFloatValue(() => runSpeed),
            new GetFloatValue(() => restSpeed),
            new GetFloatValue(() => accelerationChaseBonus),
            new GetFloatValue(() => acceleration)
        });

        /*Chase level 2*/
        // Chase (Sequence)


        //===================================================================


        /*>>> Wander branch <<<*/
        /*Wander level 2*/
        WanderNode wanderNode = new WanderNode(this, this.transform, new GetFloatValue[] {
            new GetFloatValue(() => restSpeed),
            new GetFloatValue(() => acceleration),
            new GetFloatValue(() => walkPointRange),
            new GetFloatValue(() => minRestTime),
            new GetFloatValue(() => maxRestTime),
            new GetFloatValue(() => turnMaxAngle)
        });
        GoToDestinationPoint goToWanderPointNode = new GoToDestinationPoint(this, new GetFloatValue[] {
            new GetFloatValue(() => walkSpeed),
            new GetFloatValue(() => restSpeed),
            new GetFloatValue(() => acceleration),
            new GetFloatValue(() => acceleration)
        });


        //===================================================================


        /*>>> Safe jump nodes <<<*/
        SafeJumpNode jumpToTryToTakeCoverSelector = new SafeJumpNode();
        SafeJumpNode jumpToSenseDecisionsSelector = new SafeJumpNode();


        /*>>> Health decisions <<<*/
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode, healSpellExecuteNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, jumpToSenseDecisionsSelector });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence lowHealthSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        Sequence panicReactionSequence = new Sequence(new List<Node> { areaExplosionNode, jumpToTryToTakeCoverSelector });
        Sequence criticalLowHealthSequence = new Sequence(new List<Node> { criticHealthNode, panicReactionSequence });


        /*>>> Sense decisions <<<*/
        Selector sensesSelector = new Selector(new List<Node> { hearRangeNode, sightNode });
        Sequence chaseSequence = new Sequence(new List<Node> { sensesSelector, chaseNode });

        Sequence clearSpot = new Sequence(new List<Node> { attackingRangeNode, isPlayerNotCovered });
        Sequence attackSequence = new Sequence(new List<Node> { clearSpot, attackNode });


        /*>>> Top level decisions <<<*/
        Selector healthDecisionsSelector = new Selector(new List<Node> { criticHealthNode, lowHealthSequence });
        Selector senseDecisionsSelector = new Selector(new List<Node> { attackSequence, chaseSequence });
        Selector wanderSelector = new Selector(new List<Node> { wanderNode, goToWanderPointNode });


        /*Setup safe jumps*/
        jumpToTryToTakeCoverSelector.SetJumpNode(tryToTakeCoverSelector);
        jumpToSenseDecisionsSelector.SetJumpNode(senseDecisionsSelector);


        decisionTreeTopNode = new Selector(new List<Node> { healthDecisionsSelector, senseDecisionsSelector, wanderSelector });
    }



    /*Hurting / Die methods*/
    public override void Die()
    {
        Debug.Log("I've never died before!");
    }

    public override void GetHit(float damage)
    {
        ChangeHealth(-damage);
        Debug.Log("Health: " + health);
    }

    

    /*Attack methods*/
    public override void Attack()
    {
        spellController.ExecuteSpell();
    }

    public void SetSpellType(SpellType spellType, int spellID)
    {
        try
        {
            spellController.SetSpellType(spellType, spellID);
        }
        catch (UnknownSpellException err)
        {
            Debug.Log(err);
        }
    }



    /*Debuging gizmoses*/
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, hearRange);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);

        //Gizmos.color = Color.blue;
        //Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward);
        //RaycastHit hit;

        //if (Physics.SphereCast(ray, obstacleRange / 2, out hit))
        //{
        //    Gizmos.DrawWireSphere(hit.transform.position, obstacleRange / 2);
        //}
    }
}
