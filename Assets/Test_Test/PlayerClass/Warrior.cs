using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Player_Test
{

    Dictionary<int, Action> _AddSkill = new Dictionary<int, Action>();
     
    List<Button> _addSkillButtonWarrior = new List<Button>(10);
      
    public Dictionary<int, Action> AddSkill
    {
        get { return _AddSkill; }
        set { _AddSkill = value; }
    }

    public List<Button> AddSkillButtonWarrior
    {
        get { return _addSkillButtonWarrior; }
        set { _addSkillButtonWarrior = value; }
    }
    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    //protected override void Start()
    //{
    //    base.Start();
    //}


}
