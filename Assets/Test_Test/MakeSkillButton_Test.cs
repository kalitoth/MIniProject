using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MakeSkillButton_Test : MonoBehaviour
{
    [Header("버튼 인스펙터")]
    [SerializeField]
    Button _button;
    [SerializeField]
    private ScrollRect _scrollRect;

     
    [Header("버튼 스킬 저장소")]
    Dictionary<int, UnityEngine.Events.UnityAction> _skillAction = new Dictionary<int, UnityEngine.Events.UnityAction>();
    Dictionary<int, Sprite> _skillsprites = new Dictionary<int, Sprite>();

    [Header("버튼 오브젝트 관리")]
    List<Button> _skillButton = new List<Button>(10);

    //플레이어 스킬
    [SerializeField]
    Player_Test _player;

    private void Awake()
    { 

        //버튼 저장소에 미리 저장해 놓는다
        _skillAction.Add(0, Attack);
        _skillsprites.Add(0, Resources.Load<Sprite>("Attack"));
        _skillAction.Add(1, Defence);
        _skillsprites.Add(1, Resources.Load<Sprite>("Defence"));
        _skillAction.Add(2, Moving);
        _skillsprites.Add(2, Resources.Load<Sprite>("Moving"));

         
    }
    void Start()
    {

        MakeSkillTree();


    }
    
    void Update()
    {
        

    }

    
  void MakeSkillTree()
  {

        foreach (KeyValuePair<int, Action<Player_Test>> skill in _player.PlayerSkill)
        {
            Button insbutton = Instantiate(_button, _scrollRect.content);


            _skillButton.Add(insbutton);

            insbutton.onClick.AddListener(_skillAction[skill.Key]);
            insbutton.image.sprite = _skillsprites[skill.Key];

        }
        Debug.Log("버튼이 생성됐나?");
    }

    #region 옵션
    void ListChange()
    {
        Button temp = _skillButton[0];
        _skillButton[0] = _skillButton[1];
        _skillButton[1] = temp;

        _skillButton[0].transform.SetSiblingIndex(0);

    }

    void RemoveSkillButton()
    {
        for (int i = 0; i < _skillButton.Count; i++)
        {
            _skillButton[i].gameObject.SetActive(false);

        }
    }

    void ReviveSkillButton()
    {
        for (int i = 0; i < _skillButton.Count; i++)
        {
            _skillButton[i].gameObject.SetActive(true);

        }
    }

    public void AddSkillButton()
    { 
        Button insbutton = Instantiate(_button, _scrollRect.content);
    
        _skillButton.Add(insbutton);
    
        insbutton.onClick.AddListener(_skillAction[1]);//스킬 리스트 add함수에서 키값 넣기
        insbutton.image.sprite = _skillsprites[1];
    }
    #endregion


    #region 버튼 목록

    void Attack()
    {
        _player._state = Player_Test.State.Skill;
        _player._skillIndex = 0;
    }
    void Defence()
    {
        _player._state = Player_Test.State.Skill;
        _player._skillIndex = 1;
    }

    void Moving()
    {
        _player._state = Player_Test.State.Skill;
        _player._skillIndex = 2;
    }
    #endregion
}

