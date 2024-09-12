using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActive_Romi : UnitActive
{
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
    }

    public override void Activate()
    {
        //base.Activate();
        BuffManager.Instance.GiveEffect(StatusEffect.TimeScale_Up, _values[0], _duration, _master, false);
    }

    public override bool EnableCheck()
    {
        if (_master.CurrentState == UnitAIState.Stun ||
            _master.CurrentState == UnitAIState.Death)
        {
            return false;
        }
        return true;
    }
    public override void OnSkillEvent()
    {
        base.OnSkillEvent();
        // 버프 적용
    }

    public override void DeActivate()
    {
        //base.DeActivate();
    }

    public override void Terminate()
    {
        base.Terminate();
    }


    protected override void Update()
    {

    }
}
