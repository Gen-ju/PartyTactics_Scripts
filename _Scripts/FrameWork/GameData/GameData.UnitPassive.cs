using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassiveData
{
    public int index;
    public int nameKey;
    public int infoKey;
    public PassiveSkillType type;
}
public partial class GameData : Singleton<GameData>
{
    Dictionary<int, UnitPassiveData> _dicPassiveData = new Dictionary<int, UnitPassiveData>();
    public void LoadPassiveData(string csv)
    {
        _dicPassiveData.Clear();

        System.IO.StringReader reader = new System.IO.StringReader(csv);
        string line = reader.ReadLine(); // skip first row

        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
                break;

            int nPos = 0;

            string[] fields = line.Split(',');
            UnitPassiveData data = new UnitPassiveData();
            data.index = int.Parse(fields[nPos++]);
            data.nameKey = int.Parse(fields[nPos++]);
            data.infoKey = int.Parse(fields[nPos++]);
            PassiveSkillType type = PassiveSkillType.None;
            if (Enum.TryParse(fields[nPos++], out type))
            {
                data.type = type;
            }
            else
            {
                data.type = PassiveSkillType.None;
            }

            if (!_dicPassiveData.ContainsKey(data.index))
            {
                _dicPassiveData.Add(data.index, data);
            }
        }

        reader.Close();
    }

    public UnitPassiveData GetPassiveData(int index)
    {
        if (_dicPassiveData.ContainsKey(index))
        {
            return _dicPassiveData[index];
        }
        return null;
    }
    public UnitPassive GetPassiveClass(Unit unit, PassiveSkillType type)
    {
        switch (type)
        {
            case PassiveSkillType.UnitPassive_AllDefUp: return unit.gameObject.AddComponent<UnitPassive_AllDefUp>();
            case PassiveSkillType.UnitPassive_AtkSpeedUp: return unit.gameObject.AddComponent<UnitPassive_AtkSpeedUp>();
            case PassiveSkillType.UnitPassive_AllDamageUp: return unit.gameObject.AddComponent<UnitPassive_AllDamageUp>();
            case PassiveSkillType.UnitPassive_AllMaxHPUp: return unit.gameObject.AddComponent<UnitPassive_AllMaxHPUp>();
            case PassiveSkillType.UnitPassive_Immortal: return unit.gameObject.AddComponent<UnitPassive_Immortal>();
            case PassiveSkillType.UnitPassive_RoyalJudgment: return unit.gameObject.AddComponent<UnitPassive_RoyalJudgment>();
            case PassiveSkillType.UnitPassive_Charge_Romi: return unit.gameObject.AddComponent<UnitPassive_Charge_Romi>();
            case PassiveSkillType.UnitPassive_SplashAndStun: return unit.gameObject.AddComponent<UnitPassive_SplashAndStun>();
            //*추가* 나중에 스킬 클래스 만들면 여기다가 추가하기
            default: return null;
        }
    }
}
