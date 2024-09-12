using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamagePopupManager : Singleton<DamagePopupManager>
{
    DamagePopup _prefab;
    int poolCount = 10;
    Queue<DamagePopup> _pool = new Queue<DamagePopup>();
    void Awake()
    {
        if (_prefab == null)
        {
            _prefab = Resources.Load<DamagePopup>("UI/DamagePopup");
        }
        for (int i = 0; i < poolCount; i++)
        {
            var obj = Instantiate(_prefab, transform);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    DamagePopup GetObject()
    {
        var result = _pool.Peek();
        if (result.gameObject.activeInHierarchy)
        {
            var obj = Instantiate(_prefab, transform);
            _pool.Enqueue(obj);
            result = obj;
        }
        _pool.Dequeue();
        _pool.Enqueue(result);
        return result;
    }

    public void Show(Vector3 pos, float damage, DamagePopupType type)
    {
        string text = Utility.FormatNumber(damage);
        GetObject().Init(pos, text, type);
    }

}
