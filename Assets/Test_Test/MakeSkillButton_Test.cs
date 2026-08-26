using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSkillButton_Test : MonoBehaviour
{
    [Header("버튼 인스펙터")]
    [SerializeField]
    Button _button;
    [SerializeField]
    private ScrollRect _scrollRect;

    Dictionary<int, UnityEngine.Events.UnityAction> _skillAction = new Dictionary<int, UnityEngine.Events.UnityAction>();

    Dictionary<UnityEngine.Events.UnityAction, Sprite> _skillsprites = new Dictionary<UnityEngine.Events.UnityAction, Sprite>();

    List<Button> _skillButton = new List<Button>(10);

    Skill_List_Test _skill_test;

    private void Awake()
    {
        _skill_test = GetComponent<Skill_List_Test>();
    }
    void Start()
    { 
        _skillAction.Add(0, _skill_test.Attack);
        _skillAction.Add(1, _skill_test.Defence);
        _skillsprites.Add(_skill_test.Attack, Resources.Load<Sprite>("Attack"));
        _skillsprites.Add(_skill_test.Defence, Resources.Load<Sprite>("Defence"));

        MakeSkillTree(); 
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveSkill();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ReviveSkill();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddSkill();
        }
    }

    void RemoveSkill()
    {
        for (int i = 0; i < _skillAction.Count; i++)
        {
            _skillButton[i].gameObject.SetActive(false);

        }
    }

    void ReviveSkill()
    {
        for (int i = 0; i < _skillAction.Count; i++)
        {
            _skillButton[i].gameObject.SetActive(true);

        }
    }

    void AddSkill()
    {


        _skillAction.Add(_skillAction.Count, _skill_test.Moving);
        _skillsprites.Add(_skillAction[_skillAction.Count-1], Resources.Load<Sprite>("Moving"));

        Button insbutton = Instantiate(_button, _scrollRect.content);

        _skillButton.Add(insbutton);

        insbutton.onClick.AddListener(_skillAction[_skillAction.Count - 1]);
        insbutton.image.sprite = _skillsprites[_skillAction[_skillAction.Count - 1]];
    }
    void MakeSkillTree()
    {
        for (int i = 0; i < _skillAction.Count; i++)
        {

            Button insbutton = Instantiate(_button, _scrollRect.content);

            _skillButton.Add(insbutton);

            insbutton.onClick.AddListener(_skillAction[i]);
            insbutton.image.sprite = _skillsprites[_skillAction[i]];

        }

    }
}

