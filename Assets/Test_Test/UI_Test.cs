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
    Text _text;
    [SerializeField]
    TextMeshProUGUI _textMeshPro;
    [SerializeField]
    Button _turnButton;
    
    [SerializeField]
    Image _playerImage;

    [SerializeField]
    GameObject _inventory;
    [SerializeField]
    Image _image_test;
     

    Player_Test _currentPlayer;

    //어떤 유닛을 클릭했을 때 hp
    // 상속이 필요하다
    [SerializeField]
    Slider _anyUnitHPBar; 
    Unit_Test _anyUnit;

    PlayerShift _playerMovingShift;
    Ray_UI _ray_Test;
    private RaycastHit _hit;


    void Start()
    {
        _playerMovingShift = GetComponent<PlayerShift>();
        _ray_Test = GetComponent<Ray_UI>();

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

        _currentPlayer = _playerMovingShift.Player;
    }
     
    void Update()
    {

        //ui 동기화
        //현재 선택된 캐릭터 hp
        _currentPlayer = _playerMovingShift.Player;
        _playerHPBar.value = (float)_currentPlayer.HP / _currentPlayer.MAXHP;
        //_text.text = "HP" + (float)_currentPlayer.HP / _currentPlayer.MAXHP;
        _turnButton.onClick.AddListener(CurrentPlayerTurn);
       
        //현재 플레이어 turn 넘김 버튼
        void CurrentPlayerTurn()
        {
            if(_currentPlayer.TurnEnable)
            {
                _currentPlayer.TurnEnd = true;
            }
            
        }
        //현재 유닛 이미지
        _playerImage.sprite = _currentPlayer._image;

        if(Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(_image_test, _inventory.transform);
        }    
        
        //유닛 hp
        if (!EventSystem.current.IsPointerOverGameObject())
        {
           if (_ray_Test.Hit.collider == null)
           {
               return;
           }

           _anyUnit = _ray_Test.Hit.collider.gameObject.GetComponent<Unit_Test>();

           if (_anyUnit == null)
           {
               _anyUnitHPBar.gameObject.SetActive(false);
           }
           else
           {
               _anyUnitHPBar.gameObject.SetActive(true);
           }
        }
        
          
        if(_anyUnit == null)
        {
            return;
        }    

        _anyUnitHPBar.value = (float)_anyUnit.HP / _anyUnit.MAXHP;
        _textMeshPro.text = $"{(float)_anyUnit.HP} / {_anyUnit.MAXHP}";
    }
}
