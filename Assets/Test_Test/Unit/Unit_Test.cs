using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit_Test : MonoBehaviour
{

    [Header("오디오")]
    [SerializeField]
    AudioClip[] _unitAudioClip;
    AudioSource _unitAudioSource;
    //만약에 오디오 클립 각각을 함수로 만들어 놓으면 그 유닛 컴포넌트만 알아도 함수로 오디오 클립을 가져올 수 있다
    public AudioClip[] UnitAudioClip
    {
        get { return _unitAudioClip; }
        set { _unitAudioClip = value; }
    }
    public AudioSource UnitAudioSource
    {
        get { return _unitAudioSource; }
        set { _unitAudioSource = value; }
    }
    Animator _animator;
    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }
    protected virtual void Awake()
    {
        _unitAudioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }
    private int _strength = 10;
    private int _intelligence = 10;
    private int _dexterity = 10;
    private int _constitution = 10;

    //이동력
    private float _movement = 6;
    //스피드
    private int _speed = 8;

    private int _hp;
    private int _maxHp;
    private int _basicHp = 10;

    private int _maxExp = 10;
    private int _level = 1;

    private int _usingSkillNum;

    private bool _turnEnable;
    private bool _turnEnd;

    private bool _battleReady = true;
    private bool _battleStart = true;

    private bool _alive = true;

    public Sprite _image;

    public int Strength
    {
        get { return _strength; }
        set { _strength = value; }
    }
    public int Intelligence
    {
        get { return _intelligence; }
        set { _intelligence = value; }
    }
    public int Dexterity
    {
        get { return _dexterity; }
        set { _dexterity = value; }
    }
     
    public int HP
    {
        get { return _hp; }
        set
        { 
            _hp = Mathf.Clamp(value, 0, _maxHp);
        }
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
        if (_hp <= 0)
        {
            _alive = false;

            _animator.SetTrigger("TDie");

            
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

    public void TakeDamage(Unit_Test unit ,int damage)
    {
        unit._hp -= damage;
        unit._animator.SetTrigger("TGetHit");
        unit.UnitAudioSource.PlayOneShot(unit.UnitAudioClip[2]);
    }

    
}
