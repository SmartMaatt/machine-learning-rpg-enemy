using UnityEngine;
using TMPro;

public class RealPanelControll : PanelControll
{
    private void Start()
    {
        ResetShield();
        ResetShieldClock();
        ResetHeal();
        ResetHealClock();
    }

    public override void SetupMana(float maxValue, float value)
    {
        manaBar.SetupBar(maxValue, value);
    }

    public override void ChangeMana(float newValue)
    {
        manaBar.ChangeValue(newValue);
    }

    public override void SetupHealth(float maxValue, float value)
    {
        healthBar.SetupBar(maxValue, value);
    }

    public override void ChangeHealth(float newValue)
    {
        healthBar.ChangeValue(newValue);
    }

    public override void SetupName(string name)
    {
        nameText.text = name;
    }

    public override void SetupScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public override void SetShield(float value)
    {
        shield.text = ((int)value).ToString();
    }

    public override void ResetShield()
    {
        shield.text = "__";
    }

    public override void SetShieldClock(float value)
    {
        shieldClock.text = ((int)value).ToString();
    }

    public override void ResetShieldClock()
    {
        shieldClock.text = "__";
    }

    public override void SetHeal(string value)
    {
        heal.text = value;
    }

    public override void ResetHeal()
    {
        heal.text = "__";
    }

    public override void SetHealClock(float value)
    {
        healClock.text = ((int)value).ToString();
    }

    public override void ResetHealClock()
    {
        healClock.text = "__";
    }
}
