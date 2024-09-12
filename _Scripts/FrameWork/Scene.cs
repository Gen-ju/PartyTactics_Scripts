using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Main,
}
public class Scene : MonoBehaviour
{
    public GameObject _fadeIn;
    public GameObject _fadeOut;
    public delegate void OnSceneChangedHandler(SceneType type);
    public static event OnSceneChangedHandler OnSceneChanged;
    bool _isLoading = false;
    public void LoadScene(SceneType sceneName, bool async)
    {
        _fadeIn.SetActive(false);
        _fadeOut.SetActive(false);
        if (_isLoading) return;
        _isLoading = true;
        if (async)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Single);
            _isLoading = false;
            OnSceneChanged?.Invoke(sceneName);
        }
    }


    private IEnumerator LoadSceneAsync(SceneType sceneName)
    {
        _fadeIn.SetActive(true);
        _fadeOut.SetActive(false);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        _isLoading = true;
        yield return new WaitForSecondsRealtime(1f);
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9는 로드의 90%를 나타냄
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            Debug.Log("비동기 씬 로딩 중... " + (progress * 100) + "%");
            yield return null;
        }
        _isLoading = false;
        OnSceneChanged?.Invoke(sceneName);
        _fadeIn.SetActive(false);
        _fadeOut.SetActive(true);
    }
}
