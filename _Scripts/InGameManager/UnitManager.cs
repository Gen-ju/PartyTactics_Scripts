using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{

    private Dictionary<eParty, List<Unit>> _currentUnits = new Dictionary<eParty, List<Unit>>();
    public Dictionary<eParty, List<Unit>> CurrentUnits => _currentUnits;


    public List<int> aUnits = new List<int>();
    public List<int> bUnits = new List<int>();


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Invoke("CreatePlayerUnits", 0.5f);
        Invoke("CreateEnemyUnits", 0.5f);
    }

    public void CreatePlayerUnits()
    {
        for (int i = 0; i < aUnits.Count; i++)
        {
            GameObject unitPrefab = GameData.Instance.CreateUnit(aUnits[i]).gameObject;
            unitPrefab.transform.localRotation = Quaternion.Euler(Vector3.zero);
            unitPrefab.transform.position = new Vector3(-8, -4 + (i * -1), 0);
            Unit unit = unitPrefab.GetComponent<Unit>();
            UnitData data = GameData.Instance.GetUnitData(aUnits[i]);
            InitUnit(unit, data, eParty.Player);
        }
        InitAdditive(eParty.Player);
    }

    public void CreateEnemyUnits()
    {
        for (int i = 0; i < bUnits.Count; i++)
        {
            GameObject unitPrefab = GameData.Instance.CreateUnit(bUnits[i]).gameObject;
            unitPrefab.transform.position = new Vector3(8, -4 + (i * -1), 0);
            Unit unit = unitPrefab.GetComponent<Unit>();
            UnitData data = GameData.Instance.GetUnitData(bUnits[i]);
            InitUnit(unit, data, eParty.Enemy);
        }
        InitAdditive(eParty.Enemy);
    }

    void InitUnit(Unit unit, UnitData data, eParty party)
    {
        if (!_currentUnits.ContainsKey(party))
        {
            _currentUnits.Add(party, new List<Unit>());
        }
        _currentUnits[party].Add(unit);

        unit.Init(data, party);
    }

    void InitAdditive(eParty party)
    {
        foreach (Unit unit in _currentUnits[party])
        {
            UnitPassiveData passiveData = GameData.Instance.GetPassiveData(unit.Data.passiveKey);
            if (passiveData != null && passiveData.type != PassiveSkillType.None)
            {
                UnitPassive passive = GameData.Instance.GetPassiveClass(unit, passiveData.type);
                passive.Init(unit, unit.Data.passiveValue, unit.Data.passiveDuration);
            }
            UnitActiveData activeData = GameData.Instance.GetActiveData(unit.Data.activeKey);
            if (activeData != null && activeData.type != ActiveSkillType.None)
            {
                UnitActive active = GameData.Instance.GetActiveClass(unit, activeData.type);
                if (active != null)
                {
                    active.Init(unit, unit.Data.activeValue, unit.Data.activeDuration);
                }
            }
        }
    }

    public List<Unit> GetEnemies(eParty party)
    {
        if (party == eParty.Player)
        {
            return _currentUnits[eParty.Enemy];
        }
        else
        {
            return _currentUnits[eParty.Player];
        }
    }

    public void HandleAttack(Unit attacker, Unit target, float damage, params AttackType[] flags)
    {
        // 공격이 어떤 타입을 가지는지 체크
        Dictionary<AttackType, bool> flagDic = SetAttackFlags(flags);
        float damageAmount = damage;

        attacker.OnAttackTrait(target, ref damageAmount, flagDic);

        bool isCritical = Utility.CheckProbability(attacker.CriticalPercent * 0.01f);
        if (isCritical)
        {
            // 상대가 크리티컬 면역인지 체크
            if (target.CheckImmunityEffect(ImmunitableEffect.Critical))
            {
                isCritical = false;
            }
        }
        if (isCritical)
        {
            attacker.OnCriticalTrait(target, ref damageAmount, flagDic);
        }
        target.OnDamagedTrait(attacker, ref damageAmount, flagDic);

        DamagePopupManager.Instance.Show(target.transform.position, damageAmount, isCritical ? DamagePopupType.Critical : DamagePopupType.Normal);

        float remainHp = target.Damaged(attacker, damageAmount);
        if (remainHp <= 0)
        {
            // 사망
            if (target.OnDeathTrait(target, ref damageAmount, flagDic))
            {
                // 킬
                attacker.OnKillTrait(target, ref damageAmount, flagDic);
            }
        }
        // 더 해야할 것 : 타겟이 해당 공격으로 죽는지 확인하고 OnDeathTrait, OnKillTrait과 이벤트 호출할 것
    }

    Dictionary<AttackType, bool> SetAttackFlags(AttackType[] flags)
    {
        Dictionary<AttackType, bool> result = new Dictionary<AttackType, bool>();
        for (int i = 0; i < (int)AttackType.Max;  i++)
        {
            result.Add((AttackType)i, false);
        }
        foreach (AttackType type in flags)
        {
            result[type] = true;
        }
        return result;
    }


}
