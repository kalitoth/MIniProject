using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkeletonNew : Monster_Test
{


    //배틀 시스템 플레이어 받아오기
    BattleSystem _battleSystem;

    //가장 거리가 짧은 플레이어 찾기
    float _distanceMin;
    bool _distanceFirst = true;

    Vector3 _playerPosition;
    int _playerIndex;

    //몬스터 사거리
    float _monsterRange = 3f;

    //float _movingSpeed = 0.5f;
    //추적 시간
    //float _time;
    override protected void Awake()
    {
        
        base.Awake();

        // 여기에 몬스터 스텟 넣기
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;
        _monsterExp = 10;
        _monsterGold = 10;
    }

    protected override void Update()
    {
        base.Update();



        if (UnitState.HasFlag(State.Battle))
        {
            if (TurnEnable)
            {
                if (BattleReady)
                {
                    //_tracingIndex = _initialTraceIndex;
                    BattleReady = false;
                }

                if(_battleSystem == null)
                {
                    Debug.Log("_battleSystem이 null");
                }
                //가장 거리가 짧은 플레이어 찾기
                for (int i = 0; i < _battleSystem.Players.Count; i++)
                {
                    if (_distanceFirst)
                    {
                        _distanceMin = (_battleSystem.Players[i].transform.position - transform.position).sqrMagnitude;
                        _playerIndex = i;
                        _playerPosition = _battleSystem.Players[i].transform.position;
                       // _firstPlayer = _battleSystem.Players[i].transform.position;
                        _distanceFirst = false;
                        continue;
                    }

                    float distance = (_battleSystem.Players[i].transform.position - transform.position).sqrMagnitude;

                    if (_distanceMin > distance)
                    {
                        _distanceMin = distance;
                        _playerIndex = i;
                        _playerPosition = _battleSystem.Players[i].transform.position;
                       // _firstPlayer = _battleSystem.Players[i].transform.position;
                    }
                }
                _distanceFirst = true;
                 
                //전투를 공유하는데 처음 포지션이 없으면?

                //처음 감지된 것에서 
                if (_playerPosition == null)
                {
                    Debug.Log("플레이어 트랜스폼이 null");
                    return;
                }
                 
                //방향
                if (BattleStart)
                {
                   // _time = 0;
                    BattleStart = false;
                    _agent.isStopped = false;
                }
                
                _agent.SetDestination(_playerPosition);

                Animator.SetFloat("FMoving", (_playerPosition - transform.position).magnitude);

                Movement -= _agent.velocity.magnitude* Time.deltaTime;

                if (UsingSkillNum > 0)
                {
                    float sqrRange = _monsterRange * _monsterRange;
                    if ((_playerPosition - transform.position).sqrMagnitude < sqrRange)
                    {
                        Animator.SetTrigger("TSkillActivate");
                        _battleSystem.Players[_playerIndex].HP -= 1;
                        UsingSkillNum--;
                        Debug.Log($"스킬넘버에 들어오니? {UsingSkillNum}");
                    }
                   
                }

               //Debug.Log(_agent.updatePosition);
               //Debug.Log($"agent: {_agent.nextPosition}");
               //Debug.Log($"transform: {transform.position}");

  

                if (Movement <= 0 || UsingSkillNum == 0)
                {
                    Debug.Log($"몬스터 추적 끝");
                     
                    _playerPosition = transform.position;
                    Animator.SetFloat("FMoving", 0);
                   
                    _agent.isStopped = true;
                    TurnEnable = false;
                    TurnEnd = true;
                }

               
 

                Debug.Log($"목표물 좌표 {_playerPosition}");
                Debug.Log($"몬스터 좌표 {transform.position}");

                Debug.Log($"몬스터 이동력 {Movement}");

                Debug.Log("몬스터 턴 끝");

   


            }

        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("몬스터에 히트가 들어온다");
//
// if (hit.gameObject.CompareTag("Monster"))
// {
//     _time += Time.deltaTime;
//
//     if ((_projectionPlayer - transform.position).magnitude < 2.2f || _time > 3)
//     {
//
//         if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
//         {
//             _projectionPlayer = transform.position;
//             _animator.SetFloat("FMoving", 0);
//         }
//
//         _tracingIndex--;
//         TurnEnable = false;
//         TurnEnd = true;
//
//         _time = 0;
//         Debug.Log("몬스터가 멈춘다");
//     }
// }

    }
 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Battle"))
        {
            _battleSystem = other.gameObject.GetComponent<BattleSystem>();
            UnitState = State.Battle;
            Debug.Log("여긴 들어오지?");
        }
        
    }
    private void OnDestroy()
    {
        _playerInventory.shareExp += _monsterExp;
        _playerInventory.shareGold += _monsterGold;

        _playerInventory._skeletonGem += 1;

    }

    public void Reference(BattleSystem battleSystem, ShareRepository playerInventory)
    {
        _battleSystem = battleSystem;
        _playerInventory = playerInventory;
    }

}
