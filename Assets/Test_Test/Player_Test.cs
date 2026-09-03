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

    RaycastHit _hit;
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
    //게임 초상화 이미지
    //이건 캐릭터 선택에서 부여해야 한다
    public Sprite _image;

    //스킬 고유 번호
    public int _skillIndex;
    //스킬 사용 횟수
    public int _skillNum = 0;
    public int _attackNum = 0;

    float _lineWidth = 0.05f;

    //상태
     
     

    //배틀 턴 
    private bool _battleReady = true;
    private bool _battleStart = true;
  
   
    private void Awake()
    {
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
         
        
    }

    
    void Update()
    {
         

        //임시 - 네브메쉬 들어가면 뺄 것
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _playerMoving.enabled = true;
        }

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
                if(_battleReady)
                {
                    // 이건 배틀 상태가 될 때 한번
                    if (!_playerMoving.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        _playerMoving.RayHitPoint = transform.position;
                        _playerMoving.Animator.SetFloat("FMoving", 0);
                    }
                    //배틀이 끝나면 다시 켜기
                    _battleReady = false;
                }
                
            if (TurnEnable)
                {
                 
                    // 배틀 시작할 때 주는 것
                   if(_battleStart)
                   {

                    UnitState |= State.None;

                    //턴 넘기기 버튼에서 true
                       _battleStart = false;
                   }


                if (UnitState.HasFlag(State.None))
                {
                    Movement -= _playerMoving.CharacterController.velocity.magnitude * Time.deltaTime;
                    //Debug.Log($"무브먼트  : {Movement}");
                }
                    
                //배틀 도중에 없어져야 할 것
                //스킬 횟수 공격 횟수
                if (Movement <= 0)
                {
                    Debug.Log($"이동불가");

                    UnitState &= ~State.None;
                    if (!_playerMoving.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        _playerMoving.RayHitPoint = transform.position;
                        _playerMoving.Animator.SetFloat("FMoving", 0);
                    }

                    Movement = 9;
                }
               
                
                     
                

                if(TurnEnd)
                {
                    Debug.Log("턴 끝");
                    TurnEnable = false;
                    _battleStart = true;

                    
                }
            }
            }
       

            //임시
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnEnable = true;
            Debug.Log("턴 시작");
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnitState = State.Battle;
            Debug.Log("배틀 시작");
        }
        
    }

    void PlayerUseSkill()
    {

        if (!_playerMoving.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //스킬 쓰면 스킬 취소
            //_playerMoving.enabled = true;
            //_lineRenderer.enabled = false;
            //_state = State.None;

            //스킬 쓰면 이동 취소 
            _playerMoving.RayHitPoint = transform.position;
            _playerMoving.Animator.SetFloat("FMoving", 0);
        }
 
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            _playerMoving.enabled = true;
            _lineRenderer.enabled = false;
            //_state = State.None;
            UnitState &= ~State.Skill;
            _skillNum = 0;
        }

        PlayerTarget();

        _playerSkill[_skillIndex](this, _hit);
         
    }

    void PlayerTarget()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray target = _camera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(target, out _hit, _distance,_layerMask);
 
            Debug.DrawLine(transform.position, _hit.point, Color.blue, 0.000001f);

          

           _lineRenderer.SetPosition(0, transform.position);
            
            if(_hit.collider == null)
            {
                _hit.point = transform.position;
            }

           _lineRenderer.SetPosition(1, _hit.point);

        }
      
    }

    
   
  
}
