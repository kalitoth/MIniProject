using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit_Test : MonoBehaviour
{


    private int _strength = 10;
    private int _intelligence = 10;
    private int _dexterity = 10;
    private int _constitution = 10;

    //이동력
    private float _movement = 10;
    //스피드
    private int _speed = 8;

    private int _hp;
    private int _maxHp;
    private int _basicHp = 10;

    private int _exp;
    private int _maxExp;
    private int _level = 1;

    private int _usingSkillNum;

    private bool _turnEnable;
    private bool _turnEnd;

    private bool _battleReady = true;
    private bool _battleStart = true;

    private bool _alive = true;

    public Sprite _image;

    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int MAXHP
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }
    public int BasicHp
    {
        get { return _basicHp; }
        set { _basicHp = value; }
    }
    public int Constitution
    {
        get { return _constitution; }
        set { _constitution = value; }
    }
    public float Movement
    {
        get { return _movement; }
        set { _movement = value; }
    }
    public int Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public int Exp
    {
        get { return _exp; }
        set { _exp = value; }
    }
    public int MaxExp
    {
        get { return _maxExp; }
        set { _maxExp = value; }
    }
    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }
    public int UsingSkillNum
    {
        get { return _usingSkillNum; }
        set { _usingSkillNum = value; }
    }
    public bool TurnEnable
    {
        get { return _turnEnable; }
        set { _turnEnable = value; }
    }
    public bool TurnEnd
    {
        get { return _turnEnd; }
        set { _turnEnd = value; }
    }
    public bool BattleReady
    {
        get { return _battleReady; }
        set { _battleReady = value; }
    }
    public bool BattleStart
    {
        get { return _battleStart; }
        set { _battleStart = value; }
    }
    public bool Alive
    {
        get { return _alive; }
        set { _alive = value; }
    }

    public void Die()
    {
        if(_hp <= 0)
        {
            _alive = false;
            
            if(this.CompareTag("Monster"))
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            
        }
         
    }
  

     State _state = State.None;

    public State UnitState
    {
        get { return _state; }
        set { _state = value; }
    }


    [Flags]
   public enum State : byte
   {
       Nothing = 0b0000,
       None = 0b0001,
       Skill = 0b0010,
       Battle = 0b0100,
   }
   public void BattleTurnTrigger()
   {
       BattleStart = true;
       TurnEnable = true;
       UsingSkillNum = 1;
        Movement = 6;
        _state |= State.None;
   }
    public void EnterBattleUnit()
    {
        _state = State.Battle;
    }
 
}
