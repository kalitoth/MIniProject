using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
     
    Transform _unit; 
    float _forward = 7.5f;
    float _up = 0.7f;

    private void Start()
    {
        _unit = transform.parent.GetComponent<Transform>();
    }
    private void Update()
    {
        transform.position = _unit.position + _unit.rotation*(Vector3.forward* _forward + Vector3.up* _up);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {

        }
    }
}
