using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public enum eRequestType
{
    APIData,
    TextTable,
    UnitDataTable,
    UnitPassiveData,
    UnitActiveData,
    Max
}
public enum eDataType
{
    Int,
    Float,
    String
}
public enum eSendType
{
    Get,
    Put,
    Post,
    DownLoad,
}
[Serializable]
public class WWWData
{
    public eSendType sendType;
    public HeaderData[] header;
    public BodyData[] body;
    public string url;
}
[Serializable]
public class HeaderData
{
    public string key;
    public string value;
}
[Serializable]
public class BodyData
{
    public eDataType type;
    public string key;
    public string value;
}

[Serializable]
public class Request
{
    public eRequestType type;
    public Action<string> callback;
    public string addURL = "";
    public string addBody = null;

    public Request(eRequestType _type, Action<string> _callback, string _addURL = "", string _addBody = null)
    {
        type = _type;
        callback = _callback;
        addURL = _addURL;
        addBody = _addBody;
    }
}

public class Network : Singleton<Network>
{

    public delegate void OnNetWorkFinishedHandler(eRequestType type);
    public static event OnNetWorkFinishedHandler OnNetWorkFinished;

    public bool _isNetworking = false;
    public bool _isWorking = false;
    public eRequestType _currentRequest;

    Dictionary<eRequestType, WWWData> _dicRequest = new Dictionary<eRequestType, WWWData>();
    Dictionary<GameMode, string> _dicApiUrl = new Dictionary<GameMode, string>() 
    { 
        { GameMode.Local, "LocalData/APIKeys" }, 
        { GameMode.Dev, "" }, 
        { GameMode.Live, "" } 
    };

    Queue<Request> _currentRequests = new Queue<Request>();

    public void Init(Action<string> callback)
    {
        _currentRequests.Clear();
        _dicRequest.Clear();

        if (FrameWork.Instance.GameMode != GameMode.Local)
        {
            StartCoroutine(Request());
        }

        WWWData data = new WWWData();
        data.sendType = eSendType.DownLoad;
        data.url = _dicApiUrl[FrameWork.Instance.GameMode];
        _dicRequest.Add(eRequestType.APIData, data);

        Send(eRequestType.APIData, (s) =>
        {
            SetRequestDictionary(s);
            callback.Invoke(s);
         });
    }

    void SetRequestDictionary(string str)
    {
        var json = JSON.Parse(str);
        foreach (var key in json.Keys)
        {
            eRequestType type = eRequestType.Max;
            if (Enum.TryParse(key, true, out type))
            {
                WWWData data = new WWWData();
                if (json[key].Value.Contains(".csv"))
                {
                    data.sendType = eSendType.DownLoad;
                }
                else
                {
                    data.sendType = eSendType.Post;
                }
                data.url = json[key].Value;
                _dicRequest.Add(type, data);
            }
            else
            {
                Debug.Log($"api key {key} is not found in Client.");
                continue;
            }
        }
    }


    public void Send(eRequestType type, Action<string> callback, string addURL = "", string addBody = null)
    {
        if (FrameWork.Instance.GameMode == GameMode.Local)
        {
            if (_dicRequest[type].sendType == eSendType.DownLoad)
            {
                LocalRequest(_dicRequest[type].url + addURL, callback);
            }
        }
        else
        {
            Request request = new Request(type, callback, addURL, addBody);
            _currentRequests.Enqueue(request);
        }
    }
    public void LocalRequest(string url, Action<string> callback)
    {
        // Resources.Load를 사용하여 로컬 리소스 로드
        string text = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, url));

        string result = text;
        callback(result);

    }

    IEnumerator Request()
    {
        while (true)
        {
            yield return new WaitUntil(() => _currentRequests.Count > 0);
            _isWorking = true;

            Request request = _currentRequests.Dequeue();

            float startTime = Time.time;
            Debug.Log("통신 처리 시작 : " + request.type.ToString());

            string body = request.addBody;
            request.addURL = UnityWebRequest.EscapeURL(request.addURL);
            string url = _dicRequest[request.type].url + request.addURL;

            UnityWebRequest www = CreateUnityWebRequest(request.type, body, url);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                string result = www.downloadHandler.text;
                request.callback(result.ToString());
            }
            else
            {
                Debug.LogError(www.error);
            }

            www.Dispose();

            if (OnNetWorkFinished != null) OnNetWorkFinished.Invoke(request.type);
            Debug.Log($"통신 처리 완료 :  + {request.type}, 소요 시간 : {Time.time - startTime} ");

            _isWorking = false;
        }
    }

    private UnityWebRequest CreateUnityWebRequest(eRequestType type, string body, string url)
    {
        UnityWebRequest www;

        switch (_dicRequest[type].sendType)
        {
            case eSendType.Get:
                www = UnityWebRequest.Get(url);
                break;
            case eSendType.Put:
                www = UnityWebRequest.Put(url, body);
                break;
            case eSendType.Post:
                if (body != null)
                {
                    www = UnityWebRequest.Put(url, body);
                    www.method = "POST";
                    www.SetRequestHeader("Content-Type", "application/json");
                }
                else
                {
                    www = UnityWebRequest.Put(url, "{}");
                    www.method = "POST";
                    www.SetRequestHeader("Content-Type", "application/json");
                }
                break;
            case eSendType.DownLoad:
                www = UnityWebRequestAssetBundle.GetAssetBundle(url);
                break;
            default:
                www = UnityWebRequest.Get(url);
                break;
        }

        return www;
    }
}
