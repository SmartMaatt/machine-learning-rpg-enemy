using UnityEngine;
using TMPro;

public class NonePanelControll : PanelControll
{
    public override void ChangeHealth(float newValue) { }
    public override void ChangeMana(float newValue) { }
    public override void ResetHeal() { }
    public override void ResetHealClock() { }
    public override void ResetShield() { }
    public override void ResetShieldClock() { }
    public override void SetHeal(string value) { }
    public override void SetHealClock(float value) { }
    public override void SetShield(float value) { }
    public override void SetShieldClock(float value) { }
    public override void SetupHealth(float maxValue, float value) { }
    public override void SetupMana(float maxValue, float value) { }
    public override void SetupName(string name) { }
    public override void SetupScore(int score) { }
}
