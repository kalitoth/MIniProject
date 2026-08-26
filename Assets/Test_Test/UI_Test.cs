using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class UI_Test : MonoBehaviour
{
 
    [SerializeField]
    TMP_Text _Text;
    [SerializeField]
    Slider _Slider; 
    void Start()
    {
 
    }
     
    void Update()
    {
         
        if (Input.GetKeyDown(KeyCode.G))
        {
            _Text.text = "10";
        }
        
        if(Input.GetKeyDown(KeyCode.S))
        {
            _Slider.value -= 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            _Slider.value += 0.1f;
        }
         

       
    }
}
