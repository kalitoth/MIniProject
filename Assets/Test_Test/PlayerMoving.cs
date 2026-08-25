using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMoving : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;
     
    private RayVisulizer _visulizer;

    Vector3Int vector3;

    private int _speed;
    void Start()
    {
        vector3 = Vector3Int.RoundToInt(transform.position);
        _visulizer = GetComponent<RayVisulizer>();
    }

    
    void Update()
    {
         Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
       
         Physics.Raycast(ray,out _hit);

         if(Input.GetMouseButtonDown(0))
         {
            if (_hit.collider != null)
            {
                vector3 = Vector3Int.RoundToInt(_hit.point); // _hit.point.x y z 로 새로운 벡터를 만들어서 고정 가능 꼭 RoundToInt를 쓰지 않아도 된다
                _visulizer.RayVisual(ray, _hit);

                transform.rotation = Quaternion.LookRotation((vector3 - transform.position).normalized, Vector3.up);
                _speed = 9;
            }
            
        }


        if(_speed > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, vector3, 0.1f); 
        }

        _speed--;
    }

   
}
