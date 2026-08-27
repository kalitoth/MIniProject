using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Test : Unit_Test
{

    void Start()
    {
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;
    }

    
    void Update()
    {
        
    }
}
