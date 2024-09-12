using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : UnitStatusEffect
{
    public override void ApplyEffect(Unit unit, float duration, float value, bool isEverlasting)
    {
        base.Init(unit, duration, value, isEverlasting);
        _isBenefit = true;
        _immuType = ImmunitableEffect.None;
        _type = StatusEffect.Shield;
        base.ApplyEffect(unit, duration, value, isEverlasting);
    }

    public override void SetCurrentValue(float newValue)
    {
        base.SetCurrentValue(newValue);
        if (newValue <= 0)
        {
            RemoveEffect();
        }
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        //_effect.transform.position = _target.transform.position + (Vector3.up * _target.Scale);
    }
}
