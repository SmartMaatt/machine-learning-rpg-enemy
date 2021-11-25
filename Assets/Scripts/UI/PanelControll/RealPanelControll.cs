using UnityEngine;
using TMPro;

public class RealPanelControll : PanelControll
{
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
}
