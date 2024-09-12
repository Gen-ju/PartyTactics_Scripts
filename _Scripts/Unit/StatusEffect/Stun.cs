using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : UnitStatusEffect
{
    public override void ApplyEffect(Unit unit, float duration, float value, bool isEverlasting)
    {
        base.Init(unit, duration, 0f, isEverlasting);
        _isBenefit = false;
        _immuType = ImmunitableEffect.Stun;
        _type = StatusEffect.Stun;
        base.ApplyEffect(unit, duration, 0f, isEverlasting);
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        _effect.transform.position = _target.transform.position + (Vector3.up * _target.Scale);
    }
}
