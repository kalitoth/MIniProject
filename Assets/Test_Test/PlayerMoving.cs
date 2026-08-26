using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMoving : MonoBehaviour
{
    
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CharacterController _characterController;

    //레이
    private Ray_Test _ray_test; 
    private RaycastHit _hit;
    
    //반올림
    Vector3Int vector3Round;

   

    void Start()
    {
        
        _ray_test = GetComponent<Ray_Test>();

        if(_ray_test == null)
        {
            Debug.Log("무빙에 레이가 없다");
        }

        if(_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");

        }
        Vector3 a = new Vector3(transform.position.x, 0f, transform.position.z);
        vector3Round = Vector3Int.RoundToInt(a);
    }

    
    void Update()
    {
        if (_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");
            return;
        }

        //ui 위에서는 반응 안함
        if (!EventSystem.current.IsPointerOverGameObject())
        {

        
            if (Input.GetMouseButtonDown(0))
            {
                _ray_test.RayCamTo(out _hit);

               if (_hit.collider != null)
               {

                   vector3Round = Vector3Int.RoundToInt(_hit.point); // _hit.point.x y z 로 새로운 벡터를 만들어서 고정 가능 꼭 RoundToInt를 쓰지 않아도 된다

                   transform.rotation = Quaternion.LookRotation((vector3Round - transform.position).normalized, Vector3.up);

                  
                   _ray_test.RayVisual();
               }
               
            }
        }
        
        _characterController.Move((vector3Round - transform.position)*Time.deltaTime);
        
        _animator.SetFloat("FMoving", (vector3Round - transform.position).magnitude);

        

    }

   
}
