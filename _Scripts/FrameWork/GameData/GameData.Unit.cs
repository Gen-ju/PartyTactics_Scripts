using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum UnitType
{
    Tank,
    Range,
    Melee,
    Support,
    Max
}
public enum UnitAnimationType
{
    Idle,
    Move,
    Attack,
    Attack_Additive,
    Skill,
    Skill_Additive,
    Stun,
    Death,
    Max
}
public class UnitData
{
    public int index;
    public string title;
    public string name;
    public int cost;
    public UnitType type;
    public int passiveKey;
    public float passiveDuration;
    public List<float> passiveValue;
    public int activeKey;
    public float activeDuration;
    public List<float> activeValue;

    public float coolTime;
    public float damage;
    public float delay;
    public float criPer;
    public float criDamage;
    public float range;
    public float hp;
    public float def;
    public float scale;
    public float speed;

    public Dictionary<string, AnimationClip> clipOverrides = new Dictionary<string, AnimationClip>();
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}


public partial class GameData : Singleton<GameData>
{
    Dictionary<int, UnitData> _dicUnitInfo = new Dictionary<int, UnitData>();
    public void LoadUnitInfo(string csv)
    {
        _dicUnitInfo.Clear();

        System.IO.StringReader reader = new System.IO.StringReader(csv);
        string line = reader.ReadLine(); // skip first row

        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
                break;

            int nPos = 0;

            string[] fields = line.Split(',');

            UnitData item = new UnitData();

            item.index = int.Parse(fields[nPos++]);
            item.title = GetText(int.Parse(fields[nPos++]));
            item.name = GetText(int.Parse(fields[nPos++]));
            item.cost = int.Parse(fields[nPos++]);
            item.type = (UnitType)Enum.Parse(typeof(UnitType) ,fields[nPos++]);

            if (!string.IsNullOrEmpty(fields[nPos++]))
            {
                item.passiveKey = int.Parse(fields[nPos - 1]);
            }
            float f;
            if (float.TryParse(fields[nPos++], out f))
            {
                item.passiveDuration = f;
            }
            item.passiveValue = new List<float>();
            for (int i = 0; i < 7; i++)
            {
                if (float.TryParse(fields[nPos++], out f))
                {
                    item.passiveValue.Add(f);
                }
            }
            if (!string.IsNullOrEmpty(fields[nPos++]))
            {
                item.activeKey = int.Parse(fields[nPos - 1]);
            };
            if (float.TryParse(fields[nPos++], out f))
            {
                item.activeDuration = f;
            }
            item.activeValue = new List<float>();
            for (int i = 0; i < 7; i++)
            {
                if (float.TryParse(fields[nPos++], out f))
                {
                    item.activeValue.Add(f);
                }
            }
            item.coolTime = float.Parse(fields[nPos++]);
            item.damage = float.Parse(fields[nPos++]);
            item.delay = float.Parse(fields[nPos++]);
            item.criPer = float.Parse(fields[nPos++]);
            item.criDamage = float.Parse(fields[nPos++]);
            item.range = float.Parse(fields[nPos++]);
            item.hp = float.Parse(fields[nPos++]);
            item.def = float.Parse(fields[nPos++]);
            item.scale = float.Parse(fields[nPos++]);
            item.speed = float.Parse(fields[nPos++]);

            item.clipOverrides.Clear();
            item.clipOverrides.Add(UnitAnimationType.Idle.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Move.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Attack.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Attack_Additive.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Skill.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Skill_Additive.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Stun.ToString(), GetUnitClip(fields[nPos++]));
            item.clipOverrides.Add(UnitAnimationType.Death.ToString(), GetUnitClip(fields[nPos++]));


            if (_dicUnitInfo.ContainsKey(item.index) == false)
            {
                _dicUnitInfo.Add(item.index, item);
            }
        }

        reader.Close();
    }

    AnimationClip GetUnitClip(string clipName)
    {
        string path = string.Format("Unit/Animations/{0}", clipName);
        AnimationClip clip = Resources.Load<AnimationClip>(path);
        return clip;
    }
    ParticleSystem GetUnitEffect(string clipName)
    {
        string path = string.Format("Unit/Effect/{0}", clipName);
        return Resources.Load<ParticleSystem>(path);
    }

    Bullet GetUnitBullet(string clipName)
    {
        string path = string.Format("Unit/Bullet/{0}", clipName);
        return Resources.Load<Bullet>(path);
    }

    public UnitData GetUnitData(int index)
    {
        if (_dicUnitInfo.ContainsKey(index))
        {
            return _dicUnitInfo[index];
        }
        return null;
    }

    public Unit CreateUnit(int index)
    {
        if (_dicUnitInfo.ContainsKey(index))
        {
            string path = string.Format("Unit/Unit{0}", index);
            Unit unit = Instantiate(Resources.Load<Unit>(path));
            return unit;
        }
        else
        {
            return null;
        }
    }
}
