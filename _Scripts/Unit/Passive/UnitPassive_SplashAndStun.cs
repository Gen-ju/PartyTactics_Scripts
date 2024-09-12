using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_SplashAndStun : UnitPassive
{


    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        _master.OnAttack += OnAttack;
    }

    void OnAttack(Unit unit)
    {
        _master._attackEffect2.transform.localScale = Vector3.one * _values[0] * 0.3f;
        var targetColls = Physics2D.OverlapCircleAll(unit.transform.position, _values[0]);
        foreach (var col in targetColls)
        {
            if (col.TryGetComponent<Unit>(out Unit target))
            {
                if (_master.Party == target.Party) continue;
                if (target.CurrentState == UnitAIState.Death) continue;
                if (unit != target)
                {
                    UnitManager.Instance.HandleAttack(_master, target, _master.Damage);
                }
                if (Utility.CheckProbability(_values[1] * 0.01f))
                {
                    BuffManager.Instance.GiveEffect(StatusEffect.Stun, 0f, _duration, target, false);
                }
            }
        }
    }

    public override void Activate(Unit target)
    {

    }
    public override void Activate(List<Unit> targets)
    {

    }
    public override void Terminate()
    {
        base.Terminate();
        _master.OnAttack -= OnAttack;
    }
}
