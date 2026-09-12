using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster_Test : Unit_Test
{
    //시야 직렬화
    [SerializeField]
    GameObject _monsterSight;
    //배틀 오브젝트
    [SerializeField]
    public GameObject _BattleColloseum;

    //플레이어 인벤토리와 각자 아이템 버튼
    [SerializeField]
    protected ShareRepository _playerInventory;

    //몬스터 시야와 전투
    GameObject _instSight;
    protected GameObject _BattleColosseum;

    protected MonsterSight _getSight;

    //몬스터 이동
    protected NavMeshAgent _agent;

    //protected Vector3 _firstPlayer;


    public Vector3 _initialPosition;
    public Quaternion _initialRotation;
    public bool oneDestination;

    //죽었을 때 몬스터가 주는 것 + 각자 아이템
    [Header("몬스터가 주는 것")]
    protected int _monsterGold;
    protected int _monsterExp;

    //배틀구체가 있는가?
    Collider[] _colliders = new Collider[1];
    float _radius = 0.001f;


    LayerMask _Battlelayer;


    float _time = 0;
    Vector3 _positionA;
    Vector3 _positionB;
    bool _positionSwitch = true;

    protected override void Awake()
    {
        base.Awake();
        //_audioSource = GetComponent<AudioSource>();
        _agent = GetComponent<NavMeshAgent>();

        _Battlelayer = 1 << LayerMask.NameToLayer("Battle");
    }
    void Start()
    { 
        
        _instSight = Instantiate(_monsterSight, this.transform);
        Debug.Log("_monsterSight를 생성");

        _getSight = _instSight.gameObject.GetComponent<MonsterSight>();
        //_characterController = GetComponent<CharacterController>();
         
        if (_getSight == null)
        {
            Debug.Log("_getSight가 null");
        }
        if (_getSight._player == null)
        {
            Debug.Log("_getSight._playerList가 null");
        }

        _initialRotation = transform.rotation;
        _initialPosition = transform.position;
    }

    
    protected virtual void Update()
    {
        if(_getSight._player == null)
        {
            Debug.Log("_getSight._playerList가 null");
            return;
        }

        
      if (UnitState == State.None)
      {
           if(oneDestination)
           {
                _agent.isStopped = false;
                Debug.Log("실행중?");
               _agent.SetDestination(_initialPosition);
               oneDestination = false;
           }

            if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
            {
                    Debug.Log($"무빙중이 맞나?");
                 
                    if (_positionSwitch)
                    {
                        _positionA = transform.position;
                        _positionSwitch = !_positionSwitch;
                    }
                    else
                    {
                        _positionB = transform.position;
                        _positionSwitch = !_positionSwitch;
                    }

                    Debug.Log($"포지션 크기 A - B : {(_positionA - _positionB).sqrMagnitude}");
                    Debug.Log($"포지션 크기 A : {_positionA}");
                    Debug.Log($"포지션 크기 B : {_positionB}");

                    if ((_positionA - _positionB).sqrMagnitude < 0.1f)
                    {
                        _time += Time.deltaTime;

                        if (_time > 3)
                        {
                            _initialPosition = transform.position;
                            Debug.Log("몬스터가 멈춘다");
                            _time = 0;
                        }

                    }
                
                 
                
            }

             

            Animator.SetFloat("FMoving", (_initialPosition - transform.position).magnitude);

            Debug.Log($"스켈레톤 소환되고 나서 들어온다 : {(_initialPosition - transform.position).magnitude}");

            if ((_initialPosition - transform.position).sqrMagnitude < 0.5f)
           {
               transform.rotation = _initialRotation;
           }
           
            if (_getSight._player.Count > 0)
            {
                Debug.Log($"몬스터 시야에 플레이어 리스트 숫자 {_getSight._player.Count}");
                
                Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _Battlelayer);
                for(int i = 0; i< _colliders.Length; i++)
                {
                    if (_colliders[i] == null)
                    {
                        Debug.Log("배틀구 생성");
                        _BattleColosseum = Instantiate(_BattleColloseum, transform.position, transform.rotation);
                        _BattleColosseum.SetActive(true);
                        UnitState = State.Battle;
                    }
                    
                }

                

            }
      }
          
        Die();
         
    }

    //스킬에 맞으면 배틀 시작
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Skill"))
        {
            if(!UnitState.HasFlag(State.Battle))
            {
                _BattleColosseum = Instantiate(_BattleColloseum, transform.position, transform.rotation);
                _BattleColosseum.SetActive(true);
            }
        }
    }
 
}
