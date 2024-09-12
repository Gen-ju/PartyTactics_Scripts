using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Title : Singleton<Title>
{
    TextMeshProUGUI _text_LoadProcess;

    enum LoadState
    {
        Network = 0,
        Data,
        Login,
        Finish,
        Max
    }

    LoadState _currentState = LoadState.Max;
    public void Init()
    {
        _text_LoadProcess = GameObject.Find("LoadText").GetComponent<TextMeshProUGUI>();

        SetState(LoadState.Network);
    }

    void SetState(LoadState state)
    {
        _currentState = state;
        switch (state)
        {
            case LoadState.Network:
                {
                    _text_LoadProcess.text = "";
                    FrameWork.Instance.Network.Init((s) =>
                    {
                        SetState(LoadState.Data);
                    });
                }
                break;
            case LoadState.Data:
                {
                    _text_LoadProcess.text = "";
                    GameData.Instance.Init(() => 
                    {
                        SetState(LoadState.Finish);
                    });
                }
                break;
            case LoadState.Login:
                {

                }
                break;
            case LoadState.Finish:
                {
                    _text_LoadProcess.text = "";
                    _text_LoadProcess.transform.localScale = Vector3.one * 1.2f;
                    _text_LoadProcess.transform.DOScale(Vector3.one * 1.4f, 1f)
                        .SetEase(Ease.InOutSine)
                        .SetLoops(-1, LoopType.Yoyo);
                }
                break;
        }
    }

    private void Update()
    {
        if (_currentState == LoadState.Finish)
        {
            if (Input.anyKeyDown)
            {
                FrameWork.Instance.Scene.LoadScene(SceneType.Main, true);
            }
        }
    }
}
