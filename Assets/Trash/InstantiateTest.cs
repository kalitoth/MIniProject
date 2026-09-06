using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTest : MonoBehaviour
{
    [SerializeField]
    GameObject _game;
   
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            Instantiate(_game,transform.position,transform.rotation);
        }
    }
}
