using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_List_Test : MonoBehaviour
{
    [SerializeField]
    Ray_Test ray_Test;
    private RaycastHit _hit;

    [SerializeField]
    Player_Test _currentPlayer;

    Player_Test _player;

    private void Start()
    {
        ray_Test = GetComponent<Ray_Test>();
        _currentPlayer = GetComponent<Player_Test>();
    }

    
    public void Attack(Player_Test player)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray_Test.RayCamTo(out _hit);

                if(_hit.collider != null && _hit.collider.gameObject.CompareTag("Monster"))
                {
                    Monster_Test monster = _hit.collider.gameObject.GetComponent<Monster_Test>();
                    monster.HP -= 1;

                    _currentPlayer._state = Player_Test.State.None;
                }
                
            }
        }
         //현재 플레이어와 현재 대상
        Debug.Log("Attack");
    }
    public void Defence()
    {
        Debug.Log("Defence");
    }
    public void Moving()
    {
        Debug.Log("Moving");
    }

     
}

