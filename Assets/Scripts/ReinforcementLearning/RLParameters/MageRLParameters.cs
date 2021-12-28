using UnityEngine;

public class MageRLParameters : AbstractEntityRLParameters
{
    [Header("Mage Rewards - Basic combat")]
    public float successShoot;
    public float tryShootWhenNoSpell;
    public float tryShootWhenNoMana;
    public float tryShootWhenNoCooldown;

    [Header("Mage Rewards - Enemy has shield")]
    public float useWeekSpell;
    public float useSpellSameAsShield;
    public float useStrongSpell;

    [Header("Mage Rewards - Self shield")]
    public float rechargeWrongShield;
    public float rechargeCurrentShield;

    [Header("Mage Rewards - other")]
    public float healWhenHealthUserLow;
    public float destroyHealSpell;
    public float areaSpellWhenHealthUnderCriticalLow;
}
