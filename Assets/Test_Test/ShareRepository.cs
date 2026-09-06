using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject Item
    {
        get { return _item; }
        set { _item = value; }
    }
    //소지품 들어갈 것 테스트
   //[SerializeField]
   //Image _image_test;
   //
   //TextMeshProUGUI _itemNum;

   // Image aaa;

    [SerializeField]
    protected Button _itembutton;

    //스켈레톤
    [Header("스켈레톤")]
    protected TextMeshProUGUI _skeletonGemNum;
    protected Button _skeletonGemButton;

    public int shareExp = 0;

    public int shareGold = 0;

    public int _skeletonGem = 0;

    public int potion = 0;

    bool _active;

    private void Update()
    {

        if(_inventory == null)
        {
            Debug.Log("인벤토리가 인스펙터에 없다");
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            _active = !_active;
            _inventory.gameObject.SetActive(_active);


            //인벤토리
            // 금화, 아이템, 포션, 장비
        }

    
        if (_skeletonGemNum == null && _skeletonGem != 0)
        {
            _skeletonGemButton = Instantiate(_itembutton, Item.transform); 
            _skeletonGemNum = _skeletonGemButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        if(_skeletonGemNum != null)
        {
            _skeletonGemNum.text = $"{_skeletonGem}";
        }
        

   
    }
}
