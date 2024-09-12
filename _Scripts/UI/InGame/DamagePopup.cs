using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    GameObject _criIcon;
    SpriteRenderer _criIconSprite;
    [SerializeField]
    TextMeshPro _text;

    List<Tweener> _tweens = new List<Tweener>();

    public void Init(Vector2 pos, string text, DamagePopupType type)
    {
        transform.position = new Vector3(pos.x + Random.Range(-0.2f,0.2f), pos.y + 2f + Random.Range(-0.1f, 0.1f), 0);
        _text.text = text;
        switch (type)
        {
            case DamagePopupType.Normal:
                {
                    if (_criIcon.activeSelf)
                    {
                        _criIcon.SetActive(false);
                    }
                }
                break;
            case DamagePopupType.Critical:
                {
                    if (!_criIcon.activeSelf)
                    {
                        _criIcon.SetActive(true);
                    }
                    float offsetX = _text.GetPreferredValues(text).x * 0.5f;
                    _criIcon.transform.localPosition = (Vector3.left * offsetX) + Vector3.left;
                }
                break;
            case DamagePopupType.Heal:
                {
                    if (_criIcon.activeSelf)
                    {
                        _criIcon.SetActive(false);
                    }
                }
                break;
            case DamagePopupType.Stuck:
                {
                    if (_criIcon.activeSelf)
                    {
                        _criIcon.SetActive(false);
                    }
                }
                break;
        }
        gameObject.SetActive(true);
        SetTween(type);
    }

    void SetTween(DamagePopupType type)
    {
        _tweens.Clear();

        switch (type)
        {
            case DamagePopupType.Normal:
                {
                    Tweener positionTween = DOTween.To(() => transform.position.y, y => transform.position = new Vector3(transform.position.x, y, transform.position.z), transform.position.y + 3f, 2f).SetEase(Ease.OutQuad);
                    _tweens.Add(positionTween);
                    Tweener scaleTween = DOTween.To(() => 0f, x => transform.localScale = new Vector3(x, x, x), 1f, 2f).SetEase(Ease.OutElastic);
                    _tweens.Add(scaleTween);
                    Tweener fadeInTween = DOTween.To(() => 0f, x => _text.alpha = x, 1f, 0.3f).OnComplete(() =>
                    {
                        DOTween.Sequence()
                                .AppendInterval(0.7f)
                                .Append(DOTween.To(() => 1f, x => _text.alpha = x, 0f, 1f));

                    });

                    positionTween.onComplete = () => gameObject.SetActive(false);
                    _tweens.Add(fadeInTween);
                }
                break;
            case DamagePopupType.Critical:
                {

                    Tweener positionTween = DOTween.To(() => transform.position.y, y => transform.position = new Vector3(transform.position.x, y, transform.position.z), transform.position.y + 3f, 2f).SetEase(Ease.OutQuad);
                    _tweens.Add(positionTween);
                    Tweener scaleTween = DOTween.To(() => 0f, x => transform.localScale = new Vector3(x, x, x), 1f, 2f).SetEase(Ease.OutElastic);
                    _tweens.Add(scaleTween);
                    Tweener fadeInTween = DOTween.To(() => 0f, x => _text.alpha = x, 1f, 0.3f).OnComplete(() =>
                    {
                        DOTween.Sequence()
                                .AppendInterval(0.7f)
                                .Append(DOTween.To(() => 1f, x => _text.alpha = x, 0f, 1f));

                    });
                    if (!_criIconSprite) _criIconSprite = _criIcon.GetComponent<SpriteRenderer>();
                    _criIconSprite.color = Color.clear;
                    Tweener fadeInTween2 = _criIconSprite.DOColor(Color.white, 0.3f);
                    fadeInTween2.onComplete = (() =>
                    {
                        DOTween.Sequence()
                                .AppendInterval(0.7f)
                                .Append(_criIconSprite.DOColor(Color.clear, 1f));
                    });
                    _tweens.Add(fadeInTween);
                    positionTween.onComplete = () => gameObject.SetActive(false);
                }
                break;
        }
    }

    private void OnDisable()
    {
        foreach (Tweener t in _tweens)
        {
            t.Kill();
        }
    }

}
