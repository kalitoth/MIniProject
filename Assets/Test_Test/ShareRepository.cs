using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShareRepository : MonoBehaviour
{
    //소지품창
    [SerializeField]
    Image _inventory;

    //골드
    [SerializeField]
    TextMeshProUGUI _gold;

    //소지품
    [SerializeField]
    GameObject _item;

    [Header("레벨업")]
    [SerializeField]
    Button _levelup;
    [SerializeField]
    Slider _expSlider;
    [SerializeField]
    TextMeshProUGUI _expText;


    public GameObject Item
    {
        get { return _item; }
        set { _item = value; }
    }


    [SerializeField]
    protected Button _skeletonItem;
    [SerializeField]
    protected Button _lichItem;

    //스켈레톤
    [Header("스켈레톤")]
    protected TextMeshProUGUI _skeletonGemNum;
    protected Button _skeletonGemButton;
    public int _skeletonGem = 0;
     
    [Header("리치")]
    protected TextMeshProUGUI _lichItemText;
    protected Button _lichItemButton;
    public int _lichItemNum = 0;

    public int shareExp = 0;

    public int shareGold = 0;


    public int potion = 0;

    bool _inventoryActive;


    [Header("현재 플레이어")]
    [SerializeField]
    PlayerShift _playerShift;


    [SerializeField]
    CanvasGroup _canvasGroup;

    public int _playerLife;

    float _time;
    float _duration = 1f;
    float _interpolate;
    bool introOneShot = true;
    private void Start()
    {
        _levelup.onClick.AddListener(LevelUp);

        for(int i  = 0; i < 4; i++)
        {
            if(_playerShift.PlayerParty[i] != null)
            {
                _playerLife++;
            }
        } 
    }

    private void Update()
    {

        if(_inventory == null)
        {
            Debug.Log("인벤토리가 인스펙터에 없다");
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            _inventoryActive = !_inventoryActive;
            _inventory.gameObject.SetActive(_inventoryActive);
        }

        
            if (_skeletonGemNum == null && _skeletonGem != 0)
            {
                _skeletonGemButton = Instantiate(_skeletonItem, Item.transform); 
                _skeletonGemButton.onClick.AddListener(HPRecovery);
                _skeletonGemNum = _skeletonGemButton.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (_skeletonGemButton != null)
            {
                if (_skeletonGem <= 0)
                {
                    _skeletonGemButton.gameObject.SetActive(false);
                }
                else
                {
                    _skeletonGemButton.gameObject.SetActive(true);
                }

                if (_skeletonGemNum != null)
                {
                    _skeletonGemNum.text = $"{_skeletonGem}";
                }

            }

            if (_lichItemText == null && _lichItemNum != 0)
            {
                 _lichItemButton = Instantiate(_lichItem, Item.transform);
                 _lichItemButton.onClick.AddListener(GameEnd);
                 _lichItemText = _lichItemButton.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (_lichItemButton != null)
            {
                if (_lichItemNum <= 0)
                {
                _lichItemButton.gameObject.SetActive(false);
                }
                else
                {
                _lichItemButton.gameObject.SetActive(true);
                }

                if (_lichItemText != null)
                {
                _lichItemText.text = $"{_lichItemNum}";
                }

            }


        if(_playerLife <= 0 && introOneShot)
        {
           StartCoroutine(IntroFadeout("Intro"));
            introOneShot = false;
        }

        //골드
        _gold.text = $"Gold : {shareGold}";

        //경험치
        _expSlider.value = (float)shareExp / _playerShift.Player.MaxExp;
        _expText.text = $"{shareExp}/{_playerShift.Player.MaxExp}";

        if(shareExp >= _playerShift.Player.MaxExp)
        {
            _levelup.gameObject.SetActive(true);
        }
        else
        {
            _levelup.gameObject.SetActive(false);
        }
         
    }
    public void GameEnd()
    {
        StartCoroutine(IntroFadeout("Intro"));
    }

    public void LevelUp()
    {
        if (shareExp >= _playerShift.Player.MaxExp)
        {
           
            _playerShift.Player.Level++;
            _playerShift.Player.MaxExp += _playerShift.Player.MaxExp * _playerShift.Player.Level;
            //스텟 선택 
            _playerShift.Player.skillPoint++;
            Debug.Log("레벨업");
           
        }
    }
    public void HPRecovery()
    {
        if (_playerShift.Player != null)
        {
            if (_playerShift.Player.HP < _playerShift.Player.MAXHP && _skeletonGem > 0)
            {
                _playerShift.Player.HP += 10;
                _skeletonGem -= 1;
            }
        }
        
    }


    private IEnumerator IntroFadeout(string SceneName)
    {
        //프레임 따라 가는 게 아니라 시간 따라 가야 한다

       
        _canvasGroup.alpha = 0f;
        //_time = Time.realtimeSinceStartup;
        _canvasGroup.blocksRaycasts = true;

        while (_time < _duration)
        {

            _time += Time.unscaledDeltaTime;

            _interpolate = Mathf.Clamp01(_time / _duration);

            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, _interpolate);
            yield return null;

            Debug.Log($"들어오고 있나?");
            Debug.Log($"시간 : {_time}");
            Debug.Log($"듀레이션 : {_duration}");
            Debug.Log($"인터폴레이트 : {_interpolate}");

        }

        _time = 0f;
        
         SceneManager.LoadSceneAsync(SceneName); 

         


    }
}
