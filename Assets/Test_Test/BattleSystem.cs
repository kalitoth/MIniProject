using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    List<Unit_Test> _battleList = new List<Unit_Test>(50);
    Dictionary<int, Unit_Test> _sortbattleList = new Dictionary<int, Unit_Test>(50);
    bool _sortTrigger = true;

    int _battleindex = 0;
    void Update()
    {
        //스피드 대로 순서 정렬
        if(_sortTrigger)
        {
            _sortTrigger = false;

            for (int i = 0; i < _battleList.Count; i++)
            {

            }
        }

        //딕셔너리에 집어넣기
        for(int i = 0; i < _battleList.Count; i++)
        {
            _sortbattleList.Add(i, _battleList[i]);
        }

        //전투 시작
        //턴 주기


        //죽거나 범위에 없으면 딕셔너리에서 빼기 
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_sortTrigger)
        {
            if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Player"))
            {
                _battleList.Add(other.GetComponent<Unit_Test>());
            }
            _battleindex += _battleList.Count;
        }
        else
        {
            if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Player"))
            {
                _sortbattleList.Add(_battleindex, other.GetComponent<Unit_Test>());
                _battleindex++;
            }
        }

        
    }
    //도망가거나 죽으면
    private void OnTriggerExit(Collider other)
    {
        
    }
}
