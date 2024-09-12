using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Romi : Unit
{
    public override void ActivateSkill()
    {
        _currentCoolTime = _finalCoolTime;
        _active.Activate();
    }
    protected override void State_Idle()
    {
        SetMove(false);
        _currentTarget = null;
        _rig.velocity = Vector2.zero;
        List<Unit> enemies = UnitManager.Instance.GetEnemies(Party);
        Unit target = enemies.Find(x => x.CurrentState != UnitAIState.Death);
        if (target != null)
        {
            LookTarget();
            _currentTarget = target;
            _currentTargetPos = target.transform.position;
            _passive.Activate(this);
        }
    }
    protected override void State_Attack()
    {
        List<Unit> enemies = UnitManager.Instance.GetEnemies(Party);
        Unit target = enemies.Find(x => x.CurrentState != UnitAIState.Death);
        if (target != null)
        {
            LookTarget();
            _currentTarget = target;
            _currentTargetPos = target.transform.position;
        }
        else
        {
            _passive.DeActivate();
        }
    }
}
