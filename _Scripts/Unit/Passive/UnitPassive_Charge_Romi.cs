using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class UnitPassive_Charge_Romi : UnitPassive
{
    ParticleSystem _particle0, _particle1;
    Transform _rangeEffect;
    bool _isPlaying = false;
    float _chargeTime = 0f;
    float _range = 0f;
    float _damage = 0f;
    public override void Init(Unit master, List<float> values, float duration)
    {
        base.Init(master, values, duration);
        _particle0 = Instantiate(Resources.Load<ParticleSystem>("Effect/Passive/Romi_Charge"));
        _rangeEffect = _particle0.transform.Find("Range");
        _particle1 = Instantiate(Resources.Load<ParticleSystem>("Effect/Passive/Romi_Shot"));
    }

    public override void Activate(Unit target)
    {
        if (_isPlaying) return;
        if (_master.AnimationIsPlaying(UnitAnimationType.Attack) || _master.AnimationIsPlaying(UnitAnimationType.Attack_Additive)) return;
        _master.PlayAnimation(UnitAnimationType.Attack);
        _master.SetState(UnitAIState.Attack);
        _chargeTime = 0f;

        Vector3 pos = _master.transform.position + (_master.Scale * (Vector3.up * 2f));
        _particle0.transform.position = pos;
        _particle1.transform.position = pos;
        _particle0.Play();


        _isPlaying = true;
    }
    public override void Activate(List<Unit> targets)
    {

    }

    public override void DeActivate()
    {
        if (!_isPlaying) return;
        _isPlaying = false;
        _chargeTime = 0f;
        _range = 0f;
        _damage = 0f;
        _master.PlayAnimation(UnitAnimationType.Idle);
        _master.SetState(UnitAIState.Idle);
        _particle0.Stop();
        _particle1.Stop();
        _rangeEffect.gameObject.SetActive(false);
        base.DeActivate();
    }

    public override void Terminate()
    {
        base.Terminate();
    }

    private void LateUpdate()
    {
        if (_isPlaying)
        {
            if (_master.CurrentState == UnitAIState.Attack)
            {
                _chargeTime += Time.deltaTime * _master.AtkSpeed * _master.TimeScale;
                _range = _master.Range * (_chargeTime - _duration) * _values[0] * 0.01f;
                _range = Mathf.Clamp(_range, 0f, _range);
                _damage = _master.Damage + (_master.Damage * (_chargeTime - _duration) * _values[1] * 0.01f);
                if (_chargeTime > _duration)
                {
                    if (!_rangeEffect.gameObject.activeSelf)
                    {
                        _rangeEffect.gameObject.SetActive(true);
                    }
                    _rangeEffect.transform.localScale = Vector3.one * _range * 2f;
                    List<Unit> enemies = UnitManager.Instance.GetEnemies(_master.Party);
                    foreach (Unit enemy in enemies)
                    {
                        if (enemy._isDeath) continue;
                        if (Vector2.Distance(transform.position, enemy.transform.position) < _range)
                        {
                            Attack(enemy);
                            return;
                        }
                    }
                }
                else
                {
                    if (_rangeEffect.gameObject.activeSelf)
                    {
                        _rangeEffect.gameObject.SetActive(false);
                    }
                    return;
                }
            }
            else if (_master.CurrentState == UnitAIState.Attack_Additive)
            {
                if (_rangeEffect.gameObject.activeSelf)
                {
                    _rangeEffect.gameObject.SetActive(false);
                }
                if (_master.GetAnimatorInfo().normalizedTime >= 1.0f)
                {
                    DeActivate();
                    return;
                }
                
            }
            else
            {
                DeActivate();
                return;
            }
        }
    }

    void Attack(Unit target)
    {
        Vector2 direction = new Vector2(
               _particle1.transform.position.x - target.transform.position.x,
               _particle1.transform.position.y - target.transform.position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _particle1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 180f));

        _particle0.Stop();
        _particle1.Play();
        _master.PlayAnimation(UnitAnimationType.Attack_Additive);
        _master.SetState(UnitAIState.Attack_Additive);
        UnitManager.Instance.HandleAttack(_master, target, _damage);
    }
}
