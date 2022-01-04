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


    /*>>> Setters <<<*/
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


    /*>>> Getters <<<*/
    protected bool CheckIfTimeIsUp()
    {
        return (healLastingTime <= 0);
    }

    public HealSpellNode GetHealSpellNode()
    {
        return healSpellNode;
    }

    public float GetHealTime()
    {
        return healLastingTime;
    }

    public float GetNormalizedHealTime()
    {
        return (healLastingTime / maxHealLastingTime);
    }


    /*>>> Abstract methods <<<*/
    public virtual void EndHeal()
    {
        uiPanelController.ResetHeal();
        uiPanelController.ResetHealClock();

        Destroy(healObject);
        Destroy(this);
    }

    public abstract void CollisionWithSpell(SpellInfo spellInfo);
}
