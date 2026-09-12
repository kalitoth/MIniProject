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
    [Header("플레이어 체력바")]
    [SerializeField]
    Slider _playerHPBar;
    [SerializeField]
    TextMeshProUGUI _playerHPText;

    [Header("턴 버튼")]
    [SerializeField]
    Button _turnButton;
    
    [Header("플레이어 이미지")]
    [SerializeField]
    Image _playerImage;

    
    

    Player_Test _currentPlayer;


    [Header("스킬 AddList")]
    [SerializeField]
    Image _addSkillList;
    [Header("스킬 포인트")]
    [SerializeField]
    TextMeshProUGUI _skillPoint;
    bool _addSkill;


    //어떤 유닛을 클릭했을 때 hp
    // 상속이 필요하다 
    [Header("Any 유닛 체력")]
    [SerializeField]
    TextMeshProUGUI _textMeshPro;
    [SerializeField]
    Slider _anyUnitHPBar; 
    Unit_Test _anyUnit;

    PlayerShift _playerShift;
    Ray_UI _ray_Test;
    private RaycastHit _hit;

    [Header("옵션")]
    [SerializeField]
    Image _option;
    bool _timePause;

    [Header("사운드옵션")]
    [SerializeField]
    Image _soundOption;

    [Header("컨트롤키옵션")]
    [SerializeField]
    Image _controlKey;
    void Start()
    {
        _playerShift = GetComponent<PlayerShift>();
        _ray_Test = GetComponent<Ray_UI>();

        if (_playerHPBar == null)
        {
            Debug.Log("슬라이더 인스펙터 비어있다");
        }
        if (_ray_Test == null)
        {
            Debug.Log("ray_Test 인스펙터 비어있다");
        }
        

        _anyUnitHPBar.gameObject.SetActive(false);

        _currentPlayer = _playerShift.Player;

        if (_currentPlayer == null)
        {
            Debug.Log("현재 플레이어 비어있다");
        }

        _playerImage.sprite = _currentPlayer._image;

        _turnButton.onClick.AddListener(CurrentPlayerTurn);
    }
     
    void Update()
    {

        //ui 동기화
        if (_currentPlayer != _playerShift.Player)
        {   
            _currentPlayer = _playerShift.Player;
            //현재 유닛 이미지
            _playerImage.sprite = _currentPlayer._image;
        }
             //현재 선택된 캐릭터 hp
       _playerHPBar.value = (float)_currentPlayer.HP / _currentPlayer.MAXHP;
        _playerHPText.text = $"{_currentPlayer.HP} / {_currentPlayer.MAXHP}";

        //add스킬
        if (Input.GetKeyDown(KeyCode.K))
        {
            _addSkill = !_addSkill;

            _addSkillList.gameObject.SetActive(_addSkill);
        }

        //스킬 포인트
        _skillPoint.text = $"{_currentPlayer.skillPoint}";

        // ESC옵션
        if (!_currentPlayer.UnitState.HasFlag(Unit_Test.State.Skill))
        {
            
            if(!_soundOption.gameObject.activeSelf && !_controlKey.gameObject.activeSelf)
            {

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //동기화
                    if (_option.gameObject.activeSelf)
                    {
                        _timePause = true;
                    }
                    else
                    {
                        _timePause = false;
                    }

                    _timePause = !_timePause;

                    if (_timePause)
                    {
                        Time.timeScale = 0f;
                    }
                    else
                    {
                        Time.timeScale = 1f;
                    }

                    _option.gameObject.SetActive(_timePause);
                }

            }
            else if(_soundOption.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _soundOption.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
            else if(_controlKey.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _controlKey.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
            }

        }

        //아이템이면 파괴
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (_ray_Test.Hit.collider != null)
            {
                if(_ray_Test.Hit.collider.CompareTag("Item"))
                {
                    Destroy(_ray_Test.Hit.collider.gameObject);
                }
            }
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
    //현재 플레이어 turn 넘김 버튼
    void CurrentPlayerTurn()
    {
        if (_currentPlayer.UnitState.HasFlag(Unit_Test.State.Battle))
        {
            if (_currentPlayer.TurnEnable)
            {
                _currentPlayer.TurnEnd = true;
            }
        }

    }
}
