using UnityEngine;

class EnemySpellController : SpellController
{
    private Mage _entity;
    private float maxMana;
    private float manaRestoreRate;

    private void Start()
    {
        base.Start();
        _entity = GetComponent<Mage>();
        maxMana = _entity.maxMana;
        manaRestoreRate = _entity.manaRestoreRate;
    }

    private void Update()
    {
        base.Update();
    }

    protected override void ChargeMana()
    {
        _entity.AddMana(Time.deltaTime * manaRestoreRate);
    }
}