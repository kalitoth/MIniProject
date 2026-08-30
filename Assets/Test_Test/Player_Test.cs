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

    RaycastHit _hit;
    //이동 
    PlayerMoving _playerMoving;
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

    float _lineWidth = 0.05f;

    //상태
    public State _state = State.None;
    public enum State
    {
        None,
        Skill
    }

    void Start()
    { 
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
                _lineRenderer.enabled = false;
                _state = State.None;
            }
            
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                _lineRenderer.enabled = false;
                _state = State.None;
            }

            PlayerTarget();

             _playerSkill[_skillIndex](this, _hit);
              
        }
        
    }

    void PlayerTarget()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray target = _camera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(target, out _hit);

 
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
