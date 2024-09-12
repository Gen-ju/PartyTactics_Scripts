using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public BuffType type;
    public float percent;
    public float currentTime;
    public Unit target;



    public void Init(BuffType type, float per, Unit t)
    {
        gameObject.SetActive(true);
        this.type = type;
        percent = per;
        target = t;
    }
    public void DeActivation()
    {
        target.m_Buff.Remove(this);
        target.BuffCheck();
        gameObject.SetActive(false);
    }
}
