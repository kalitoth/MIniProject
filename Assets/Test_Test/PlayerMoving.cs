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
    private Transform _currentPlayer;

    //레이
    [SerializeField]
    private Ray_Test _ray_test; 
    private RaycastHit _hit;
    
    //반올림
    Vector3Int vector3Round;

   public Animator Animator => _animator;
         


    void Start()
    {
         
        if (_ray_test == null)
        {
            
            Debug.Log("무빙에 레이가 없다");
        }

        if(_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");

        }
        _currentPlayer = GetComponent<Transform>();

        Vector3 projection = new Vector3(_currentPlayer.position.x, 0f, _currentPlayer.position.z);
        vector3Round = Vector3Int.RoundToInt(projection);
    }

    public void Moving()
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
                    if (_hit.collider.gameObject.CompareTag("Ground"))
                    {


                        vector3Round = Vector3Int.RoundToInt(_hit.point); // _hit.point.x y z 로 새로운 벡터를 만들어서 고정 가능 꼭 RoundToInt를 쓰지 않아도 된다

                        _currentPlayer.rotation = Quaternion.LookRotation((vector3Round - _currentPlayer.position).normalized, Vector3.up);


                        _ray_test.RayVisual();
                    }
                    //이동 플레이어 바꾸기
                    if(_hit.collider.gameObject.CompareTag("Player"))
                    {
                        _animator = null;
                        _characterController = null;
                        _currentPlayer = null;

                        _currentPlayer = _hit.collider.gameObject.GetComponent<Transform>();
                        _animator = _hit.collider.gameObject.GetComponent<Animator>();
                        _characterController = _hit.collider.gameObject.GetComponent<CharacterController>();
                    }    
                }

            }
        }

        _characterController.Move((vector3Round - _currentPlayer.position) * Time.deltaTime);
        
        _animator.SetFloat("FMoving", (vector3Round - _currentPlayer.position).magnitude);
        
    }

   
}
