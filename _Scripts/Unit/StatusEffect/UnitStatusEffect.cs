using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusEffect : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _effect;
    protected Unit _target;
    protected ImmunitableEffect _immuType;
    protected StatusEffect _type;
    protected float _duration;
    protected float _currentDuration;
    protected float _value;
    protected float _currentValue;
    protected bool _isActive = false;
    protected bool _isEverlasting = false;
    protected bool _isBenefit = false;
    public ImmunitableEffect ImmuType => _immuType;
    public StatusEffect Type => _type;
    public float Duration => _duration;
    public float CurrentDuration => _currentDuration;
    public float Value => _value;
    public float CurrnetValue => _currentValue;
    public bool IsActive => _isActive;
    public bool IsBenefit => _isBenefit;

    protected List<BuffSystem> _buff = new List<BuffSystem>();
    public List<BuffSystem> Buff => _buff;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }
    public virtual void Init(Unit unit, float duration, float value, bool isEverlasting)
    {
        gameObject.SetActive(true);
        _target = unit;
        if (_effect != null)
        {
            _effect.Play();
        }
        _duration = duration;
        _value = value;
        _currentValue = value;
        _isEverlasting = isEverlasting;
        if (!_isEverlasting)
            _currentDuration = _duration;
    }
    public virtual void ApplyEffect(Unit unit, float duration, float value, bool isEverlasting)
    {
        _isActive = true;
        _target.ApplyStatusEffect(this);
    }

    public virtual void SetCurrentValue(float newValue)
    {
        _currentValue = newValue;
    }

    public virtual void RemoveEffect()
    {
        if (_effect != null)
        {
            _effect?.Stop();
        }
        _target.RemoveStatusEffect(this);
        foreach (BuffSystem buff in _buff)
        {
            buff.DeActivation();
        }
        _buff.Clear();
        _isActive = false;
        gameObject.SetActive(false);
    }

    protected virtual void LateUpdate()
    {
        if (!IsActive) return;
        if (!_isEverlasting)
        {
            _currentDuration -= Time.deltaTime;
            if (_currentDuration <= 0)
            {
                RemoveEffect();
            }
        }
    }
}
