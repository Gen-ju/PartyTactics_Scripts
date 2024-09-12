using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum eParty
{
    Player,
    Enemy,
}
public partial class Unit : MonoBehaviour
{
    protected Transform _root;
    protected Rigidbody2D _rig;
    protected Animator _ani;
    protected NavMeshAgent _agent;

    [SerializeField] protected eParty _party;
    [SerializeField] protected UnitType _type;
    [SerializeField] public ParticleSystem _attackEffect;
    [SerializeField] public ParticleSystem _attackEffect2;
    [SerializeField] protected ParticleSystem _skillEffect;
    [SerializeField] protected ParticleSystem _skillEffect2;
    [SerializeField] protected Bullet _bullet;
    [SerializeField] protected Bullet _bullet2;
    public eParty Party => _party;
    public UnitType Type => _type;

    bool _isActive = false;
    public bool _isLeader = false;


    float _initDamage = 1f;
    float _initCoolTime = 1f;
    float _initRange = 1f;
    float _initDelay = 1f;
    float _initScale = 3f;
    float _initDef = 5f;
    float _initCriPer = 5f;
    float _initSpeed = 4f;
    float _initCriDamage = 150f;
    float _initHp = 100f;


    [SerializeField] protected float _finalDamage = 1f;
    [SerializeField] protected float _finalCoolTime = 1f;
    [SerializeField] protected float _finalCoolSpeed = 1f;
    [SerializeField] protected float _finalAtkSpeed = 1f;
    [SerializeField] protected float _finalRange = 1f;
    [SerializeField] protected float _finalDelay = 1f;
    [SerializeField] protected float _finalScale = 3f;
    [SerializeField] protected float _finalDef = 5f;
    [SerializeField] protected float _finalHp = 100f;
    [SerializeField] protected float _finalSpeed = 4f;
    [SerializeField] protected float _finalCriPer = 5f;
    [SerializeField] protected float _finalCriDamage = 100f;
    [SerializeField] protected float _finalTimeScale = 1f;
    [SerializeField] protected float _finalReduce = 1f;
                      
    [SerializeField] protected float _currentCoolTime = 1f;
    [SerializeField] protected float _currentDelay = 1f;
    [SerializeField] protected float _currentHp = 100f;
    [SerializeField] protected float _currentShield = 0f;

    public float MaxHP => _finalHp;
    public float HP => _currentHp;
    public float MaxCoolTime => _finalCoolTime;
    public float CoolTime => _currentCoolTime;
    public float AtkSpeed => _finalAtkSpeed;
    public float Range => _finalRange;
    public float Damage => _finalDamage;
    public float CriticalPercent => _finalCriPer;
    public float Scale => _finalScale;
    public float Shield => _currentShield;
    public float TimeScale => _finalTimeScale;


    float _atkAniLength = 1f;

    bool[] _immunityEffects = new bool[(int)ImmunitableEffect.Max];
    Dictionary<StatusEffect, List<UnitStatusEffect>> _currnetStatusEffect = new Dictionary<StatusEffect, List<UnitStatusEffect>>();
    public List<BuffSystem> m_Buff = new List<BuffSystem>();

    protected UnitData _data;
    public UnitData Data => _data;

    protected UnitPassive _passive;
    protected UnitActive _active;

    public UnitPassive Passive => _passive;
    public UnitActive Active => _active;
    public virtual void Init(UnitData data, eParty party)
    {
        _data = data;
        _type = data.type;
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
        _party = party;
        _root = transform.Find("UnitRoot");
        _rig = GetComponentInChildren<Rigidbody2D>();
        _ani = GetComponentInChildren<Animator>();

        var animatorOverrideController = new AnimatorOverrideController(_ani.runtimeAnimatorController);

        foreach (var key in data.clipOverrides.Keys)
        {
            animatorOverrideController[key] = data.clipOverrides[key];
        }

        _ani.runtimeAnimatorController = animatorOverrideController;

        _initDamage = data.damage;
        _initCoolTime = data.coolTime;
        _initRange = data.range;
        _initDelay = data.delay;
        _initScale = data.scale;
        _initDef = data.def;
        _initCriPer = data.criPer;
        _initSpeed = data.speed;
        _initCriDamage = data.criDamage;
        _initHp = data.hp;

        _finalDamage = _initDamage;
        _finalCoolTime = _initCoolTime;
        _finalRange = _initRange;
        _finalDelay = _initDelay;
        _finalScale = _initScale;
        _finalDef = _initDef;
        _finalSpeed = _initSpeed;
        SetSpeed();
        _finalHp = _initHp;
        _finalCriPer = _initCriPer;
        _finalCriDamage = _initCriDamage;
        _currentHp = _finalHp;
        _currentDelay = 0;
        _currentCoolTime = _finalCoolTime;
        _finalCoolSpeed = 1f;
        _finalAtkSpeed = 1f;
        _finalTimeScale = 1f;
        _isActive = true;

        HpBarManager.i.InitHpBar(this);
        //if (_attackEffect != null)
        //    _attackEffect.transform.parent = null;
        if (_attackEffect2 != null)
            _attackEffect2.transform.parent = null;
        //if (_skillEffect != null)
        //    _skillEffect.transform.parent = null;
        //if (_skillEffect2 != null)
        //    _skillEffect2.transform.parent = null;
        InitPool();
        _root.localScale = Vector3.one * _finalScale;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        BuffCheck();
    }

    public virtual void SetPassiveSkill(UnitPassive skill)
    {
        _passive = skill;
    }
    public virtual void SetActiveSkill(UnitActive skill)
    {
        _active = skill;
    }


    public virtual void SetSpeed()
    {
        _agent.speed = _finalSpeed * _finalTimeScale;
    }

    public virtual void AttackStart()
    {
        if (_attackEffect != null)
        {
            //_attackEffect.transform.position = atkEnemy.transform.position;
            _attackEffect.Play();
        }
    }
    public delegate void OnAttackEventHandler(Unit target);
    public event OnAttackEventHandler OnAttack;
    public virtual void AttackEvent()
    {
        UnitManager.Instance.HandleAttack(this, atkEnemy, _finalDamage);
        _attackEffect2.transform.position = atkEnemy.transform.position + (Vector3.up * atkEnemy.Scale * 0.5f);
        _attackEffect2.Play();
        OnAttack?.Invoke(atkEnemy);
    }

    public virtual void FireEvent()
    {
        Bullet bullet = GetObject();
        bullet.Init(this, atkEnemy, _finalDamage);
    }

    public delegate void OnSkillEventHandler();
    public event OnSkillEventHandler OnSkill;
    public virtual void SkillEvent()
    {
        OnSkill?.Invoke();
        _skillEffect?.Play();
    }

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;
    public virtual void Death()
    {
        OnDeath?.Invoke();
        SetState(UnitAIState.Death);
    }

    public delegate void OnDamagedHandler(Unit unit, float damage);
    public event OnDamagedHandler OnDamaged;
    public virtual float Damaged(Unit attacker, float originDamage)
    {
        float damage = originDamage;
        _lastHitMe = attacker;
        _hitMeRememberTime = Random.Range(0.3f, 0.5f);

        if (!_currnetStatusEffect.ContainsKey(StatusEffect.Shield))
        {
            _currnetStatusEffect.Add(StatusEffect.Shield, new List<UnitStatusEffect>());
        }
        if (_currnetStatusEffect[StatusEffect.Shield].Count > 0)
        {
            for (int i = 0; i < _currnetStatusEffect[StatusEffect.Shield].Count; i++)
            {
                Shield shield = (Shield)_currnetStatusEffect[StatusEffect.Shield][i];
                if (shield.CurrnetValue >= damage)
                {
                    shield.SetCurrentValue(shield.CurrnetValue - damage);
                    damage = 0;
                }
                else
                {
                    damage = damage - shield.CurrnetValue;
                    shield.SetCurrentValue(0f);
                }
            }
            StatusEffectCheck();
        }
        OnDamaged?.Invoke(attacker, originDamage);
        _currentHp -= damage;
        return _currentHp;
    }
    public virtual float BuffChange(BuffType type, float origin)
    {
        float totalBuff = 0;

        if (m_Buff.Count > 0)
        {
            for (int i = 0; i < m_Buff.Count; i++)
            {
                if (m_Buff[i].type.Equals(type))
                {
                    totalBuff += m_Buff[i].percent;
                }
            }
        }

        return origin + (origin * totalBuff * 0.01f);
    }
    public virtual void BuffCheck()
    {
        _finalDamage = BuffChange(BuffType.Damage, _initDamage);
        _finalCoolSpeed = BuffChange(BuffType.CoolSpeed, 1f);
        _finalAtkSpeed = BuffChange(BuffType.AtkSpeed, 1f);
        _finalRange = BuffChange(BuffType.Range, _initRange);
        _agent.stoppingDistance = _finalRange - 0.1f;
        _finalScale = BuffChange(BuffType.Scale, _initScale);
        _finalDef = BuffChange(BuffType.Defense, _initDef);

        float hpAmount = _currentHp / _finalHp;
        _finalHp = BuffChange(BuffType.MaxHp, _initHp);
        _currentHp = _finalHp * hpAmount;
        _finalSpeed = BuffChange(BuffType.Speed, _initSpeed);
        SetSpeed();
        _finalCriPer = BuffChange(BuffType.CriPercent, _initCriPer);
        _finalCriDamage = BuffChange(BuffType.CriDamage, _initCriDamage);
        _finalTimeScale = BuffChange(BuffType.TimeScale, 1f);
        _finalReduce = BuffChange(BuffType.Reduce, 1f);
    }

    public void ApplyStatusEffect(UnitStatusEffect effect)
    {
        if (!_currnetStatusEffect.ContainsKey(effect.Type))
        {
            _currnetStatusEffect.Add(effect.Type, new List<UnitStatusEffect>());
        }
        _currnetStatusEffect[effect.Type].Add(effect);
        StatusEffectCheck();
    }

    public void RemoveStatusEffect(UnitStatusEffect effect)
    {
        if (_currnetStatusEffect[effect.Type].Contains(effect))
        {
            _currnetStatusEffect[effect.Type].Remove(effect);
        }
        StatusEffectCheck();
    }
    public void StatusEffectCheck()
    {
        // 복사본 생성
        m_Buff.Clear();
        var keysCopy = _currnetStatusEffect.Keys.ToList();

        foreach (var key in keysCopy)
        {
            if (_currnetStatusEffect[key].Count == 0)
            {
                switch (key)
                {
                    case StatusEffect.Stun:
                        {
                            if (CurrentState == UnitAIState.Stun)
                            {
                                SetState(UnitAIState.Idle);
                            }
                        }
                        break;
                    case StatusEffect.Shield:
                        {
                            _currentShield = 0f;
                        }
                        break;
                }
            }
            else
            {
                // 복사본으로 정렬
                var sortedEffects = _currnetStatusEffect[key].OrderByDescending(data => data.Value)
                                    .ThenByDescending(data => data.CurrentDuration)
                                    .ToList();
                m_Buff.AddRange(sortedEffects[0].Buff);
                _currnetStatusEffect[key] = sortedEffects;

                switch (key)
                {
                    case StatusEffect.Stun:
                        {
                            if (CheckImmunityEffect(ImmunitableEffect.Stun)) break;
                            SetState(UnitAIState.Stun);
                        }
                        break;
                    case StatusEffect.Shield:
                        {
                            _currentShield = 0f;
                            foreach (var effect in sortedEffects)
                            {
                                _currentShield += effect.CurrnetValue;
                            }
                        }
                        break;
                }
            }
        }
        BuffCheck();
    }

    public void SetImmunityEffect(ImmunitableEffect effect)
    {
        _immunityEffects[(int)effect] = true;
    }

    public bool CheckImmunityEffect(ImmunitableEffect effect)
    {
        return _immunityEffects[(int)effect];
    }


    public virtual void OnStartTrait(Unit attacker, ref float damage, Dictionary<AttackType, bool> flags) 
    {

    }
    public virtual void OnAttackTrait(Unit target, ref float damage, Dictionary<AttackType, bool> flags) 
    {
        // 50% 확률로 스턴을 부여한다
        //if (Utility.CheckProbability(0.5f))
        //{
        //    BuffManager.Instance.GiveEffect(StatusEffect.Stun, 0f, 2f, target);
        //}
    }
    public virtual void OnCriticalTrait(Unit target, ref float damage, Dictionary<AttackType, bool> flags) 
    {
        //BuffManager.Instance.GiveEffect(StatusEffect.Stun, 0f, 2f, target);
        damage *= _finalCriDamage * 0.01f;
    }

    public virtual void OnDamagedTrait(Unit attacker, ref float damage, Dictionary<AttackType, bool> flags)
    {
        // 받은 공격이 고정 데미지라면 방어력 계산을 하지 않음
        if (!flags[AttackType.Fixed])
        {
            //방어력 계산
            damage = Utility.ApplyDefenseReduction(damage, _finalDef);
        }

        // 받은 공격이 반사 데미지라면 다시 반사를 하지 않음
        if (!flags[AttackType.Reflect])
        {
            //// 받은 데미지의 10%로
            //float reflectDamage = damage * 0.1f;
            //// 고정 데미지, 반사 특성을 가진 공격을 가한다
            //UnitManager.Instance.HandleAttack(this, attacker, reflectDamage, AttackType.Fixed, AttackType.Reflect);
            //Debug.Log("반사 : " + reflectDamage);
        }
    }
    public virtual bool OnDeathTrait(Unit attacker, ref float damage, Dictionary<AttackType, bool> flags) 
    {
        if (_currnetStatusEffect.ContainsKey(StatusEffect.Immortal))
        {
            _currentHp = 1f;
            return false;
        }
        else
        {
            Death();
            return true;
        }
    }
    public virtual void OnKillTrait(Unit attacker, ref float damage, Dictionary<AttackType, bool> flags) 
    {

    }

}
