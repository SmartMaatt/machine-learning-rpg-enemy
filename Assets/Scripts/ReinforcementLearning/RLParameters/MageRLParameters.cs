using UnityEngine;

public class MageRLParameters : AbstractEntityRLParameters
{
    [Header("Mage Rewards - Basic combat")]
    public float successShoot;
    public float tryShootWhenNoSpell;
    public float tryShootWhenNoMana;
    public float tryShootWhenNoCooldown;

    [Header("Mage Rewards - Shield combat")]
    public float useWeekSpell;
    public float useSpellSameAsShield;
    public float useStrongSpell;
    public float useSpellWhenAttackOrChase;
    public float useShieldWhenHide;

    [Header("Mage Rewards - other")]
    public float healWhenHealthUserLow;
    public float destroyHealSpell;
    public float areaSpellWhenHealthUnderCriticalLow;
}
