using System;
using System.Collections;
using System.Collections.Generic;
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

    private int _level = 1;

    private bool _turnEnable;

    private bool _battleEnd;

    
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
    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }
    public bool TurnEnable
    {
        get { return _turnEnable; }
        set { _turnEnable = value; }
    }
    public bool BattleEnd
    {
        get { return _battleEnd; }
        set { _battleEnd = value; }
    }


    public void BattleTurnTrigger()
    {
        TurnEnable = true;
    }

   //State _state = State.None;
   //[Flags]
   //public enum State : byte
   //{
   //    Nothing = 0b0000,
   //    None = 0b0001,
   //    Skill = 0b0010,
   //    Battle = 0b0100,
   //}
   //public void EnterBattleUnit()
   //{
   //    _state = State.Battle;
   //}
}
