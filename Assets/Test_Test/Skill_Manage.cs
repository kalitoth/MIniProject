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
    Ray_Test _ray_Test;
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
        Skill_List = GetComponent<Skill_List>();
        _button = GetComponent<MakeSkillButton_Test>();
        _ray_Test = GetComponent<Ray_Test>();

        if (_button == null)
        {
            Debug.Log("스킬 리스트에 버튼이 없다");
        }
        if (_ray_Test == null)
        {
            Debug.Log("스킬 리스트에 레이가 없다");
        }

        
      
    }
    private void Start()
    {
        //기본 스킬
        foreach (Player_Test player in _playerParty)
        {
            if (player != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    player.PlayerSkill.Add(i, Skill_List.SkillList[i]);
                }

            }
        }
        // 모든 캐릭터의 스킬트리 만들기 + 모든 스킬 active false
        _button.MakeSkillTree();
        // 현재 캐릭터의 스킬트리만 active true
        _button.ReviveSkillButton();

        _player = _playerParty[0];

    }

    private void Update()
    { 
        //플레이어 바꾸기
        if (Input.GetMouseButtonDown(0))
        {
            if (_ray_Test.Hit.collider != null)
            {
                if (_hit.collider == _ray_Test.Hit.collider)
                {
                    Debug.Log("리턴 때문에 못들어감");
                    return;
                }

                if (_ray_Test.Hit.collider.gameObject.CompareTag("Player"))
                {
                    _hit = _ray_Test.Hit;
                    _player = _hit.collider.gameObject.GetComponent<Player_Test>();
                    Debug.Log("스킬 플레이어 바꾸기");
                }
            }
        }

        //스킬 에드
        //SkillAdd함수와 그안에 index만 넣어주면 스킬 add가 된다
       //if (Input.GetKeyDown(KeyCode.Alpha7))
       //{
       //    Debug.Log("스킬 ADD");
       //    int moving = 2;
       //    SkillAdd(moving);
       //}

    }

    //스킬 add는 동료를 넣었을 때 - 이거 아님, 레벨업 했을 때, 무기를 바꿔 끼웠을 때
    //스킬 추가
    public void SkillAdd(int skillIndex)
    {
        if (_player.PlayerSkill.ContainsKey(skillIndex))
        {
            return;
        }

        _player.PlayerSkill.Add(skillIndex, Skill_List.SkillList[skillIndex]);
        _button.AddSkillButton(skillIndex);
    }


}

