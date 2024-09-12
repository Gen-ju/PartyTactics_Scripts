using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_AtkSpeedUp : UnitPassive
{
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        Activate(master);
    }

    public override void Activate(Unit target)
    {
        _effects.Add(BuffManager.Instance.GiveEffect(StatusEffect.AtkSpeed_Up, _values[0], _duration, target, true));
    }
    public override void Activate(List<Unit> targets)
    {

    }
    public override void Terminate()
    {
        base.Terminate();
    }
}
