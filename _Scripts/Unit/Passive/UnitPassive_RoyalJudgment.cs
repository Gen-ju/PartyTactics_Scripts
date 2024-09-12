using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_RoyalJudgment : UnitPassive
{
    int _hitCount = 0;
    ParticleSystem _particle, _particle2;


    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        _particle = Instantiate(Resources.Load<ParticleSystem>("Effect/Passive/RoyalJudgment1"), _master.transform);
        _particle.transform.localPosition = Vector3.up * _master.Scale * 0.5f;
        _particle.transform.localScale = Vector3.one * _master.Scale;
        _particle2 = Instantiate(Resources.Load<ParticleSystem>("Effect/Passive/RoyalJudgment2"), _master.transform);
        _particle.transform.localPosition = Vector3.up * _master.Scale * 0.5f;
        _particle.transform.localScale = Vector3.one * _master.Scale;
        _master.OnDamaged += OnDamaged;
    }

    void OnDamaged(Unit unit, float damage)
    {
        _hitCount++;
        if (_hitCount >= 10)
        {
            _hitCount = 0;
            Activate(UnitManager.Instance.GetEnemies(_master.Party));
        }
        else
        {
            _particle.Play();
        }
    }

    public override void Activate(Unit target)
    {

    }
    public override void Activate(List<Unit> targets)
    {
        _particle2.Play();
        foreach (Unit target in targets)
        {
            if (target != null)
            {
                UnitManager.Instance.HandleAttack(_master, target, _master.Damage * _values[0] * 0.01f, AttackType.Fixed, AttackType.Reflect);
            }
        }
    }
    public override void Terminate()
    {
        base.Terminate();
        _master.OnDamaged -= OnDamaged;
    }
}
