using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Local,
    Dev,
    Live
}
public class FrameWork : Singleton<FrameWork>
{
    [SerializeField] GameMode _gameMode;
    public GameMode GameMode { get { return _gameMode; } }


    Network _network;
    public Network Network { get { return _network; } }

    Scene _scene;
    public Scene Scene { get { return _scene; } }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitSystems();
    }

    void InitSystems()
    {
        _network = Network.Instance;
        _network.transform.parent = transform;
        _scene = GetComponentInChildren<Scene>();
        Title.Instance.Init();
    }
}
