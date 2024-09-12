using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitActive : MonoBehaviour
{
    protected Unit _master;
    protected List<float> _values = new List<float>();
    protected float _duration;
    protected bool _isPlaying = false;

    public bool IsPlaying => _isPlaying;

    public virtual void Init(Unit master, List<float> values, float duration)
    {
        _master = master;
        _values = values;
        _duration = duration;
        _master.SetActiveSkill(this);
    }

    public virtual bool EnableCheck()
    {
        return false;
    }

    public virtual void Activate()
    {
        _isPlaying = true;
    }

    public virtual void OnSkillEvent()
    {

    }

    public virtual void DeActivate()
    {
        if (_master.CurrentState != UnitAIState.Death)
        {
            _master.SetState(UnitAIState.Idle);
        }
        _isPlaying = false;
    }

    public virtual void Terminate()
    {

    }

    protected virtual void Update()
    {
        if (_isPlaying)
        {
            if (!StatusCheck())
            {
                DeActivate();
            }
        }
    }

    protected virtual bool StatusCheck()
    {
        if (_master != null)
        {
            return _master.CurrentState == UnitAIState.Skill;
        }
        else
        {
            return false;
        }
    }
}
