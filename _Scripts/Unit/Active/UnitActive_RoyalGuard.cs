using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActive_RoyalGuard : UnitActive
{
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        _master.OnSkill += OnSkillEvent;
    }

    public override void Activate()
    {
        base.Activate();
        BuffManager.Instance.GiveEffect(StatusEffect.Shield, _master.MaxHP * _values[0] * 0.01f, _duration, _master, false);
    }

    public override bool EnableCheck()
    {
        if (_master.CurrentState == UnitAIState.Skill ||
            _master.CurrentState == UnitAIState.Death ||
            _master.CurrentState == UnitAIState.Stun ||
            _master.GetAnimatorInfo().IsName("Attack")) return false;
        return true;
    }
    public override void OnSkillEvent()
    {
        base.OnSkillEvent();
        // 버프 적용
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    public override void Terminate()
    {
        base.Terminate();
    }

    protected override void Update()
    {
        if (IsPlaying)
        {
            if (_master.CurrentState == UnitAIState.Skill)
            {
                if (_master.GetAnimatorInfo().IsName("Skill"))
                {
                    if (_master.GetAnimatorInfo().normalizedTime >= 1.0f)
                    {
                        DeActivate();
                    }
                }
                else
                {
                    //DeActivate();
                }
            }
            else
            {
                DeActivate();
            }
        }
    }
}
