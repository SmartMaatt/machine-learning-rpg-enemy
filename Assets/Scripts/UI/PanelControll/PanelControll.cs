using UnityEngine;
using TMPro;

public abstract class PanelControll : MonoBehaviour
{
    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected ValueBar manaBar;
    [SerializeField] protected ValueBar healthBar;

    public abstract void SetupMana(float maxValue, float value);
    public abstract void ChangeMana(float newValue);
    public abstract void SetupHealth(float maxValue, float value);
    public abstract void ChangeHealth(float newValue);
    public abstract void SetupName(string name);
}