using Palmmedia.ReportGenerator.Core.Parser.Analysis;
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
using static UnityEditor.Experimental.GraphView.GraphView;

public class Skill_List_Test : MonoBehaviour
{ 
    Ray_Test _ray_Test;
    private RaycastHit _hit;
     
    // 최대 4인팟 
    //일단 처음에 정하는 걸로
    [SerializeField]
    Player_Test[] _playerParty = new Player_Test[4];

    //초기 플레이어
    Player_Test _player;

    MakeSkillButton_Test _button;

    Skill_List skill_List = new Skill_List();
    //이게 없으면 무기나 스킬 얻는 것마다 add를 해줘야 한다 << 이거 player필요함 << 결합도 올라감
    //스킬 목록
    Dictionary<int, Action<Player_Test>> _skillList = new Dictionary<int, Action<Player_Test>>();

    
    private void Awake()
    {
        foreach (Player_Test player in _playerParty)
        {
            if (player != null)
            { 
                AddBasicSkill(player);
            }
        }

        _skillList.Add(0, Attack);
        _skillList.Add(1, Defence);
        _skillList.Add(2, Moving);
       //Type type = skill_List.GetType();
       //Debug.Log($"{type}");
       //MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic);
       //foreach (MethodInfo method in methods)
       //{
       //   //_skillList.Add(1, method);
       //}
    }

    private void Start()
    {
        _button = GetComponent<MakeSkillButton_Test>();
        _ray_Test = GetComponent<Ray_Test>();

        if(_button == null)
        {
            Debug.Log("스킬 리스트에 버튼이 없다");
        }
        if(_ray_Test == null)
        {
            Debug.Log("스킬 리스트에 레이가 없다");
        }

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
        //SkillAdd 와 index 함수만 넣어주면 스킬 add가 된다
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("스킬 ADD");
            int moving = 2;
            SkillAdd(moving);
        }

    }

    //스킬 add는 동료를 넣었을 때 - 이거 아님, 레벨업 했을 때, 무기를 바꿔 끼웠을 때
    //스킬 추가
    public void SkillAdd(int skillIndex)
    {
        if (_player.PlayerSkill.ContainsKey(skillIndex))
        {
            return;
        }

        _player.PlayerSkill.Add(skillIndex, _skillList[skillIndex]);
        _button.AddSkillButton(skillIndex);
    }

    //기본 스킬
    void AddBasicSkill(Player_Test player)
    {
        player.PlayerSkill.Add(0, Attack);
        player.PlayerSkill.Add(1, Defence);
        Debug.Log("스킬이 들어갔나?");
    }
    
    #region 스킬 목록
     
    public void Attack(Player_Test player)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);

                if(_hit.collider != null && _hit.collider.gameObject.CompareTag("Monster"))
                {
                    Monster_Test monster = _hit.collider.gameObject.GetComponent<Monster_Test>();
                    monster.HP -= 1;

                    player._state = Player_Test.State.None;
                }
                
            }
        }
         //현재 플레이어와 현재 대상
        Debug.Log("Attack");
    }
    public void Defence(Player_Test player)
    {
        Debug.Log("Defence");
    }
    public void Moving(Player_Test player)
    {
        Debug.Log("Moving");
    }

    #endregion
}

