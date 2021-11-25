using UnityEngine;
using TMPro;

public class NonePanelControll : PanelControll
{
    public override void ChangeHealth(float newValue) { }
    public override void ChangeMana(float newValue) { }
    public override void SetupHealth(float maxValue, float value) { }
    public override void SetupMana(float maxValue, float value) { }
    public override void SetupName(string name) { }
}
