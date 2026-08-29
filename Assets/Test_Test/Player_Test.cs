using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player_Test : Unit_Test
{

    //이동 
    PlayerMoving _playerMoving;

    //스킬 리스트
    Dictionary<int, Action<Player_Test>> _playerSkill = new Dictionary<int, Action<Player_Test>>(); 

    public Dictionary<int, Action<Player_Test>> PlayerSkill
    {
        get {  return _playerSkill; }
    }

    //스킬 고유 번호
    public int _skillIndex;

    //상태
    public State _state = State.None;
    public enum State
    {
        None,
        Skill
    }

    void Start()
    {
        _playerMoving = GetComponent<PlayerMoving>();

        if (_playerMoving == null)
        {
            Debug.Log("플레이어에 플레이어 무빙이 없다");
        }

        //능력치
        //최대 체력 = 기본 점수 + 수정치 * (레벨 + 직업에 따른 점수) > 직업이 없으니 생략 
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10)*0.5f)* Level;
        HP = MAXHP;
    }

    
    void Update()
    {
        if (_state == State.None)
        { 
            _playerMoving.Moving();
        }

           
        if (_state == State.Skill)
        {
            if (!_playerMoving.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _state = State.None;
            }
            _playerSkill[_skillIndex](this);
             
        }
        
    }

    

  
}
