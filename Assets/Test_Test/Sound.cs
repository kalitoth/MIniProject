using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;
using UnityEngine.UI;
public class Sound : MonoBehaviour
{
   [SerializeField]
   AudioMixer audioMixer;
  
    [SerializeField]
    Slider _masterSlider;
    [SerializeField]
    Slider _BGMSlider;
    [SerializeField]
    Slider _effectSlider;
    float _masterVolume = 0.5f;
    float _BGMVolume = 0.5f;
    float _effectVolume = 0.5f;
    string _master = "Master";
    string _BGM = "BGM";
    string _effect = "Effect";
 
    void Update()
    { 
        _masterVolume = Mathf.Clamp(_masterSlider.value, 0.0001f,2f);
        _BGMVolume = Mathf.Clamp(_BGMSlider.value, 0.0001f,2f);
        _effectVolume = Mathf.Clamp(_effectSlider.value, 0.0001f,2f);

        audioMixer.SetFloat(_master, Mathf.Log10(_masterVolume) * 20);
        audioMixer.SetFloat(_BGM, Mathf.Log10(_BGMVolume) * 20);
        audioMixer.SetFloat(_effect, Mathf.Log10(_effectVolume) * 20);

         
    }
}
