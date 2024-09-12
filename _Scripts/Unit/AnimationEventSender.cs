using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSender : MonoBehaviour
{
    Unit _unit;
    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    public void Fire()
    {
        _unit.FireEvent();
    }

    public void Attack()
    {
        _unit.AttackEvent();
    }
    public void AttackStart()
    {
        _unit.AttackStart();
    }
    public void ActiveSkill()
    {
        _unit.SkillEvent();
    }
}
