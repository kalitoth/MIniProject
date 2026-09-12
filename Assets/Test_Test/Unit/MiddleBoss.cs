using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiddleBoss : Monster_Test
{
    //스켈레톤 소환
    [SerializeField]
    SkeletonNew _skeleton;

    [SerializeField]
    GameEndObject _gameEndObject;

    //배틀 시스템 플레이어 받아오기
    BattleSystem _battleSystem;

    //가장 거리가 짧은 플레이어 찾기
    float _distanceMin;
    bool _distanceFirst = true;

    Vector3 _playerPosition;
    int _playerIndex;

    float _monsterRange = 12f;

    bool _dieOneShot = true;

    int _damage = 2;
    override protected void Awake()
    {

        base.Awake();

        MAXHP = 50;
        HP = MAXHP;
        _monsterExp = 1000;
        _monsterGold = 1000;
    }

    protected override void Update()
    {
        base.Update();

        if (!Alive)
        {
            if (_dieOneShot)
            {
                if (UnitAudioSource.isPlaying)
                { 
                    UnitAudioSource.PlayOneShot(DieAudio());
                }
                 
                GameEndObject gameobject = Instantiate(_gameEndObject, transform.position, transform.rotation);
                gameobject.Reference(_playerInventory);
                _dieOneShot = false;

            }
        }

        if (UnitState.HasFlag(State.Battle))
        {
            if (TurnEnable)
            {
                if (BattleReady)
                {
                    oneDestination = true; 
                    BattleReady = false;
                }

                if (_battleSystem == null)
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
                        _distanceFirst = false;
                        continue;
                    }

                    float distance = (_battleSystem.Players[i].transform.position - transform.position).sqrMagnitude;

                    if (_distanceMin > distance)
                    {
                        _distanceMin = distance;
                        _playerIndex = i;
                        _playerPosition = _battleSystem.Players[i].transform.position;
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

                Movement -= _agent.velocity.magnitude * Time.deltaTime;

                
                 

                if (UsingSkillNum > 0)
                {
                    float sqrRange = _monsterRange * _monsterRange;
                    float bossPhase = HP / (float)MAXHP;
                    if ((_playerPosition - transform.position).sqrMagnitude < sqrRange)
                    {
                        if (bossPhase < 0.3f)
                        {
                            UnitAudioSource.PlayOneShot(CurseAudio());
                            _battleSystem.Players[_playerIndex].MAXHP -= 1;
                            
                        }
                        
                        if(bossPhase < 0.8f)
                        {
                            UnitAudioSource.PlayOneShot(SqawnAudio());
                            SkeletonNew skeleton = Instantiate(_skeleton, transform.position+ transform.forward, transform.rotation);
                            skeleton.Reference(_BattleColloseum, _battleSystem, _playerInventory);

                            Debug.Log($"스켈레톤 소환함 : {transform.position + transform.forward}");
                        }
                        else
                        {
                            TakeDamage(_battleSystem.Players[_playerIndex], _damage);
                        }

                        
                        Animator.SetTrigger("TSkillActivate");
                        UsingSkillNum--;
                    }

                    
                }
                

               

                //턴 끝
                if (Movement <= 0 || UsingSkillNum == 0)
                {
                    Debug.Log($"몬스터 추적 끝");

                    _playerPosition = transform.position;
                    Animator.SetFloat("FMoving", 0);

                    _agent.isStopped = true;
                    TurnEnable = false;
                    TurnEnd = true;
                }


                //Debug.Log(_agent.updatePosition);
                //Debug.Log($"agent: {_agent.nextPosition}");
                //Debug.Log($"transform: {transform.position}");


                Debug.Log($"목표물 좌표 {_playerPosition}");
                //Debug.Log($"몬스터 좌표 {transform.position}");

                //Debug.Log($"몬스터 이동력 {Movement}");

                Debug.Log("몬스터 턴 끝");




            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battle"))
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

    }

 


    AudioClip SqawnAudio()
    {
        return UnitAudioClip[0];
    }

    AudioClip DieAudio()
    {
        return UnitAudioClip[1];
    }
    AudioClip CurseAudio()
    {
        return UnitAudioClip[3];
    }
}

