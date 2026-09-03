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
    private Transform _playertransform;

  // private Player_Test _player;
  // [Header("무빙시프트")]
  // [SerializeField]
  //  PlayerShift _movingShift;

    [Header("레이")]
    [SerializeField]
    private Camera _camera;
    Ray _ray;
    LayerMask _layerMask;
    private RaycastHit _hit;
    float _rayMaxDistance = 500f;

    
    Vector3 _projectionRay;
    Vector3 _projectionPlayer;
    Vector3 move;
    float gravity = -9.81f;

    public Vector3 _rayHitPoint; 

    public Vector3 RayHitPoint
    {
        get { return _rayHitPoint; }
        set { _rayHitPoint = value; }
    }
 
    public CharacterController CharacterController => _characterController;
   public Animator Animator => _animator;
    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster");

        //_player = GetComponent<Player_Test>();
        _playertransform = gameObject.GetComponent<Transform>();

        _animator = gameObject.GetComponent<Animator>();
        _characterController = gameObject.GetComponent<CharacterController>();

        if (_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");

        }
        if (_playertransform == null)
        {
            Debug.Log("무빙에 현재 플레이어가 없다");
        }

        _rayHitPoint = _playertransform.position;

        enabled = false;
    }
 

    public void Moving()
    { 
        if (_animator == null || _characterController == null)
        {
            Debug.Log("무빙에 애니메이터, 컨트롤러가 없다");
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                //이동은 하지만 새로운 ray가 들어가지 않음
                if (!enabled)
                {
                    return;
                }
                
                RayCamTo(out _hit, _layerMask);

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
                          _playertransform.rotation = Quaternion.LookRotation((_rayHitPoint - _playertransform.position).normalized, Vector3.up);
                               
                     
                      }
                 }
            }

        }
        
        _projectionRay = Vector3.ProjectOnPlane(_rayHitPoint, Vector3.up);
       _projectionPlayer = Vector3.ProjectOnPlane(_playertransform.position, Vector3.up);
       move = _projectionRay - _projectionPlayer;
       move.y = gravity;
         
       _characterController.Move(move * Time.deltaTime);
         
        //y가 다르게 생성되면 뛰면서 생성
        _animator.SetFloat("FMoving", (_rayHitPoint - _playertransform.position).magnitude);
        
    }
    public void RayCamTo(out RaycastHit hit, LayerMask layerMask)
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out hit, _rayMaxDistance, layerMask);

    }

}
