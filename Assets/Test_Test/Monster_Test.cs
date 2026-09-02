using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Test : Unit_Test
{
    //시야 직렬화
    [SerializeField]
    GameObject _monsterSight;

    GameObject _instSight;

    MonsterSight _getSight;

    MonsterState _state = MonsterState.None;
    enum MonsterState
    {
        None,
        Battle
    }
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


        if(_state == MonsterState.None)
        {
            if (_getSight._playerList.Count > 0)
            {

                _state = MonsterState.Battle;
           
            }
        }
         
        if(_state == MonsterState.Battle)
        {
            //Battle(_getSight._playerList);
        }
    }
    //플레이어 정보를 보내야함
    //몬스터가 많아지면 시야에 있는 것을 공유해야함 하나처럼
    //지금 몬스터 하나에서 순서를 정해서 배틀하는 것은 쉽다 문제는 여러마리일때 어떻게 해야하나
    //결국 어딘가로 보내서 정보를 합쳐야 한다 하지만 팀이 2개면 2팀의 배틀이 있어야 하는데?

   //void Battle(Dictionary<int, Player_Test> players)
   //{ 
   //   for(int i = 0; i < 4 ; i++)
   //   {
   //      if(players[i] == null)
   //       { 
   //           continue; 
   //       }
   //   
   //      int[] _speed = new int[4];
   //      _speed[i] = players[i].Speed;
   //   }
   //   
   //   
   //   
   //   //_characterController.velocity* Time.deltaTime
   //}

}
