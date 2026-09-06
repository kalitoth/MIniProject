using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Test : Unit_Test
{
    //시야 직렬화
    [SerializeField]
    GameObject _monsterSight;
    //배틀 오브젝트
    [SerializeField]
    GameObject _BattleSystem;

    //플레이어 인벤토리와 각자 아이템 버튼
    [SerializeField]
    protected ShareRepository _playerInventory; 

    //몬스터 시야와 전투
    GameObject _instSight;
    GameObject _BattleColosseum;

    protected MonsterSight _getSight;

    //몬스터 이동
    protected CharacterController _characterController;
    protected Animator _animator;

     
    protected Vector3 _firstPlayer;


    public Vector3 _initialPosition;

    //죽었을 때 몬스터가 주는 것 + 각자 아이템
    [Header("몬스터가 주는 것")]
    protected int _monsterGold;
    protected int _monsterExp;

    void Start()
    { 
        _instSight = Instantiate(_monsterSight, this.transform);
        Debug.Log("_monsterSight를 생성");

        _getSight = _instSight.gameObject.GetComponent<MonsterSight>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if (_getSight == null)
        {
            Debug.Log("_getSight가 null");
        }
        if (_getSight._playerList == null)
        {
            Debug.Log("_getSight._playerList가 null");
        }


        _initialPosition = transform.position;
    }

    
    protected virtual void Update()
    {
        if(_getSight._playerList == null)
        {
            Debug.Log("_getSight._playerList가 null");
            return;
        }

        
      if (UnitState == State.None)
      {
            if((_initialPosition - transform.position).sqrMagnitude > 0.5f)
            {
                transform.rotation = Quaternion.LookRotation((_initialPosition - transform.position).normalized, Vector3.up);
            }
               
            _characterController.Move((_initialPosition - transform.position)*Time.deltaTime);
            _animator.SetFloat("FMoving", (_initialPosition - transform.position).magnitude);

           
            if (_getSight._playerList.Count > 0)
            {
                for(int i = 0; i < 4 ; i++)
                {
                    if(!_getSight._playerList.ContainsKey(i))
                    {
                        continue;
                    }

                    _firstPlayer = _getSight._playerList[i].transform.position;
                    break;
                }
                
              _BattleColosseum = Instantiate(_BattleSystem,transform.position,transform.rotation);
              _BattleColosseum.SetActive(true);
              UnitState = State.Battle;
            }
      }
          
        Die();
         
    }

    //죽을 때 드랍? or 바로 들어가게?
 

}
