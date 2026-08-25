using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_test : MonoBehaviour
{
     
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _sharpness = 10;
    private float _interpole;
    
    Vector3 offset = new Vector3(0,10,-5);
    void Start()
    {
        _interpole = 1 - Mathf.Exp(-_sharpness);
    }

    
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Quaternion camToPlayer = Quaternion.LookRotation(_player.position - transform.position);
        transform.position = Vector3.Lerp(transform.position, _player.position+ offset, _interpole);
        transform.rotation = Quaternion.Slerp(transform.rotation, camToPlayer, _interpole);
         
    }
}
