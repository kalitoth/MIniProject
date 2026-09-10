 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements; 

public class Skill_Manage : MonoBehaviour
{
    PlayerShift _playerShift;
    //Ray_Skill_SkillButton _ray_Test;
    private RaycastHit _hit;
     
    // 최대 4인팟 
    [SerializeField]
    Player_Test[] _playerParty = new Player_Test[4];

    //초기 플레이어
    Player_Test _player;

    MakeSkillButton_Test _button;

    //스킬 목록
    Skill_List Skill_List;

    private void Awake()
    {
        _playerShift = GetComponent<PlayerShift>();
        Skill_List = GetComponent<Skill_List>();
        _button = GetComponent<MakeSkillButton_Test>();
        //_ray_Test = GetComponent<Ray_Skill_SkillButton>();

        if (_button == null)
        {
            Debug.Log("스킬 리스트에 버튼이 없다");
        }
        if (_playerShift == null)
        {
            Debug.Log("스킬 리스트에 플레이어시프트가 없다");
        }
         
    }
    private void Start()
    { 
        //기본 스킬 + 스킬트리
        foreach (Player_Test player in _playerParty)
        {
            if (player != null)
            {
               // for (int i = 0; i < 4; i++)
                //{
                     
                    if(player.TryGetComponent<Warrior>(out _))
                    {
                        if (!player.PlayerSkill.ContainsKey(0))
                        {
                            player.PlayerSkill.Add(0, Skill_List.SkillList[0]);
                            player.AddSkill.Add(2,AddFireball);
                        }
                            
                    }
                    if (player.TryGetComponent<Wizard>(out _))
                    { 
                        if (!player.PlayerSkill.ContainsKey(1))
                        {
                            player.PlayerSkill.Add(1, Skill_List.SkillList[1]);
                            player.AddSkill.Add(3, AddMagicMissale);
                        }
                           
                    }
                    //player.PlayerSkill.Add(i, Skill_List.SkillList[i]);
                //}

            }
        }
        // 모든 캐릭터의 스킬트리 만들기 + 모든 스킬 active false
        _button.MakeSkillTree();
        _button.AddSkillTree();

        _player = _playerParty[0];
        // 현재 캐릭터의 스킬트리만 active true
        _button.ReviveSkillButton(_player);
        _button.ReviveAddSkillButton(_player);



    }

    private void Update()
    {

       //if (_player.UnitState.HasFlag(Unit_Test.State.Skill))
       //{
       //    //Debug.Log("스킬바꾸기 안들어감");
       //    return;
       //}

        if(_player != _playerShift.Player)
        {
            _player = _playerShift.Player;
        }

        ChangeSkillAdd();

    }

    #region 스킬add관련

   
    //스킬 add는 동료를 넣었을 때 - 이거 아님, 레벨업 했을 때, 무기를 바꿔 끼웠을 때
    //스킬 추가
    public void SkillAdd(int skillIndex)
    {
        //스킬이 이미 있으면 추가 x 
        if (_player.PlayerSkill.ContainsKey(skillIndex))
        {
            return;
        }
        _player.skillPoint--;
        _player.PlayerSkill.Add(skillIndex, Skill_List.SkillList[skillIndex]);
        _button.AddSkillButton(skillIndex);
    }
    void ChangeSkillAdd()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_playerParty[0] == null)
            {
                return;
            }

            _player = _playerParty[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_playerParty[1] == null)
            {
                return;
            }

            _player = _playerParty[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_playerParty[2] == null)
            {
                return;
            }

            _player = _playerParty[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_playerParty[3] == null)
            {
                return;
            }

            _player = _playerParty[3];
        }
    }

    #endregion

    #region 스킬 add목록


    public void AddFireball()
    {
        if(_player.skillPoint == 0)
        {
            return;
        }
         
        int skillIndex = 2;
        SkillAdd(skillIndex);
    }
    public void AddMagicMissale()
    {
        if (_player.skillPoint == 0)
        {
            return;
        }
       
        int skillIndex = 3;
        SkillAdd(skillIndex);
    }

    #endregion
}

