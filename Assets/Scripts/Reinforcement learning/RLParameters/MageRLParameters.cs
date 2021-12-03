using UnityEngine;

public class MageRLParameters : AbstractEntityRLParameters
{
    [Header("Mage Rewards - Basic combat")]
    public float missShoot;
    public float successShoot;
    public float tryShootWhenNoMana;

    [Header("Mage Rewards - Enemy has shield")]
    public float useWeekSpell;
    public float useSpellSameAsShield;
    public float useStrongSpell;
    public float useSpellsWhenNotInAttackMode;

    [Header("Mage Rewards - Self shield")]
    public float getHitByWeekSpell;
    public float getHitBySpellSameAsShield;
    public float getHitByStrongSpell;
    public float rechargeWrongShield;
    public float rechargeCurrentShield;

    [Header("Mage Rewards - other")]
    public float healWhenHealthUserLow;
    public float destroyHealSpell;
    public float areaSpellWhenHealthUnderCriticalLow;
}
