using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMoving : MonoBehaviour
{
    //맨 처음 캐릭터는 인스펙터로
    
    private Animator _animator;
    private CharacterController _characterController;
    private Transform _currentPlayer;
    [SerializeField]
    PlayerMovingShift _movingShift;
     
    private RaycastHit _hit;

    public RaycastHit Hit
    {
        get { return _hit; }
        set { _hit = value; }
    }

    Vector3 _projectionRay;
    Vector3 _projectionPlayer;
    Vector3 move;
    float gravity = -9.81f;

    Vector3 _rayHitPoint;

    bool _rotation = false;

   public Animator Animator => _animator;
         

    void Start()
    { 
         _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _currentPlayer = this.GetComponent<Transform>();
 
        if (_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");

        } 
        if(_currentPlayer == null)
        {
            Debug.Log("무빙에 현재 플레이어가 없다");
        }
         
        _rayHitPoint = _currentPlayer.position;
        
    }

    public void Moving()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _movingShift.MovingShift();
        }
            

        if (_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("무빙먼저?");
                Debug.Log($"{_hit.collider}");

            

            if (_hit.collider != null)
            {
                    Debug.Log($"콜라이더 밑");
                 if (_hit.collider.gameObject.CompareTag("Ground"))
                 { 
                        //이전의 hit와 같다면 return
                    if(_hit.point == _rayHitPoint)
                    {
                        return;
                    }
                     _rayHitPoint = _hit.point;
                
                     Debug.Log($"무빙{_hit.point}");
                     _currentPlayer.rotation = Quaternion.LookRotation((_rayHitPoint - _currentPlayer.position).normalized, Vector3.up);
                          
                
                 }
            }
            }

        }
        

        _projectionRay = Vector3.ProjectOnPlane(_rayHitPoint, Vector3.up);
       _projectionPlayer = Vector3.ProjectOnPlane(_currentPlayer.position, Vector3.up);
       move = _projectionRay - _projectionPlayer;
       move.y = gravity;
         
       _characterController.Move(move * Time.deltaTime);
           
       _animator.SetFloat("FMoving", (_rayHitPoint - _currentPlayer.position).magnitude);
          
    }

 
}
