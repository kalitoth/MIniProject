using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Test : MonoBehaviour
{
 
    //초상화 옆 플레이어 hp
    [SerializeField]
    Slider _playerHPBar;
    [SerializeField]
    Player_Test _currentPlayer;

    //어떤 유닛을 클릭했을 때 hp
    // 상속이 필요하다
    [SerializeField]
    Slider _anyUnitHPBar; 
    Unit_Test _anyUnit;

    [SerializeField]
    Ray_Test _ray_Test;
    private RaycastHit _hit;


    void Start()
    {
        if (_playerHPBar == null)
        {
            Debug.Log("슬라이더 인스펙터 비어있다");
        }
        if (_ray_Test == null)
        {
            Debug.Log("ray_Test 인스펙터 비어있다");
        }
        if (_currentPlayer == null)
        {
            Debug.Log("현재 플레이어 비어있다");
        }

        _anyUnitHPBar.gameObject.SetActive(false);
    }
     
    void Update()
    {   
        
        //ui 동기화
        //현재 선택된 캐릭터 hp
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);

                if (_hit.collider == null)
                {
                    return;
                }
                 
                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    
                    if (_hit.collider.gameObject.GetComponent<Player_Test>() == null)
                    {
                        Debug.Log("플레이어 컴포넌트 없음");
                        return;
                    }
                    _currentPlayer = _hit.collider.gameObject.GetComponent<Player_Test>();
                }
            }
        }
        _playerHPBar.value = (float)_currentPlayer.HP / _currentPlayer.MAXHP;


        //유닛 hp
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);

                if (_hit.collider == null)
                {
                    return;
                }

                _anyUnit = _hit.collider.gameObject.GetComponent<Unit_Test>();

                if (_anyUnit == null)
                {
                    _anyUnitHPBar.gameObject.SetActive(false);
                }
                else
                {
                    _anyUnitHPBar.gameObject.SetActive(true);
                }

                //if (_hit.collider.gameObject.CompareTag("Player") || _hit.collider.gameObject.CompareTag("Monster"))
                //{
                //    
                //    if (_hit.collider.gameObject.GetComponent<Unit_Test>() == null)
                //    {
                //        Debug.Log("유닛 컴포넌트 없음");
                //        return;
                //    }
                //    
                //}
               
            }
        }

        if(_anyUnit == null)
        {
            return;
        }    

        _anyUnitHPBar.value = (float)_anyUnit.HP / _anyUnit.MAXHP;

 
    }
}
