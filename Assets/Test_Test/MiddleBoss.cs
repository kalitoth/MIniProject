using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Monster_Test
{
    [SerializeField]
    Skeleton _skeleton;

    Vector3 _projectionPlayer;
    Vector3 _projectionMonster;
    Vector3 _move;

    Vector3 _playerPosition;
    Vector3 _position;

    float _distanceMin;
    int _playerIndex;

    float _gravity = -9.81f;

    bool _distanceFirst = true;

    float _movingSpeed = 0.5f;

    int _tracingIndex;
    int _initialTraceIndex = 3;

    float _time;

    private void Awake()
    {
        //MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        MAXHP = 100;
          HP = MAXHP;
        _monsterExp = 1000;
        _monsterGold = 1000;
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
                   _tracingIndex = _initialTraceIndex;
                   BattleReady = false;
               }
               
               //가장 거리가 짧은 플레이어 찾기
               int playerNum = 4;
               for (int i = 0; i < 4; i++)
               {
                   if (!_getSight._playerList.ContainsKey(i))
                   {
                       playerNum--;
                       continue;
                   }
                   if (_distanceFirst)
                   {
                       _distanceMin = (_getSight._playerList[i].transform.position - transform.position).sqrMagnitude;
                       _playerIndex = i;
                       _playerPosition = _getSight._playerList[i].transform.position;
                       _firstPlayer = _getSight._playerList[i].transform.position;
                       _distanceFirst = false;
                       continue;
                   }
               
                   float b = (_getSight._playerList[i].transform.position - transform.position).sqrMagnitude;
               
                   if (_distanceMin > b)
                   {
                       _distanceMin = b;
                       _playerIndex = i;
                       _playerPosition = _getSight._playerList[i].transform.position;
                       _firstPlayer = _getSight._playerList[i].transform.position;
                   }
               }
               _distanceFirst = true;
               
               //전투를 공유하는데 처음 포지션이 없으면?
               
               //처음 감지된 것에서 
               if (_playerPosition == null)
               {
                   Debug.Log("플레이어 트랜스폼이 null");
                   _playerPosition = _firstPlayer;
                   return;
               }
               
               //방향
               if (BattleStart)
               {
                   transform.rotation = Quaternion.LookRotation((_playerPosition - transform.position).normalized, Vector3.up);
                   _time = 0;
                   BattleStart = false;
               }
               
               
               
               //이동 
               _projectionPlayer = Vector3.ProjectOnPlane(_playerPosition, Vector3.up);
               _projectionMonster = Vector3.ProjectOnPlane(transform.position, Vector3.up);
               
               _move = _projectionPlayer - _projectionMonster;
               _move.y = _gravity;
               
               
               if (playerNum > 0)
               {
                   if (UsingSkillNum > 0)
                   {
                        Instantiate(_skeleton, (_projectionPlayer+transform.position)*0.5f, _getSight._playerList[_playerIndex].transform.rotation);
                       // _getSight._playerList[_playerIndex].HP -= 1;
                       UsingSkillNum--;
                   }
               
               }
               
               
               
               _characterController.Move(_move * _movingSpeed * Time.deltaTime);
               _animator.SetFloat("FMoving", (_projectionPlayer - _projectionMonster).magnitude);
               
               Movement -= _characterController.velocity.magnitude * Time.deltaTime;
               
               if (Movement <= 0 || UsingSkillNum == 0)
               {
                   Debug.Log($"몬스터 추적 끝");
               
                   //추적 인덱스
                   if (playerNum == 0)
                   {
                       _tracingIndex--;
                   }
                   else
                   {
                       _tracingIndex = _initialTraceIndex;
                   }
               
                   if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                   {
                       _projectionPlayer = transform.position;
                       _animator.SetFloat("FMoving", 0);
                   }
               
                   TurnEnable = false;
                   TurnEnd = true;
               }
               
               if (playerNum == 0 && Movement > 0 && (_playerPosition - transform.position).sqrMagnitude < 2f)
               {
                   _tracingIndex--;
               
                   if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                   {
                       _projectionPlayer = transform.position;
                       _animator.SetFloat("FMoving", 0);
                   }
               
                   TurnEnable = false;
                   TurnEnd = true;
               }
               
               if (_tracingIndex <= 0)
               {
                   //전투 상태가 풀리고 원래 있던 곳으로 가야 한다 
                   UnitState = State.None;
                   TurnEnable = false;
                   TurnEnd = true;
               }
               

                Debug.Log($"몬스터 이동력 {Movement}");

                Debug.Log("몬스터 턴 끝");
            }

        }
    }

    private void OnDestroy()
    {
        _playerInventory.shareExp += _monsterExp;
        _playerInventory.shareGold += _monsterGold;

        _playerInventory._skeletonGem += 1;

    }

}

