using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class Player_Test : Unit_Test
{
   
    
    [SerializeField]
    Camera _camera;
    [SerializeField]
    GameObject _playerSight;
    [SerializeField]
    ShareRepository _share;
    
    MakeSkillButton_Test _makeSkillButton;


   // AudioSource _audioSource;
   // public AudioSource AudioSource => _audioSource;

    public RaycastHit _hit;
     
    public RaycastHit Hit
    {
        get { return _hit; }
        set { _hit = value; }
    }
    LayerMask _layerMask;
    float _distance = 500;
    //이동 
    public PlayerMoving _playerMoving;
    public LineRenderer _lineRenderer;

    
    //스킬 리스트
    Dictionary<int, Action<Player_Test,RaycastHit>> _playerSkill = new Dictionary<int, Action<Player_Test, RaycastHit>>();
    //스킬 버튼
    List<Button> _skillButton = new List<Button>(10);

    public Dictionary<int, Action<Player_Test, RaycastHit>> PlayerSkill
    {
        get {  return _playerSkill; }
    }
    public List<Button> SkillButton
    {
        get {  return _skillButton; }
    }

    //스킬 포인트
    public int skillPoint = 0;

    //스킬 add 버튼
    Dictionary<int, UnityEngine.Events.UnityAction> _AddSkill = new Dictionary<int, UnityEngine.Events.UnityAction>();

    List<Button> _addSkillButton = new List<Button>(10);

    public Dictionary<int, UnityEngine.Events.UnityAction> AddSkill
    {
        get { return _AddSkill; }
        set { _AddSkill = value; }
    }

    public List<Button> AddSkillButton
    {
        get { return _addSkillButton; }
        set { _addSkillButton = value; }
    }

    //게임 초상화 이미지
    //이건 캐릭터 선택에서 부여해야 한다
    //public Sprite _image;

    //스킬 고유 번호
    public int _skillIndex;
    //스킬 사용 횟수
    public int _skillNum = 0;
    public int _attackNum = 0;

    float _lineWidth = 0.05f;
  
    protected override void Awake()
    {
        base.Awake();

        
        UnitState = State.None;
        Instantiate(_playerSight,gameObject.transform);
        _layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Ground");

        _lineRenderer = GetComponent<LineRenderer>();
        _playerMoving = GetComponent<PlayerMoving>();

        if (_playerMoving == null)
        {
            Debug.Log("플레이어에 플레이어 무빙이 없다");
        }

        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;

        //능력치
        //최대 체력 = 기본 점수 + 수정치 * (레벨 + 직업에 따른 점수) > 직업이 없으니 생략 
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;
    }
    void Start()
    {
        _makeSkillButton = _share.gameObject.GetComponent<MakeSkillButton_Test>();

        
    }

    
    void Update()
    { 
        if (UnitState.HasFlag(State.None))
        {
            _playerMoving.Moving(); 
        }

        if (UnitState.HasFlag(State.Skill))
        {
            PlayerUseSkill();
        }
         
        if (UnitState.HasFlag(State.Battle))
        {
             InBattle();
        }
         
        Die();
         
        if(Input.GetKeyDown(KeyCode.O))
        {
            _share.shareExp += 100;
            Debug.Log("경험치 + 100");
        }
        if(_share.shareExp >= MaxExp)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                Level++;
                MaxExp += MaxExp * Level;
                //스텟 선택
                //스킬 선택
                //스킬 포인트
                skillPoint++;
                Debug.Log("레벨업");
            }
        }

    }
    void InBattle()
    {
        if (BattleReady)
        {
            // 이건 배틀 상태가 될 때 한번
            if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _playerMoving.RayHitPoint = transform.position;
                Animator.SetFloat("FMoving", 0);
            }
            //배틀이 끝나면 다시 켜기
            _makeSkillButton.SkillButtonInteractF(this);
            _playerMoving.enabled = false;
            BattleReady = false;
        }

        if (TurnEnable)
        {

            // 배틀 시작할 때 주는 것
            if (BattleStart)
            {
                BattleStart = false;

                Movement += 3;
                UnitState |= State.None;

                _makeSkillButton.SkillButtonInteractT(this);
                //턴 넘기기 버튼에서 true
            }


            if (UnitState.HasFlag(State.None))
            {
                Movement -= _playerMoving.CharacterController.velocity.magnitude * Time.deltaTime;
                //Debug.Log($"플레이어 이동력  : {Movement}");

                if (Movement <= 0)
                {
                    Debug.Log($"이동불가");

                    UnitState &= ~State.None;
                    if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        _playerMoving.RayHitPoint = transform.position;
                        Animator.SetFloat("FMoving", 0);
                    }


                }
            }

            //배틀 도중에 없어져야 할 것
            //스킬 횟수 공격 횟수


            if (TurnEnd)
            {
                Debug.Log("턴 끝");
                TurnEnable = false;

                //이동 취소 
                UnitState &= ~State.None;
                _playerMoving.enabled = false;

                if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    _playerMoving.RayHitPoint = transform.position;
                    Animator.SetFloat("FMoving", 0);
                }

                //스킬 취소
                Animator.SetBool("BSkillReady", false);
                _playerMoving.enabled = true;
                _lineRenderer.enabled = false;
                UnitState &= ~State.Skill;
                _skillNum = 0;

                //스킬 버튼 비활성화 
                _makeSkillButton.SkillButtonInteractF(this);
            }
        }
    }
    void PlayerUseSkill()
    {

        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //스킬 쓰면 스킬 취소
            //_playerMoving.enabled = true;
            //_lineRenderer.enabled = false;
            //_state = State.None;

            //스킬 쓰면 이동 취소 
            _playerMoving.RayHitPoint = transform.position;
            Animator.SetFloat("FMoving", 0);
        }
        
        Animator.SetBool("BSkillReady", true);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Animator.SetBool("BSkillReady", false);
            _playerMoving.enabled = true;
            _lineRenderer.enabled = false; 
            UnitState &= ~State.Skill;
            _skillNum = 0;
        }
        //스킬 사용
        
        PlayerTarget();
        if(!UnitAudioSource.isPlaying)
        {
            UnitAudioSource.PlayOneShot(SkillReadyAudio());
        }
        
        _playerSkill[_skillIndex](this, _hit);
         
    }

    void PlayerTarget()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray target = _camera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(target, out _hit, _distance, _layerMask);
            
            Debug.DrawLine(transform.position, _hit.point, Color.blue, 0.000001f);
             
           _lineRenderer.SetPosition(0, transform.position);
            
            if(_hit.collider == null)
            {
                _hit.point = transform.position;
            }

           _lineRenderer.SetPosition(1, _hit.point);

        }
      
    }

    AudioClip SkillReadyAudio()
    {
        return UnitAudioClip[0];
    }
    public AudioClip DieAudio()
    {
        return UnitAudioClip[1];
    }
   
  
}
