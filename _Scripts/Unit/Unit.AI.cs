using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum LookDirection
{
    Left,
    Right
}
public enum UnitAIState
{
    Idle,
    MoveToAttack,
    PartyCollisionCheck,
    FllowLeader,
    Attack,
    Attack_Additive,
    RunAway,
    Skill,
    Skill_Additive,
    Death,
    Stun,
    Max
}
public partial class Unit : MonoBehaviour
{
    [SerializeField] protected UnitAIState _currentState = UnitAIState.Idle;
    public UnitAIState CurrentState => _currentState;


    protected Unit _currentTarget = null;
    protected Unit _lastHitMe = null;
    protected float _hitMeRememberTime = 1f;

    protected Vector2 _currentTargetPos;
    public Unit CurrentTarget => _currentTarget;


    private void Update()
    {
        if (_isDeath) 
        {
            SetState(UnitAIState.Death);
            return; 
        }
        if (_currnetStatusEffect.ContainsKey(StatusEffect.Stun) && _currnetStatusEffect[StatusEffect.Stun].Count > 0)
        {
            SetState(UnitAIState.Stun);
        }
        if (ActiveSkillEnableCheck())
        {
            ActivateSkill();
            return;
        }
        StateCheck();
        _currentCoolTime -= Time.deltaTime * _finalCoolSpeed;
        _currentDelay -= Time.deltaTime * _finalAtkSpeed * _finalTimeScale;
        _hitMeRememberTime -= Time.deltaTime;
    }
    public virtual void ActivateSkill()
    {
        SetState(UnitAIState.Skill);
        _ani.Play("Skill");
        _currentCoolTime = _finalCoolTime;
        _active.Activate();
    }


    public virtual bool ActiveSkillEnableCheck()
    {
        if (_isActive && _active)
        {
            if (_currentCoolTime <= 0f && _active.EnableCheck())
            {
                return true;
            }
            else 
            {
                return false; 
            }
        }
        else
        {
            return false;
        }
    }

    public void PlayAnimation(UnitAnimationType type)
    {
        _ani.Play(type.ToString());
    }

    public bool AnimationIsPlaying(UnitAnimationType type)
    {
        return _ani.GetCurrentAnimatorStateInfo(0).IsName(type.ToString());
    }

    public AnimatorStateInfo GetAnimatorInfo()
    {
        return _ani.GetCurrentAnimatorStateInfo(0);
    }

    void TargetExistCheck()
    {
        if (CurrentTarget == null || CurrentTarget._isDeath)
        {
            if (CurrentState != UnitAIState.Skill &&
                CurrentState != UnitAIState.Stun &&
                CurrentState != UnitAIState.Death &&
                CurrentState != UnitAIState.Attack_Additive &&
                CurrentState != UnitAIState.Skill_Additive)
            {
                SetState(UnitAIState.Idle);
            }
        }
    }

    protected void LookTarget(bool reverse = false)
    {
        if (_currentTarget != null)
        {
            if (transform.position.x > _currentTarget.transform.position.x)
            {
                if (reverse)
                {
                    LookDir(LookDirection.Right);
                }
                else
                {
                    LookDir(LookDirection.Left);
                }
            }
            else
            {
                if (reverse)
                {
                    LookDir(LookDirection.Left);
                }
                else
                {
                    LookDir(LookDirection.Right);
                }
            }
        }
    }

    void LookDir(LookDirection dir)
    {
        if (dir == LookDirection.Right)
        {
            _root.localScale = new Vector2(_initScale * -1f, _initScale);
        }
        else if (dir == LookDirection.Left)
        {
            _root.localScale = new Vector2(_initScale * 1f, _initScale);
        }
    }

    protected virtual void StateCheck()
    {
        _ani.speed = 1f;
        if (_agent.enabled == false)
        {
            _agent.enabled = true;
        }
        switch (CurrentState)
        {
            case UnitAIState.Idle: State_Idle(); break;
            case UnitAIState.MoveToAttack: State_MoveToAttack(); break;
            case UnitAIState.PartyCollisionCheck: State_PartyCollisionCheck(); _findTimer = 1f; break;
            case UnitAIState.FllowLeader: State_FllowLeader(); break;
            case UnitAIState.Attack: State_Attack(); break;
            case UnitAIState.Attack_Additive: State_Attack_Additive(); break;
            case UnitAIState.RunAway: State_RunAway(); break;
            case UnitAIState.Skill: State_Skill(); break;
            case UnitAIState.Skill_Additive: State_Skill_Additive(); break;
            case UnitAIState.Death: State_Death(); break;
            case UnitAIState.Stun: State_Stun(); break;
            default: SetState(UnitAIState.Idle); break;
        }
    }

    protected virtual void State_Idle()
    {
        SetMove(false);
        _currentTarget = null;
        _rig.velocity = Vector2.zero;
        List<Unit> enemies = new List<Unit>();
        if (Party == eParty.Player)
        {
            enemies = UnitManager.Instance.CurrentUnits[eParty.Enemy];
        }
        else
        {
            enemies = UnitManager.Instance.CurrentUnits[eParty.Player];
        }
        Unit target = enemies.Find(x => x.CurrentState != UnitAIState.Death);
        if (target != null)
        {
            _currentTarget = target;
            _currentTargetPos = target.transform.position;
            SetState(UnitAIState.MoveToAttack);
        }
        if (!_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _ani.Play("Idle");
        }
    }
    protected virtual void State_MoveToAttack()
    {
        //try
        //{
        SetMove(true);
        FindAttackAbleTarget();
        LookTarget();
        if (CurrentTarget != null)
        {
            if (!_ani.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            {
                _ani.Play("Move");
            }


            float distanceToTarget = Vector2.Distance(transform.position, _currentTarget.transform.position);

            if (distanceToTarget >= _initRange)
            {
                _currentTargetPos = _currentTarget.transform.position;
                _agent.SetDestination(_currentTargetPos);
                //Vector2 direction = (_currentTargetPos - (Vector2)transform.position).normalized;
                //_rig.velocity = direction * 4f;
            }
            else
            {
                SetState(UnitAIState.PartyCollisionCheck);
                //if (_currentDelay > 0)
                //{
                //    _agent.SetDestination(_currentTargetPos);
                //}
                //else
                //{
                //    SetState(UnitAIState.PartyCollisionCheck);
                //}
            }
        }
        else
        {
            SetState(UnitAIState.Idle);
        }

        //}
        //catch
        //{
        //    SetState(UnitAIState.Idle);
        //}
    }
    float _findTimer = 1f;
    protected virtual void State_PartyCollisionCheck()
    {
        FindAttackAbleTarget();
        SetState(UnitAIState.Attack);
    }
    protected virtual void State_FllowLeader()
    {

    }
    float aniSpeed = 1f;
    Unit atkEnemy;
    protected virtual void State_Attack()
    {
        try
        {
            LookTarget();
            SetMove(false);
            _rig.velocity = Vector2.zero;
            if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                //공격속도에 따른 애니메이션 속도 조정
                float aniLength = _ani.GetCurrentAnimatorStateInfo(0).length; 
                if (_finalDelay < aniLength * _finalAtkSpeed)
                {
                    aniSpeed = Mathf.Clamp(aniLength * _finalAtkSpeed / _finalDelay, 0f, 100f);
                }
                else
                {
                    aniSpeed = 1f;
                }
                _ani.speed = aniSpeed * _finalTimeScale;
                _currentDelay = _finalDelay;

                if (_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f || _currentDelay <= 0)
                {
                    _ani.Play("Idle");
                }
            }
            else
            {
                TargetExistCheck();
                _ani.Play("Idle");
            }
            if (_currentDelay < 0)
            {
                atkEnemy = CurrentTarget;
                _ani.Play("Attack");
            }

            float distanceToTarget = Vector2.Distance(transform.position, _currentTarget.transform.position);

            if (distanceToTarget > _initRange)
            {
                SetState(UnitAIState.Idle);
            }

        }
        catch
        {
            SetState(UnitAIState.Idle);
        }

    }

    protected virtual void State_Attack_Additive()
    {

    }
    protected virtual void State_RunAway()
    {
        LookTarget(true);
        if (!_ani.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            _ani.Play("Move");
        }

        if (_lastHitMe != null)
        {
            Vector2 backPos = transform.position + (transform.position - _lastHitMe.transform.position);
            backPos.y += Random.Range(-1f, 1f);
            SetMove(true);
            _agent.SetDestination(backPos);
        }
        if (_hitMeRememberTime < 0 || _lastHitMe.CurrentState == UnitAIState.Death)
        {
            _lastHitMe = null;
            SetState(UnitAIState.Idle);
        }
    }
    protected virtual void State_Skill()
    {
        LookTarget();
        SetMove(false);
    }
    protected virtual void State_Skill_Additive()
    {

    }

    public bool _isDeath = false;
    protected virtual void State_Death()
    {
        if (!_isDeath)
        {
            SetMove(false);
            _ani.Play("Death");
            HpBarManager.i.TerminateHpBar(this);
            _isDeath = true;
        }
    }
    protected virtual void State_Stun()
    {
        SetMove(false);
        if (!_ani.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
        {
            _ani.Play("Stun");
        }
    }
    public virtual void SetState(UnitAIState state)
    {
        _currentState = state;
    }

    protected virtual void FindAttackAbleTarget()
    {
        List<Unit> enemies = UnitManager.Instance.GetEnemies(_party);
        foreach (Unit enemy in enemies)
        {
            if (enemy._isDeath) continue;
            if (Vector2.Distance(transform.position, enemy.transform.position) < _finalRange)
            {
                _currentTarget = enemy;
                return;
            }
        }
    }

    protected virtual void SetMove(bool movable)
    {
        if (_agent.isStopped = movable)
        {
            _agent.isStopped = !movable;
        }
        if (!movable)
        {
            _agent.velocity = Vector3.zero;
        }
    }
}
