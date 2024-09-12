using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_Immortal : UnitPassive
{
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        _master.OnDamaged += OnDamaged;
    }

    void OnDamaged(Unit unit, float damage)
    {
        if (_master.HP <= 0)
        {
            Activate(_master);
        }
    }

    public override void Activate(Unit target)
    {
        _effects.Add(BuffManager.Instance.GiveEffect(StatusEffect.Immortal, _duration, 0f, _master, false));
    }
    public override void Activate(List<Unit> targets)
    {

    }
    public override void Terminate()
    {
        base.Terminate();
        _master.OnDamaged -= OnDamaged;
    }
}
