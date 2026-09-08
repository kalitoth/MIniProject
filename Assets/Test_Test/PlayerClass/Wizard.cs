using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
public class Wizard : Player_Test
{


    Dictionary<int, Action> _AddSkill = new Dictionary<int, Action>();

    List<Button> _addSkillButtonWizard = new List<Button>(10);
 

    public Dictionary<int, Action> AddSkill
    {
        get { return _AddSkill; }
        set { _AddSkill = value; }
    }
    public List<Button> AddSkillButtonWizard
    {
        get { return _addSkillButtonWizard; }
        set { _addSkillButtonWizard = value; }
    }


}
