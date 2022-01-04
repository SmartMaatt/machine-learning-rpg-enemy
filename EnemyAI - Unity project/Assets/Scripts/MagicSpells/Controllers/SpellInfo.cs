using System;
using UnityEngine;

public class SpellInfo : MonoBehaviour
{
    public CastSpellNode castSpellNode;
    public ShieldSpellNode shieldSpellNode;
    public HealSpellNode healSpellNode;
    public AreaExplosionNode areaExplosionNode;

    public Mage owner;
    public MageRLParameters rlParams;

    public void SetupSpellInfoOwner(Mage owner)
    {
        try
        {
            this.owner = owner;
            rlParams = owner.GetMageRLParameters();
        }
        catch (NullReferenceException)
        {
            this.owner = null;
            rlParams = null;
        }
    }

    public void AddRLReward(float reward)
    {
        try
        {
            owner.AddRLReward(reward);
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Unexpected missing owner!");
        }
    }

    public bool IsAI()
    {
        return owner != null;
    }
}
