using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneFadeIn : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _canvasGroup;

    float _time;

    float _duration = 0.5f;

    float _interpolate;
    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        //프레임 따라 가는 게 아니라 시간 따라 가야 한다

        _canvasGroup.alpha = 1f;

        _canvasGroup.blocksRaycasts = false;

        yield return null;

        while (_time < _duration)
        {

            _time += Time.unscaledDeltaTime;

            _interpolate = Mathf.Clamp01(_time / _duration);

            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, _interpolate);
            yield return null;

        }

        _time = 0f;

    }
}
