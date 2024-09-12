using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitPassive : MonoBehaviour
{
    protected Unit _master;
    protected List<float> _values;
    protected float _duration;
    protected List<UnitStatusEffect> _effects = new List<UnitStatusEffect>();
    public virtual void Init(Unit master, List<float> values, float duration)
    {
        _master = master;
        _master.OnDeath += Terminate;
        _values = values.ToList();
        _duration = duration;
        _effects.Clear();
        _master.SetPassiveSkill(this);
    }
    public virtual void Activate(Unit target)
    {

    }
    public virtual void DeActivate()
    {
        foreach (var effect in _effects)
        {
            effect.RemoveEffect();
        }
        _effects.Clear();
    }
    public virtual void Activate(List<Unit> targets)
    {

    }
    public virtual void Terminate()
    {
        DeActivate();
        _master.OnDeath -= Terminate;
    }
}
