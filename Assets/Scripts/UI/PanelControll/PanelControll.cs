using UnityEngine;
using TMPro;

public abstract class PanelControll : MonoBehaviour
{
    [Header("Main elements")]
    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected ValueBar manaBar;
    [SerializeField] protected ValueBar healthBar;

    [Header("Shield elements")]
    [SerializeField] protected TMP_Text shield;
    [SerializeField] protected TMP_Text shieldClock;

    [Header("Heal elements")]
    [SerializeField] protected TMP_Text heal;
    [SerializeField] protected TMP_Text healClock;

    public abstract void SetupMana(float maxValue, float value);
    public abstract void ChangeMana(float newValue);
    public abstract void SetupHealth(float maxValue, float value);
    public abstract void ChangeHealth(float newValue);
    public abstract void SetupName(string name);

    public abstract void SetShield(float value);
    public abstract void ResetShield();
    public abstract void SetShieldClock(float value);
    public abstract void ResetShieldClock();
    public abstract void SetHeal(string value);
    public abstract void ResetHeal();
    public abstract void SetHealClock(float value);
    public abstract void ResetHealClock();
}