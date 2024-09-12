using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager i;
    public HpBar _prefab;
    public Dictionary<Unit, HpBar> _currentObj = new Dictionary<Unit, HpBar>();
    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void InitHpBar(Unit unit)
    {
        HpBar newBar = Instantiate(_prefab, transform);
        newBar.Init(unit);
        _currentObj.Add(unit, newBar);
    }

    public void TerminateHpBar(Unit unit)
    {
        if (_currentObj.ContainsKey(unit))
        {
            Destroy(_currentObj[unit].gameObject);
            _currentObj.Remove(unit);
        }
    }
}
