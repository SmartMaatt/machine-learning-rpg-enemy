using UnityEngine;

public class AreaExplosionExecuteNode : Node
{
    private Mage entity;
    private Vector3 currentDesination;
    private GetFloatValue Probability;

    public AreaExplosionExecuteNode(Mage entity, GetFloatValue Probability)
    {
        this.entity = entity;
        this.currentDesination = Vector3.zero;

        this.Probability = Probability;
    }

    public override NodeState Evaluate()
    {
        if (currentDesination != entity.GetCurrentDestination())
        {
            if (IsTimeToHeal())
            {
                entity.SetSpellType(SpellType.CUSTOM, (int)CustomSpell.AREA_EXPLOSION);
                entity.Attack();
            }
            currentDesination = entity.GetCurrentDestination();
        }
        return NodeState.SUCCESS;
    }

    private bool IsTimeToHeal()
    {
        return (Probability() - GenerateRandomNumber()) > 0;
    }

    private float GenerateRandomNumber()
    {
        return (float)UnityEngine.Random.RandomRange(0, 101) / 100;
    }
}
