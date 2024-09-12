using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public partial class GameData : Singleton<GameData>
{
    public void Init(Action action)
    {
        DontDestroyOnLoad(gameObject);
        FrameWork.Instance.Network.Send(eRequestType.TextTable, (s) => 
        {
            LoadTextData(s);
            FrameWork.Instance.Network.Send(eRequestType.UnitDataTable, (s) =>
            {
                LoadUnitInfo(s);
                FrameWork.Instance.Network.Send(eRequestType.UnitPassiveData, (s) =>
                {
                    LoadPassiveData(s);
                    FrameWork.Instance.Network.Send(eRequestType.UnitActiveData, (s) =>
                    {
                        LoadActiveData(s);
                        action?.Invoke();
                    });
                });
            });
        });
    }
}
