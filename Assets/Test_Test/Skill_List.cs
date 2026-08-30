using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 


public class Skill_List : MonoBehaviour
{
     

    Dictionary<int, Action<Player_Test,RaycastHit>> _skillList = new Dictionary<int, Action<Player_Test, RaycastHit>>();

    public Dictionary<int, Action<Player_Test, RaycastHit>> SkillList
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
         
    }
    #region 스킬 목록

    public void Attack(Player_Test player, RaycastHit _hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Monster"))
                {
                    Monster_Test monster = _hit.collider.gameObject.GetComponent<Monster_Test>();
                    monster.HP -= 1;

                    player._state = Player_Test.State.None;
                    player._lineRenderer.enabled = false;
                }

            }
        }
        //현재 플레이어와 현재 대상
        Debug.Log("Attack");
    }
    public void Defence(Player_Test player, RaycastHit _hit)
    {
        Debug.Log("Defence");
    }
    public void Moving(Player_Test player, RaycastHit _hit)
    {
        Debug.Log("Moving");
    }

    public void Fireball(Player_Test _player)
    {
        Debug.Log("파이어볼");
    }

    #endregion
}
