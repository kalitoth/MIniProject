using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster_Test
{
    Vector3 _playerPosition;
    Vector3 _position;

    float _distanceMin;
    int _playerIndex;

    float _gravity = -9.81f;

    bool _distanceFirst = true;

    float _movingSpeed = 0.5f;

    int _tracingIndex = 2;
    void Awake()
    {
        // 여기에 몬스터 스텟 넣기
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;

        
    }

    protected override void Update()
    {
        base.Update();



        if (UnitState.HasFlag(State.Battle))
        {
            if (TurnEnable)
            {
                //여기에 ai

                //가장 거리가 짧은 플레이어 찾기
                int a = 4;
                for(int i = 0;  i < 4; i++)
                {
                    if(!_getSight._playerList.ContainsKey(i))
                    {
                        a--;
                        continue;
                    }
                    if (_distanceFirst)
                    {
                        _distanceMin = (_getSight._playerList[i].transform.position - transform.position).sqrMagnitude;
                        _distanceFirst = false;
                        _playerIndex = i;
                        _playerPosition = _getSight._playerList[i].transform.position;
                        _firstPlayer = _getSight._playerList[i].transform.position;
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
                if(_playerPosition == null)
                {
                    Debug.Log("플레이어 트랜스폼이 null");
                    _playerPosition = _firstPlayer;
                    return;
                }

                //방향
                transform.rotation = Quaternion.LookRotation((_playerPosition - transform.position).normalized, Vector3.up);

                //이동 
                Vector3 _projectionPlayer = Vector3.ProjectOnPlane(_playerPosition, Vector3.up);
                Vector3 _projectionMonster = Vector3.ProjectOnPlane(transform.position, Vector3.up);

                Vector3 move =  _projectionPlayer - _projectionMonster;
                move.y = _gravity;
                
                
                if( a > 0)
                {
                    if(UsingSkillNum > 0)
                    {
                        _getSight._playerList[_playerIndex].HP -= 1;
                        UsingSkillNum--;
                    }
                    
                }

                _characterController.Move(move * _movingSpeed * Time.deltaTime);
                _animator.SetFloat("FMoving", (_projectionPlayer - _projectionMonster).magnitude);

                Movement -= _characterController.velocity.magnitude * Time.deltaTime;

                if (Movement <= 0 || UsingSkillNum == 0)
                {
                    Debug.Log($"몬스터 추적 끝");

                    //추적 인덱스
                    if(a == 0)
                    {
                        _tracingIndex--;
                    }
                    else
                    {
                        _tracingIndex = 2;
                    }

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
                    _tracingIndex = 2;
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
}
