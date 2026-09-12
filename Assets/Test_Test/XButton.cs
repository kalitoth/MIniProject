using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XButton : MonoBehaviour
{

   public void ActiveFalseParents()
    { 
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
         
    }
}
