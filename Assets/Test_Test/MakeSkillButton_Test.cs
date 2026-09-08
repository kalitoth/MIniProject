using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI; 

public class MakeSkillButton_Test : MonoBehaviour
{
    [Header("버튼 인스펙터")]
    [SerializeField]
    Button _button;
    [SerializeField]
    private ScrollRect _scrollRect;

    Ray_Skill_SkillButton _ray_Test;
    RaycastHit _hit;

    [Header("버튼 스킬 저장소")]
    Dictionary<int, UnityEngine.Events.UnityAction> _skillAction = new Dictionary<int, UnityEngine.Events.UnityAction>();
    Dictionary<int, Sprite> _skillsprites = new Dictionary<int, Sprite>();
     
    //플레이어 스킬
   
    Player_Test _player;

    public Player_Test PlayerButton
    {
        get { return _player; }
        set { _player = value; }
    }
    [SerializeField]
    Player_Test[] _playerParty = new Player_Test[4];

    PlayerShift _playerShift;

    private void Awake()
    { 
        //버튼 저장소에 미리 저장해 놓는다
        _skillAction.Add(0, Sword);
        _skillsprites.Add(0, Resources.Load<Sprite>("Sword"));
        _skillAction.Add(1, Bow);
        _skillsprites.Add(1, Resources.Load<Sprite>("Bow"));
        _skillAction.Add(2, Fireball);
        _skillsprites.Add(2, Resources.Load<Sprite>("Fireball"));
        _skillAction.Add(3, Scroll);
        _skillsprites.Add(3, Resources.Load<Sprite>("Scroll"));
        _skillAction.Add(4, Jump);
        _skillsprites.Add(4, Resources.Load<Sprite>("Jump"));
        
         


    }
    void Start()
    {
        _playerShift = GetComponent<PlayerShift>();

        if(_playerShift == null )
        {
            Debug.Log("_playerShift가 null");
        }
        _ray_Test = GetComponent<Ray_Skill_SkillButton>();
        _player = _playerParty[0];
    }
    
    void Update()
    {
        if (_player.UnitState.HasFlag(Unit_Test.State.Skill))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_playerParty[0] == null)
            {
                return;
            }

            RemoveSkillButton(_player);
            _player = _playerParty[0];
            ReviveSkillButton(_player);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_playerParty[1] == null)
            {
                return;
            }

            RemoveSkillButton(_player);
            _player = _playerParty[1];
            ReviveSkillButton(_player);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_playerParty[2] == null)
            {
                return;
            }

            RemoveSkillButton(_player);
            _player = _playerParty[2];
            ReviveSkillButton(_player);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_playerParty[3] == null)
            {
                return;
            }

            RemoveSkillButton(_player);
            _player = _playerParty[3];
            ReviveSkillButton(_player);
        }

        //클릭한 캐릭터의 스킬로 전환
        if (_playerShift.SkillShiftHit.collider != null)
        {
            if (_hit.collider == _playerShift.SkillShiftHit.collider)
            {
           
                return;
            }
            if (_playerShift.SkillShiftHit.collider.gameObject.CompareTag("Player"))
            {
                
                _hit = _playerShift.SkillShiftHit;

                Debug.Log("버튼 삭제먼저?");
                RemoveSkillButton(_player);
                _player = _playerShift.Player;
                ReviveSkillButton(_player);
            }
        }
        
        
            
    }
   
    //초기 스킬트리
  public void MakeSkillTree()
  {
        foreach (Player_Test _player in _playerParty)
        { 
            if(_player != null)
            {
                foreach (KeyValuePair<int, Action<Player_Test, RaycastHit>> skill in _player.PlayerSkill)
                {
                    Button insbutton = Instantiate(_button, _scrollRect.content);

                    
                    _player.SkillButton.Add(insbutton);

                    insbutton.onClick.AddListener(_skillAction[skill.Key]);
                    insbutton.image.sprite = _skillsprites[skill.Key];
                    insbutton.gameObject.SetActive(false);

                }
                Debug.Log("버튼이 생성됐나?");
            }
            
            
        }
    }

    #region 옵션
    //스킬 추가 버튼
    public void AddSkillButton(int skillIndex)
    {
        Button insbutton = Instantiate(_button, _scrollRect.content);

        _player.SkillButton.Add(insbutton);

        insbutton.onClick.AddListener(_skillAction[skillIndex]);//스킬 리스트 add함수에서 키값 넣기
        insbutton.image.sprite = _skillsprites[skillIndex];
    }

    //버튼의 순서 바꾸기 나중에 수정 필요
    void ListChange()
    {
        Button temp = _player.SkillButton[0];
        _player.SkillButton[0] = _player.SkillButton[1];
        _player.SkillButton[1] = temp;

        _player.SkillButton[0].transform.SetSiblingIndex(0);

    }

    //현재 플레이어의 스킬 버튼 비활성화
    public void RemoveSkillButton(Player_Test _player)
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
            _player.SkillButton[i].gameObject.SetActive(false);
            
        }
       
    }
    //현재 플레이어의 스킬 버튼 활성화
    public void ReviveSkillButton(Player_Test _player)
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
           _player.SkillButton[i].gameObject.SetActive(true);
           
        }
        
    }
    public void SkillButtonInteractT(Player_Test _player)
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
            _player.SkillButton[i].interactable = true;
        }
        
    }
    public void SkillButtonInteractF(Player_Test _player)
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
            _player.SkillButton[i].interactable = false;
        }
        
    }

    
    #endregion


    #region 버튼 목록
    void Switch(int Index)
    {
        Debug.Log($"너 누군데 : {_player}");
        
        if (!_player.UnitState.HasFlag(Unit_Test.State.Skill))
        {
            _player.UnitState |= Unit_Test.State.Skill;
            _player._playerMoving.enabled = false; 
            _player._lineRenderer.enabled = true;
            _player._lineRenderer.SetPosition(1, _player.transform.position);
           
        }
        else
        {
            //스킬 바꾸면 그대로
            if (Index != _player._skillIndex)
            {
                _player._skillIndex = Index;
                _player._skillNum = 0;
                return;
            }

            //스킬 끔
             
            _player.UnitState &= ~Unit_Test.State.Skill;
            //왜 안되는 거지?
            //_player.Hit.point = _player.transform.position;
            _player._playerMoving.enabled = true;
            _player._lineRenderer.enabled = false;
            //_player.Hit.point
        }
        _player._skillIndex = Index;
        _player._skillNum = 0;

    }
    void Sword()
    {
        int Index = 0;
        Switch(Index); 
    }
    void Bow()
    {
        int Index = 1;
        Switch(Index); 
    }

    void Fireball()
    {
        int Index = 2;
        Switch(Index); 
    }
    void Scroll()
    {
        int Index = 3;
        Switch(Index); 
    }

    void Jump()
    {
        int Index = 4;
        Switch(Index);
    }
    #endregion
}

