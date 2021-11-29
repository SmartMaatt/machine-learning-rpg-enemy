using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealSpell : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] protected float healLastingTime;
    [SerializeField] protected float maxHealLastingTime;

    [Header("Spells")]
    [SerializeField] protected HealSpellNode healSpellNode;

    [Header("References")]
    [SerializeField] protected GameObject entityModel;
    protected GameObject healObject;
    protected PanelControll uiPanelController;

    protected void SetupShieldObject(GameObject heal)
    {
        healObject = Instantiate(heal, transform.position, Quaternion.identity);
        healObject.GetComponent<SpellInfo>().healSpellNode = healSpellNode;
        healObject.transform.parent = entityModel.transform;
    }

    public void ChangeTime(float change)
    {
        healLastingTime += change;
        healLastingTime = Mathf.Clamp(healLastingTime, 0f, maxHealLastingTime);
        uiPanelController.SetHealClock(healLastingTime);
    }

    protected bool CheckIfTimeIsUp()
    {
        return (healLastingTime <= 0);
    }

    public virtual void EndHeal()
    {
        uiPanelController.ResetHeal();
        uiPanelController.ResetHealClock();

        Destroy(healObject);
        Destroy(this);
    }

    public HealSpellNode GetHealSpellNode()
    {
        return healSpellNode;
    }

    public float GetHealTime()
    {
        return healLastingTime;
    }

    public abstract void CollisionWithSpell();
}
