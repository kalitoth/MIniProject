using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 
public class BattleSystem : MonoBehaviour
{
    
   
    bool _sortTrigger = true;
    bool _battleTrigger = true;

    int _battleListAdd = 0;
     

    Dictionary<Collider, Unit_Test> _ColliderUnit = new Dictionary<Collider, Unit_Test>(); 
    List<Unit_Test> _battleList = new List<Unit_Test>(20);

    int _battleIndex = 0;

    void Update()
    {
        // 딕셔너리에서 꺼내서 넣기 
        //remove하면 당겨지니 뒤에서부터 해야함
        //스피드 대로 순서 정렬 > 오름차순으로
        // 배틀은 뒤에서부터
        if (_sortTrigger)
        {
            _sortTrigger = false;

            _battleList.Sort(compare);
        }

        //전투 시작
        if (_battleTrigger)
        {
            Debug.Log($"배틀 인덱스{_battleIndex}");
            Debug.Log($"배틀리스트 카운트 {_battleList.Count}");
            //턴 주기
            _battleList[_battleIndex].BattleTurnTrigger();
            _battleTrigger = false;

        }

        //BattleEnd가 true면 턴 넘어간다
        if (_battleList[_battleIndex].BattleEnd)
        {
            _battleTrigger = true;
            _battleIndex++;
            if (_battleIndex >= _battleList.Count)
            {
                _battleIndex = 0;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
       
       if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Player"))
       {
           Unit_Test addUnit = other.GetComponent<Unit_Test>();

           _ColliderUnit.Add(other, addUnit);
           _battleList.Add(addUnit); 
            //여기서 addUnit의 초상화 가져와서 ui만들기
       }  
        
    }
    //도망가거나 죽으면
    private void OnTriggerExit(Collider other)
    {
        _battleList.Remove(_ColliderUnit[other]);
        _ColliderUnit.Remove(other);
    }

    private int compare(Unit_Test x, Unit_Test y)
    {
        return (x.Speed - y.Speed);
    }
}
