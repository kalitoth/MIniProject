using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; 
public class Camera_test : MonoBehaviour
{

    //플레이어가 계속 바뀔 수 있어야 한다?
    //몬스터도 이 카메라 써야함
    //나중에 전투매니저에서 전투 목록을 리스트 같은걸로 관리하면 바꾸기
    
    private Player_Test _curruntPlayer;
    [SerializeField]
    PlayerShift _playerMovingShift;
    //[SerializeField]
    //Ray_UI _ray_Test;
      
    [Header("캠 이동속도")]
    [SerializeField]
    private float _sharpnessPos = 16;
    private float _sharpnessRot = 2f;
    private float _interpolePos;
    private float _interpoleRot;
    //이거 캠 스피드 옵션으로 뺄 수 있도록 
    private float _camSpeed = 10;
    private float _camWheelSpeed = 240;

    
    //내부
    RaycastHit _hit;
    
    Vector3 offset = new Vector3(0,20,-10);
    Vector3 mouseWheel = Vector3.zero;
    Quaternion camToSomething;


    //bool active = true;
    //float a;

    CamState camState = CamState.None;
    enum CamState
    {
        None, 
        Free
    }

    void Start()
    { 
       //if (_ray_Test == null)
       //{
       //    Debug.Log("카메라에 레이 인스펙터가 없다");
       //}

        _curruntPlayer = _playerMovingShift.Player;
    }
 

    private void LateUpdate()
    {
        _interpolePos = 1 - Mathf.Exp(-_sharpnessPos * Time.deltaTime);
        _interpoleRot = 1 - Mathf.Exp(-_sharpnessRot * Time.deltaTime);
       
        if (!_curruntPlayer.UnitState.HasFlag(Unit_Test.State.Skill))
        {
           

            //레이 정보
            // if (!EventSystem.current.IsPointerOverGameObject())
            {
                //if (Input.GetMouseButtonDown(0))
                {
                    _curruntPlayer = _playerMovingShift.Player;
                }

               
            }
        }
       //if (!_curruntPlayer.UnitState.HasFlag(Unit_Test.State.Skill))
       //{
       //    if (Input.GetMouseButton(1))
       //    {
       //        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       //
       //        float ray = Input.mousePosition.x;
       //        if (active)
       //        {
       //            Debug.Log("들어오지?11");
       //            a = ray;
       //            active = false;
       //        }
       //        
       //
       //        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ray.direction), _interpoleRot);
       //        //transform.Rotate(Vector3.up);
       //        //ray.direction
       //        //offset.z += 1f;
       //        if(a > ray)
       //        {
       //            transform.rotation = Quaternion.AngleAxis(-1f, Vector3.up) * transform.rotation;
       //            Debug.Log("들어오지?22");
       //        }
       //        else if(a < ray)
       //        {
       //            transform.rotation = Quaternion.AngleAxis(1f, Vector3.up) * transform.rotation;
       //            Debug.Log("들어오지?2233");
       //        }
       //
       //
       //        
       //    }
       //    if (Input.GetMouseButtonUp(1))
       //    {
       //        Debug.Log("들어오지?33");
       //        active = true;
       //    }
       //}
           

        //자유 이동
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            camState = CamState.Free;
        }

        if(Input.GetKey(KeyCode.W))
        {
            //transform.Translate(Vector3.forward * _camSpeed * Time.deltaTime);
            transform.position += Vector3.forward* _camSpeed *Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            //transform.Translate(Vector3.back * _camSpeed * Time.deltaTime);
            transform.position += Vector3.back * _camSpeed * Time.deltaTime;
        }
        if( Input.GetKey(KeyCode.A))
        {
            //transform.Translate(Vector3.left * _camSpeed * Time.deltaTime);
            transform.position += Vector3.left * _camSpeed * Time.deltaTime;
        }
        if( Input.GetKey (KeyCode.D))
        {
            //transform.Translate(Vector3.right * _camSpeed * Time.deltaTime);
           transform.position += Vector3.right * _camSpeed * Time.deltaTime;
        }

        //휠 
        mouseWheel =  Vector3.down * _camWheelSpeed * Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime;
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            transform.position += mouseWheel;
        }
        
         
        //임시로 사용
        if (Input.GetKeyDown (KeyCode.Tab))
        {
            camState = CamState.None;
        }
        //스킬을 사용했을 때 free상태라면 계속 free
        //아니면 스킬상태로
        //스킬이 끝나면 None상태로
        // free > None 어떻게?
 
   
        

        
        if (_curruntPlayer.UnitState.HasFlag(Unit_Test.State.Skill) && camState != CamState.Free)
        {
            
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                  _hit = _curruntPlayer.Hit;
              if(_curruntPlayer.Hit.transform == null)
              { 
                    return;
              }
            } 
            transform.position = Vector3.Lerp(transform.position, (_curruntPlayer.transform.position + _hit.point) * 0.5f + offset, _interpolePos);
            camToSomething = Quaternion.LookRotation((_curruntPlayer.transform.position + _hit.point) * 0.5f - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, camToSomething, _interpoleRot);
                
        }
        else if (camState == CamState.None)
        {
            
            _hit.point = _curruntPlayer.transform.position;
            camToSomething = Quaternion.LookRotation(_curruntPlayer.transform.position - transform.position);
            transform.position = Vector3.Lerp(transform.position, _curruntPlayer.transform.position + offset, _interpolePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, camToSomething, _interpoleRot);
        }


    }



}
