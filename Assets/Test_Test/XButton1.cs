using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XButton1 : MonoBehaviour
{
    [SerializeField]
    Image _button;
   public void ActiveFalseParents()
    { 
        _button.gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
         
    }
}
