using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    PlayerShift _playerShift; 
    [SerializeField]  
    MakeSkillButton_Test _button;
    [SerializeField]
    Image _character;
    [SerializeField]
    ScrollRect _rect;
    [SerializeField]
    ShareRepository _shareRepository;

    bool _sortTrigger = true;
    bool _battleTrigger = true;
    bool triggerExit = true;
    int _battleListAdd = 0;
 
    List<Player_Test> _players = new List<Player_Test>(4);

   Dictionary<Collider, Unit_Test> _ColliderUnit = new Dictionary<Collider, Unit_Test>(20); 
   Dictionary< Unit_Test,Collider> _UnitCollider = new Dictionary<Unit_Test, Collider>(20); 
    List<Unit_Test> _battleList = new List<Unit_Test>(20);

    Dictionary<Collider,Image> _colliderImage = new Dictionary<Collider,Image>(20);

    int _battleIndex;
    private void Awake()
    {
        _battleIndex = 0;
        
    }
    
    void Update()
    {
        // 딕셔너리에서 꺼내서 넣기 
        //remove하면 당겨지니 뒤에서부터 해야함
        //스피드 대로 순서 정렬 > 오름차순으로
        // 배틀은 뒤에서부터

        if(_battleList.Count > 0)
        { 
            if (_sortTrigger)
            {
                _sortTrigger = false;

                _battleList.Sort(compare);
                     
            }
           
            //전투 중에 들어오면?


        //전투 시작
            if (_battleTrigger)
            {
                    //플레이어 전환
               for (int i = 0; i < _players.Count; i++)
               {
                    if(_battleList[_battleIndex].CompareTag("Monster"))
                    {
                        _button.SkillButtonInteractF(_players[i]);
                        break;
                    }
                    else
                    {
                        _button.SkillButtonInteractT(_players[i]);
                    }

                     
                    if (_battleList[_battleIndex] == _players[i])
                    {
                        Debug.Log($"배틀시스템에 플레이어가 들어감");

                        //플레이어 전환
                        _playerShift.Player = _players[i];
                        //이동
                        _players[i]._playerMoving.enabled = true;

                        //스킬 주체
                        _button.PlayerButton = _players[i];

                        //스킬 버튼
                        _button.ReviveSkillButton(_players[i]);
                        _button.SkillButtonInteractT(_players[i]);
                    }
                    else
                    {
                        _button.SkillButtonInteractF(_players[i]);
                        _button.RemoveSkillButton(_players[i]);
                        //이동
                        _players[i]._playerMoving.enabled = false;
                    }
                } 
            
                 Debug.Log($"배틀 인덱스{_battleIndex}");
                 Debug.Log($"배틀리스트 카운트 {_battleList.Count}");
                 
                 //턴 주기
                _battleList[_battleIndex].BattleTurnTrigger();
                _battleTrigger = false;
                
                triggerExit = true;

                
                

                

            }
            
            //BattleEnd가 true면 턴 넘어간다
            if (_battleList[_battleIndex].TurnEnd)
            {
                Debug.Log($"어디서 멈추나");
                _battleList[_battleIndex].TurnEnd = false;
                _battleTrigger = true;

                _battleIndex++;

               

                 Debug.Log($"턴 끝나고 배틀 인덱스{_battleIndex}");
                
                if (_battleIndex >= _battleList.Count)
                {
                    _battleIndex = 0;
                }

                


            }

            //죽었을 때
            for (int i = 0; i < _battleList.Count; i++)
            {
                if (!_battleList[i].Alive || _battleList[i].UnitState == Unit_Test.State.None)
                {
                    if(i <= _battleIndex)
                    {
                        _battleIndex--;
                    }

                    if(_battleList[i].CompareTag("Monster"))
                    {
                        _shareRepository.shareExp += _battleList[i].Exp;
                    }
                    Destroy(_colliderImage[_UnitCollider[_battleList[i]]].gameObject);
                    _colliderImage.Remove(_UnitCollider[_battleList[i]]);
                    _ColliderUnit.Remove(_UnitCollider[_battleList[i]]);
                    _UnitCollider.Remove(_battleList[i]);
                    _battleList.RemoveAt(i);

                }

                
            }
            //종료 조건
            if (_battleList.Count == _players.Count || _players.Count == 0)
            {
                if(_players.Count > 0)
                {
                    for (int i = 0; i < _players.Count; i++)
                    {
                        _button.SkillButtonInteractT(_players[i]);
                    }
                }    
                
                for (int i = 0; i < _battleList.Count; i++)
                {
                    _battleList[i].UnitState = Unit_Test.State.None;
                }

                for (int i = _battleList.Count-1; i >= 0; i--)
                {
                    if (_battleList[i].Alive)
                    {
                        Destroy(_colliderImage[_UnitCollider[_battleList[i]]].gameObject);
                        _colliderImage.Remove(_UnitCollider[_battleList[i]]);
                        _ColliderUnit.Remove(_UnitCollider[_battleList[i]]);
                        _UnitCollider.Remove(_battleList[i]);
                        _battleList.RemoveAt(i);
                    }
                }

                
                Destroy(this.gameObject);
            }

        }


        

    }

    private void OnTriggerEnter(Collider other)
    {
       
       if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Player"))
       {
           Unit_Test addUnit = other.GetComponent<Unit_Test>();
            _UnitCollider.Add(addUnit, other);
           _ColliderUnit.Add(other, addUnit);
           _battleList.Add(addUnit);
            Debug.Log($"온트리거 발동");
            addUnit.UnitState = Unit_Test.State.Battle;


            if(other.gameObject.CompareTag("Player"))
            {
                Player_Test addPlayer = addUnit.GetComponent<Player_Test>();

                if (addPlayer != null)
                {
                    _players.Add(addPlayer);
                }
            }

            //여기서 addUnit의 초상화 가져와서 ui만들기
            Image charcterImage = Instantiate(_character, _rect.content);
            charcterImage.sprite = addUnit._image;

            _colliderImage.Add(other, charcterImage);
       }  
        
    }
    //도망가거나 죽으면
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("트리거 엑시트 들어오나?");
        //_battleList[_battleList.IndexOf(_ColliderUnit[other])].TurnEnd = true;

        _ColliderUnit[other].BattleStart = false;
        _ColliderUnit[other].TurnEnd = true;
        _ColliderUnit[other].TurnEnable = false;
        _ColliderUnit[other].BattleReady = true;
        _ColliderUnit[other].UnitState = Unit_Test.State.None;

        
       

        if (_battleList.IndexOf(_ColliderUnit[other]) <= _battleIndex)
        {
            _battleIndex--;

            //트리거당 한번
            if (triggerExit)
            {
                _battleIndex++;
                triggerExit = false;
            }
        }

        

        if (other.gameObject.CompareTag("Player"))
        {
            Player_Test player = other.GetComponent<Player_Test>();
            player._playerMoving.enabled = false;

            
            _players.Remove(player);

            if (_players.Count > 0)
            {
                _button.RemoveSkillButton(player);
            }
            else
            {
               player._playerMoving.enabled = true;
            }
            _button.SkillButtonInteractT(player);
        }

        _battleList.Remove(_ColliderUnit[other]);
        _ColliderUnit.Remove(other);
        Destroy(_colliderImage[other].gameObject);
        _colliderImage.Remove(other);
        Debug.Log($"배틀리스트 카운트 {_battleList.Count}");
        

        _battleTrigger = true;


        if (_battleIndex >= _battleList.Count)
        {
            _battleIndex = 0;
        }

    }

    private int compare(Unit_Test x, Unit_Test y)
    {
        return (y.Speed -x.Speed);
    }

  


}
