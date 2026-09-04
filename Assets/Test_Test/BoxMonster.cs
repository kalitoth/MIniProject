using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMonster : Monster_Test
{

    void Awake()
    {
        // 여기에 몬스터 스텟 넣기
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;
    }

    
    protected override void Update()
    {
        base.Update();

        if (UnitState.HasFlag(State.Battle))
        {
            if (TurnEnable)
            {
                //여기에 ai
                Debug.Log("몬스터 행동");


                TurnEnable = false;
                TurnEnd = true;
                Debug.Log("몬스터 턴 끝");
            }

        }
    }
}
