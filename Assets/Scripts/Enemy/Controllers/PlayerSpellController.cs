using UnityEngine;

class PlayerSpellController : SpellController
{
    [SerializeField] private float maxMana;
    [SerializeField] private float manaRestoreRate;
    private float mana;

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastFireball();
        }
    }

    protected override void ChargeMana()
    {
        AddMana(Time.deltaTime * manaRestoreRate);
    }

    private bool SetMana(float newMana)
    {
        if (newMana > 0 && newMana <= maxMana)
        {
            mana = newMana;
            return true;
        }
        return false;
    }

    private bool AddMana(float additionalMana)
    {
        if (additionalMana > 0)
        {
            mana += additionalMana;
            if (mana > maxMana)
            {
                mana = maxMana;
            }
            return true;
        }
        return false;
    }

    private bool UseMana(float manaToUse)
    {
        if ((manaToUse > 0) && (manaToUse < mana))
        {
            mana -= manaToUse;
            return true;
        }
        return false;
    }
}