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
    [Header("목표물(플레이어,오브젝트)")]
    [SerializeField]
    private Transform _curruntPlayer;
    [SerializeField]
    Ray_Test _ray_Test;
      
    [Header("캠 이동속도")]
    [SerializeField]
    private float _sharpness = 10;
    private float _interpole;
    //이거 캠 스피드 옵션으로 뺄 수 있도록 
    private float _camSpeed = 3;
    private float _camWheelSpeed = 60;
    
    //내부
    RaycastHit _hit;
    Vector3 offset = new Vector3(0,10,-5);
    Vector3 mouseWheel = Vector3.zero;
    Quaternion camToSomething;
     
    CamState camState = CamState.None;
    enum CamState
    {
        None,
        Skill,
        Free,
    }

    void Start()
    {
        _interpole = 1 - Mathf.Exp(-_sharpness);

         
        if (_ray_Test == null)
        {
            Debug.Log("카메라에 레이 인스펙터가 없다");
        }
    }

 
    private void LateUpdate()
    {


        //레이 정보
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_ray_Test._hit.collider != null)
                {
                    //플레이어 바꾸기
                    if (_ray_Test._hit.collider.gameObject.CompareTag("Player"))
                    {

                        _curruntPlayer = null;

                        _curruntPlayer = _ray_Test._hit.collider.gameObject.GetComponent<Transform>();

                    }
                }

            }
        }
        

        //자유 이동
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            camState = CamState.Free;
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward* _camSpeed *Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * _camSpeed * Time.deltaTime;
        }
        if( Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * _camSpeed * Time.deltaTime;
        }
        if( Input.GetKey (KeyCode.D))
        {
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
         

        if(camState == CamState.None)
        {
            camToSomething = Quaternion.LookRotation(_curruntPlayer.position - transform.position);
            transform.position = Vector3.Lerp(transform.position, _curruntPlayer.position + offset, _interpole);
            transform.rotation = Quaternion.Slerp(transform.rotation, camToSomething, _interpole);
        }
        else if(camState == CamState.Skill)
        {
            //플레이어가 스킬을 썼을 때
            //플레이어가 스킬을 썼다는 것이 필요
            //플레이어의 레이 캐스트가 히트 한 몬스터의 좌표가 필요
            //지면이 아니라 몬스터를 맞추었을 때의 좌표가 필요
            transform.position = Vector3.Lerp(transform.position, (_curruntPlayer.position + _hit.transform.position) * 0.5f + offset, _interpole);
            camToSomething = Quaternion.LookRotation((_curruntPlayer.position+_hit.transform.position)*0.5f - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, camToSomething, _interpole);
        }
        
    }
}
