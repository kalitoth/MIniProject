using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlKey1 : MonoBehaviour
{
    [SerializeField]
    Image _button;
 

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _button.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
