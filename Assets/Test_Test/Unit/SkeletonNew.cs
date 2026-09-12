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

    int _damage = 1;

    bool _dieOneShot = true;
    
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

        if (!Alive)
        {
            if(_dieOneShot)
            {
                UnitAudioSource.PlayOneShot(DieAudio());
                
                _dieOneShot = false;
            }
            
        }

        if (UnitState.HasFlag(State.Battle))
        {
            if (BattleReady)
            {
                oneDestination = true;
                BattleReady = false;
            }

            if (TurnEnable)
            {
                //방향
                if (BattleStart)
                {
                    _agent.isStopped = false;
                    //TurnEnd = false;

                    BattleStart = false;
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
                 
                 

                //처음 감지된 것에서 
                if (_playerPosition == null)
                {
                    Debug.Log("플레이어 트랜스폼이 null");
                    return;
                }
                 
                
                
                _agent.SetDestination(_playerPosition);

                Animator.SetFloat("FMoving", (_playerPosition - transform.position).magnitude);

                Movement -= _agent.velocity.magnitude* Time.deltaTime;

                if (UsingSkillNum > 0)
                {
                    float sqrRange = _monsterRange * _monsterRange;
                    if ((_playerPosition - transform.position).sqrMagnitude < sqrRange)
                    {
                        UnitAudioSource.PlayOneShot(AttackAudio());
                        Animator.SetTrigger("TSkillActivate");
                        TakeDamage(_battleSystem.Players[_playerIndex], _damage);
                        UsingSkillNum--;
                        Debug.Log($"스킬넘버에 들어오니? {UsingSkillNum}");
                    }
                   
                }
 

                if (Movement <= 0 || UsingSkillNum == 0)
                {
                    Debug.Log($"몬스터 추적 끝");
                     
                    _playerPosition = transform.position;
                    Animator.SetFloat("FMoving", 0);
                   
                    _agent.isStopped = true;
                    TurnEnd = true;

                    TurnEnable = false;
                }

               
 

                Debug.Log($"목표물 좌표 {_playerPosition}");
                //Debug.Log($"몬스터 좌표 {transform.position}");

                Debug.Log($"몬스터 이동력 {Movement}");

                Debug.Log("몬스터 턴 끝");

   


            }

        }
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

    public void Reference(GameObject BattleColloseum, BattleSystem battleSystem, ShareRepository playerInventory)
    {
        _BattleColloseum = BattleColloseum; 
        _battleSystem = battleSystem;
        _playerInventory = playerInventory;
       // _initialPosition = position;
       // Animator.SetFloat("FMoving", 0);
    }

    AudioClip AttackAudio()
    {
        return UnitAudioClip[0];
    }
    AudioClip DieAudio()
    {
        return UnitAudioClip[1];
    }
}
