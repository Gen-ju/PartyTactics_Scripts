using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    Kor,
    Eng,
    Max
}
public partial class GameData : Singleton<GameData>
{
    Dictionary<Language, Dictionary<int, string>> _dicText = new Dictionary<Language, Dictionary<int, string>>();
    public void LoadTextData(string csv)
    {
        _dicText.Clear();

        System.IO.StringReader reader = new System.IO.StringReader(csv);
        string line = reader.ReadLine(); // skip first row

        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
                break;

            int nPos = 0;

            string[] fields = line.Split(',');
            int index = int.Parse(fields[nPos++]);
            if (!_dicText.ContainsKey(Language.Kor))
            {
                _dicText.Add(Language.Kor, new Dictionary<int, string>());
            }
            if (!_dicText[Language.Kor].ContainsKey(index))
            {
                _dicText[Language.Kor].Add(index, fields[nPos++]);
            }
            //if (!_dicText.ContainsKey(Language.Eng))
            //{
            //    _dicText.Add(Language.Eng, new Dictionary<int, string>());
            //}
            //if (!_dicText[Language.Eng].ContainsKey(index))
            //{
            //    _dicText[Language.Eng].Add(index, fields[nPos++]);
            //}
        }

        reader.Close();
    }

    public string GetText(int index)
    {
        if (_dicText[Language.Kor].ContainsKey(index))
        {
            return _dicText[Language.Kor][index];
        }
        return null;
    }
}
