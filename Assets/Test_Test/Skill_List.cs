using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;


public class Skill_List : MonoBehaviour
{
    Ray_Test _ray_Test;
    RaycastHit _hit;

    Dictionary<int, Action<Player_Test>> _skillList = new Dictionary<int, Action<Player_Test>>();

    public Dictionary<int, Action<Player_Test>> SkillList
    {
        get { return _skillList; }
    }

    private void Awake()
    {
        _skillList.Add(0, Attack);
        _skillList.Add(1, Defence);
        _skillList.Add(2, Moving);
    }
    private void Start()
    {
        _ray_Test = GetComponent<Ray_Test>();
        if( _ray_Test == null )
        {
            Debug.Log("스킬 목록에 레이캐스트가 없다");
        }

    }
    #region 스킬 목록

    public void Attack(Player_Test player)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);

                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Monster"))
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

    public void Fireball(Player_Test _player)
    {
        Debug.Log("파이어볼");
    }

    #endregion
}
