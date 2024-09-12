using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_AllMaxHPUp : UnitPassive
{
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        Activate(master);
    }

    public override void Activate(Unit target)
    {
        var targets = UnitManager.Instance.CurrentUnits[target.Party];
        foreach (var t in targets)
        {
            _effects.Add(BuffManager.Instance.GiveEffect(StatusEffect.MaxHp_Up, _values[0], _duration, t, true));
        }
    }
    public override void Activate(List<Unit> targets)
    {

    }
    public override void Terminate()
    {
        base.Terminate();
    }
}
