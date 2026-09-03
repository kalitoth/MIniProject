using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Test : Unit_Test
{
    //시야 직렬화
    [SerializeField]
    GameObject _monsterSight;
    [SerializeField]
    GameObject _BattleSystem;

    GameObject _instSight;

    MonsterSight _getSight;

  
    private void Awake()
    {
        MAXHP = BasicHp + Mathf.FloorToInt((Constitution - 10) * 0.5f) * Level;
        HP = MAXHP;

        _instSight = Instantiate(_monsterSight, this.transform);
        Debug.Log("_monsterSight를 생성");
    }
    void Start()
    {
        _getSight = _instSight.gameObject.GetComponent<MonsterSight>();
         
        if(_getSight == null)
        {
            Debug.Log("_getSight가 null");
        }
        if (_getSight._playerList == null)
        {
            Debug.Log("_getSight._playerList가 null");
        }
    }

    
    void Update()
    {
        if(_getSight._playerList == null)
        {
            Debug.Log("_getSight._playerList가 null");
            return;
        }


        if(UnitState == State.None)
        {
            if (_getSight._playerList.Count > 0)
            {

                 Instantiate(_BattleSystem,transform.position,transform.rotation, transform);

                UnitState = State.Battle;
            }
        }
         
        if(UnitState == State.Battle)
        {
            if(TurnEnable)
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
