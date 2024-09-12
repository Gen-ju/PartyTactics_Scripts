using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActiveData
{
    public int index;
    public int nameKey;
    public int infoKey;
    public ActiveSkillType type;
}
public partial class GameData : Singleton<GameData>
{
    Dictionary<int, UnitActiveData> _dicActiveData = new Dictionary<int, UnitActiveData>();
    public void LoadActiveData(string csv)
    {
        _dicActiveData.Clear();

        System.IO.StringReader reader = new System.IO.StringReader(csv);
        string line = reader.ReadLine(); // skip first row

        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
                break;

            int nPos = 0;

            string[] fields = line.Split(',');
            UnitActiveData data = new UnitActiveData();
            data.index = int.Parse(fields[nPos++]);
            data.nameKey = int.Parse(fields[nPos++]);
            data.infoKey = int.Parse(fields[nPos++]);
            ActiveSkillType type = ActiveSkillType.None;
            if (Enum.TryParse(fields[nPos++], out type))
            {
                data.type = type;
            }
            else
            {
                data.type = ActiveSkillType.None;
            }

            if (!_dicActiveData.ContainsKey(data.index))
            {
                _dicActiveData.Add(data.index, data);
            }
        }

        reader.Close();
    }

    public UnitActiveData GetActiveData(int index)
    {
        if (_dicActiveData.ContainsKey(index))
        {
            return _dicActiveData[index];
        }
        return null;
    }

    public UnitActive GetActiveClass(Unit unit, ActiveSkillType type)
    {
        switch (type)
        {
            case ActiveSkillType.UnitActive_RoyalGuard: return unit.gameObject.AddComponent<UnitActive_RoyalGuard>();
            case ActiveSkillType.UnitActive_Romi: return unit.gameObject.AddComponent<UnitActive_Romi>();
            //*추가* 나중에 스킬 클래스 만들면 여기다가 추가하기
            default: return null;
        }
    }
}
