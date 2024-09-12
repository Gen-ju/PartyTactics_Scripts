using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense_Up : UnitStatusEffect
{
    public override void ApplyEffect(Unit unit, float duration, float value, bool isEverlasting)
    {
        base.Init(unit, duration, 0f, isEverlasting);
        _isBenefit = true;
        _immuType = ImmunitableEffect.None;
        _type = StatusEffect.Defense_Up;
        _buff.Add(BuffManager.Instance.GiveBuff(BuffType.Defense, value, unit));
        base.ApplyEffect(unit, duration, value, isEverlasting);
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
    }
    protected override void LateUpdate()
    {
        //base.LateUpdate();
    }
}
