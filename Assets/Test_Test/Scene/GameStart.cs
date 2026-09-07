using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _canvasGroup;
    float _time;
    float _interpolate;
    float _duration = 0.5f;

    AsyncOperation sceneManager;
    private void Awake()
    {
        
    }

    public void GameStartFuc()
    {
        StartCoroutine(Fadeout());
    }

    private IEnumerator Fadeout()
    {
        //프레임 따라 가는 게 아니라 시간 따라 가야 한다

        _canvasGroup.alpha = 0f;
        //_time = Time.realtimeSinceStartup;
        _canvasGroup.blocksRaycasts = true;

        while (_time < _duration)
        {

            _time += Time.deltaTime;

            _interpolate = Mathf.Clamp01( _time / _duration);

            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, _interpolate);
            yield return null;

        }

        sceneManager = SceneManager.LoadSceneAsync("MainScene");

        _time = 0f;
        //sceneManager.allowSceneActivation = false;


    }
 
}
