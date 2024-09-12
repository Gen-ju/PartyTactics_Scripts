using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : Bullet
{
    protected Vector3 _direction;
    protected float _terminateTime = 10f, _currentTime = 0f;
    protected override void Awake()
    {
        _isInit = false;
        gameObject.SetActive(false);
    }
    public override void Init(Unit unit, Unit target, params float[] values)
    {
        base.Init(unit, target, values);
        Vector3 targetPos = target.transform.position + (Vector3.up * 0.5f * target.Scale);
        _direction = (targetPos - transform.position).normalized;
        _currentTime = 0f;
    }

    private void Update()
    {
        if (_isInit)
        {
            _currentTime += Time.deltaTime;
            Vector3 moveVector = _direction * 30f * Time.deltaTime * + _unit.TimeScale;
            transform.Translate(moveVector, Space.World);
            if (_currentTime > _terminateTime)
            {
                Terminate();
            }
            //transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, step);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isInit) return;
        if (col.TryGetComponent<Unit>(out Unit hit))
        {
            if (hit.Party == _target.Party)
            {
                UnitManager.Instance.HandleAttack(_unit, _target, _values[0]);
                Terminate();
            }
        }

    }

    public override void Terminate()
    {
        base.Terminate();
    }
}
