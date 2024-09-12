using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HpBar : MonoBehaviour
{
    float _hpTargetFill, _shieldTargetFill, _hpBarWidth, _barRectY;
    [SerializeField] Image _hPBar, _shieldBar, _coolTimeBar;
    [SerializeField] RectTransform _shieldBarRect;
    Unit _unit;
    Camera _mainCam;

    //float _beforeHP, _beforeMaxHP, _beforeShield;

    public void Init(Unit unit)
    {
        _unit = unit;
        if (_unit.MaxCoolTime != 0)
        {
            _coolTimeBar.gameObject.SetActive(true);
        }
        else
        {
            _coolTimeBar.gameObject.SetActive(false);
        }
        _mainCam = Camera.main;
        _hpBarWidth = _hPBar.GetComponent<RectTransform>().rect.width;
        _barRectY = _shieldBarRect.sizeDelta.y;
        Refresh();
    }

    private void Update()
    {
        transform.position = _mainCam.WorldToScreenPoint(_unit.transform.position + (Vector3.down * 0.5f));
        
        Refresh();
    }

    void Refresh()
    {
        float shieldRectX = 0;
        if (_coolTimeBar.gameObject.activeInHierarchy)
        {
            _coolTimeBar.fillAmount = 1f - (_unit.CoolTime / _unit.MaxCoolTime);
        }
        if (_unit.HP + _unit.Shield > _unit.MaxHP)
        {
            _hpTargetFill = _unit.HP / (_unit.HP + _unit.Shield);
            _hPBar.fillAmount = _hpTargetFill;//Mathf.Lerp(_hPBar.fillAmount, _hpTargetFill, Time.deltaTime * 10f);

            _shieldBarRect.anchoredPosition = new Vector2(_hPBar.fillAmount * _hpBarWidth, _shieldBarRect.anchoredPosition.y);
            _shieldBarRect.sizeDelta = new Vector2(_hpBarWidth - (_hPBar.fillAmount * _hpBarWidth), _barRectY);
            _shieldTargetFill = 1f;
            _shieldBar.fillAmount = _shieldTargetFill;//Mathf.Lerp(_shieldBar.fillAmount, _shieldTargetFill, Time.deltaTime * 10f);
        }
        else if (_unit.Shield != 0)
        {

            _hpTargetFill = _unit.HP / _unit.MaxHP;
            _hPBar.fillAmount = _hpTargetFill;// Mathf.Lerp(_hPBar.fillAmount, _hpTargetFill, Time.deltaTime * 10f);
            shieldRectX = _hPBar.fillAmount * _hpBarWidth;
            _shieldBarRect.anchoredPosition = new Vector2(shieldRectX, _shieldBarRect.anchoredPosition.y);
            _shieldBarRect.sizeDelta = new Vector2(_hpBarWidth - (shieldRectX), _barRectY);
            _shieldTargetFill = _unit.Shield / (_unit.MaxHP - _unit.HP);
            _shieldBar.fillAmount = _shieldTargetFill;// Mathf.Lerp(_shieldBar.fillAmount, _shieldTargetFill, Time.deltaTime * 10f);
        }
        else
        {
            _shieldBar.fillAmount = 0f;
            _hpTargetFill = _unit.HP / _unit.MaxHP;
            _hPBar.fillAmount = _hpTargetFill;//Mathf.Lerp(_hPBar.fillAmount, _hpTargetFill, Time.deltaTime * 10f);
        }
        //_beforeHP = _unit.HP;
        //_beforeMaxHP = _unit.MaxHP;
        //_beforeShield = _unit.Shield;
    }

}
