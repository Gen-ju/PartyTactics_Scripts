using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit : MonoBehaviour
{

    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] int _bulletPoolCount = 3;
    Queue<Bullet> _pool = new Queue<Bullet>();
    void InitPool()
    {
        if (_bulletPrefab == null) return;
        for (int i = 0; i < _bulletPoolCount; i++)
        {
            var obj = Instantiate(_bulletPrefab);
            _pool.Enqueue(obj);
        }
    }

    Bullet GetObject()
    {
        var result = _pool.Peek();
        if (result.gameObject.activeInHierarchy)
        {
            var obj = Instantiate(_bulletPrefab);
            _pool.Enqueue(obj);
            result = obj;
        }
        _pool.Dequeue();
        _pool.Enqueue(result);
        return result;
    }
}
