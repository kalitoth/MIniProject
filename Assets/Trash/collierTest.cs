using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collierTest : MonoBehaviour
{

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("트리거 안에 있다");
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("콜라이더 안에 있다");
    }
}
