using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _hitEffect;
    protected Unit _unit, _target;
    protected bool _isInit = false;
    protected float[] _values;

    public bool IsInit => _isInit;

    protected virtual void Awake()
    {
        _isInit = false;
        gameObject.SetActive(false);
    }
    public virtual void Init(Unit unit, Unit target, params float[] values)
    {
        _unit = unit;
        _target = target;
        transform.position = unit.transform.position + (Vector3.up * _unit.Scale * 0.5f);
        _values = values;
        _isInit = true;
        gameObject.SetActive(true);
    }
    public virtual void Terminate()
    {
        _isInit = false;
        if (_hitEffect != null)
        {
            _hitEffect.Play();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
