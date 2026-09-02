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
    [SerializeField]
    Player_Test _player;
    [SerializeField]
    Player_Test[] _playerParty = new Player_Test[4];

     

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
 
    }
    void Start()
    { 
        _ray_Test = GetComponent<Ray_Skill_SkillButton>();
       
    }
    
    void Update()
    {
        if (_player._state.HasFlag(Player_Test.State.Skill))
        {
            return;
        }

        
        //클릭한 캐릭터의 스킬로 전환
        if (_ray_Test.Hit.collider != null)
        {
            if (_hit.collider == _ray_Test.Hit.collider)
            {
                return;
            }

            if (_ray_Test.Hit.collider.gameObject.CompareTag("Player"))
            {
                _hit = _ray_Test.Hit;

                Debug.Log("버튼 삭제먼저?");
                RemoveSkillButton();
                _player = _hit.collider.gameObject.GetComponent<Player_Test>();
                ReviveSkillButton();
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
    void RemoveSkillButton()
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
            _player.SkillButton[i].gameObject.SetActive(false);

        }
    }
    //현재 플레이어의 스킬 버튼 활성화
    public void ReviveSkillButton()
    {
        for (int i = 0; i < _player.SkillButton.Count; i++)
        {
            _player.SkillButton[i].gameObject.SetActive(true);

        }
    }

    
    #endregion


    #region 버튼 목록
    void Switch(int Index)
    { 
        if (!_player._state.HasFlag(Player_Test.State.Skill))
        {
            _player._state |= Player_Test.State.Skill;
            _player._playerMoving.enabled = false; 
            _player._lineRenderer.enabled = true;
            _player._lineRenderer.SetPosition(1, _player.transform.position);
           
        }
        else
        {
            if (Index != _player._skillIndex)
            {
                _player._skillIndex = Index;
                _player._skillNum = 0;
                return;
            }
            _player._state &= ~Player_Test.State.Skill;
            _player._playerMoving.enabled = true;
            _player._lineRenderer.enabled = false;
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
    #endregion
}

