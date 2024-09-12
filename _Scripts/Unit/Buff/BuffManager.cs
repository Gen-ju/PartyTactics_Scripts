using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    Dictionary<StatusEffect, UnitStatusEffect> _effectPrefabs = new Dictionary<StatusEffect, UnitStatusEffect>();
    Dictionary<StatusEffect, Queue<UnitStatusEffect>> _effectPools = new Dictionary<StatusEffect, Queue<UnitStatusEffect>>();
    Queue<BuffSystem> m_Buff = new Queue<BuffSystem>();

    private void Awake()
    {
        Init();
    }
    void Init()
    {
        SetEffectPrefab();
        InitPool();
    }

    void SetEffectPrefab()
    {
        _effectPrefabs.Clear();
        for (int i = 0; i < (int)StatusEffect.Max; i++) 
        {
            UnitStatusEffect obj = Resources.Load<UnitStatusEffect>("Effect/StatusEffect/" + ((StatusEffect)i).ToString());
            if (!obj) continue;
            _effectPrefabs.Add((StatusEffect)i, obj);
        }

        for (int i = 0; i < 20; i++)
        {
            var obj = new GameObject("buff").AddComponent<BuffSystem>();
            obj.transform.parent = transform;
            obj.gameObject.SetActive(false);
            m_Buff.Enqueue(obj);
        }
    }
    void InitPool()
    {
        for (int i = 0; i < (int)StatusEffect.Max; i++) 
        {
            if (!_effectPrefabs.ContainsKey((StatusEffect)i))
            {
                continue;
            }
            var prefab = _effectPrefabs[(StatusEffect)i];
            if (prefab == null) return;
            _effectPools[(StatusEffect)i] = new Queue<UnitStatusEffect>();
            for (int j = 0; j < 10; j++)
            {
                var obj = Instantiate(prefab, transform);
                _effectPools[(StatusEffect)i].Enqueue(obj);
            } 
        }
    }

    UnitStatusEffect GetEffect(StatusEffect type)
    {
        var result = _effectPools[type].Peek();
        if (result.gameObject.activeInHierarchy)
        {
            var obj = Instantiate(_effectPrefabs[type], transform);
            _effectPools[type].Enqueue(obj);
            result = obj;
        }
        _effectPools[type].Dequeue();
        _effectPools[type].Enqueue(result);
        return result;
    }

    BuffSystem GetBuffSystem()
    {
        var result = m_Buff.Peek();
        if (result.gameObject.activeInHierarchy)
        {
            var obj = new GameObject("buff").AddComponent<BuffSystem>();
            obj.transform.parent = transform;
            obj.gameObject.SetActive(false);
            m_Buff.Enqueue(obj);
            result = obj;
        }
        m_Buff.Dequeue();
        m_Buff.Enqueue(result);
        return result;
    }

    public UnitStatusEffect GiveEffect(StatusEffect type, float value, float du, Unit target, bool isEverlasting)
    {
        if (target == null || target.CurrentState == UnitAIState.Death) return null;
        
        UnitStatusEffect effect = GetEffect(type);
        effect.ApplyEffect(target, du, value, isEverlasting);

        return effect;
    }

    public BuffSystem GiveBuff(BuffType type, float per, Unit target)
    {
        BuffSystem buff = GetBuffSystem();
        buff.Init(type, per, target);
        return buff;
    }
}
