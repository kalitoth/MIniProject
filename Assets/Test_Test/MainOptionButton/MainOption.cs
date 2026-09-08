using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainOption : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _canvasGroup;
    AsyncOperation sceneManager;


    float _time = 0;
    float _duration = 0.5f;

    float _interpolate;


 public void Resume()
 {
     Time.timeScale = 1;
    
     transform.parent.gameObject.SetActive(false);

 }


    public void OptionScene()
    {
        StartCoroutine(IntroFadeout("Option"));
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void firstMenu()
    {
        StartCoroutine(IntroFadeout("Intro"));
    }


    private IEnumerator IntroFadeout(string SceneName)
    {
        //프레임 따라 가는 게 아니라 시간 따라 가야 한다

        _canvasGroup.alpha = 0f;
        //_time = Time.realtimeSinceStartup;
        _canvasGroup.blocksRaycasts = true;
        
        while (_time < _duration)
        {

            _time += Time.unscaledDeltaTime;

            _interpolate = Mathf.Clamp01(_time / _duration);

            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, _interpolate);
            yield return null;

            Debug.Log($"들어오고 있나?");
            Debug.Log($"시간 : {_time}");
            Debug.Log($"듀레이션 : {_duration}");
            Debug.Log($"인터폴레이트 : {_interpolate}");

        }

        //sceneManager = 
            SceneManager.LoadSceneAsync(SceneName);

        _time = 0f;

        Time.timeScale = 1;
        //sceneManager.allowSceneActivation = false;


    }
}
