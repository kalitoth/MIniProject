using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
 

public class MonsterSight : MonoBehaviour
{

    Transform _parent;
    Unit_Test _unit;
    float _forward = 7.5f;
    float _up = 0.7f;

    int dictionarySize = 4;

    //public Dictionary<int, Player_Test> _playerList; 
    //public Dictionary<Player_Test, int> _playerListRev;

    public Dictionary<Collider, Player_Test> _player;
    Collider[] _removeList;

    LayerMask _layer;
    float _distance = 20f;
    RaycastHit hit;
    float _rayTime;
    float _rayTimeFrequency = 0.1f;
    private void Awake()
    {
        _parent = transform.parent.GetComponent<Transform>(); 
        _unit = _parent.GetComponent<Unit_Test>();

        _player = new Dictionary<Collider, Player_Test>(dictionarySize);
        _removeList = new Collider[dictionarySize];

        //_playerList = new Dictionary<int, Player_Test>(dictionarySize);
        //_playerListRev = new Dictionary<Player_Test, int>(dictionarySize);

        Debug.Log("몬스터 시야 생성");
        _layer = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        transform.position = _parent.position + _parent.rotation * (Vector3.forward * _forward + Vector3.up * _up);

        // if (_playerList.Count > 0)
        // {
        //     for (int i = 0; i < dictionarySize; i++)
        //     {
        //         if (_playerList.ContainsKey(i))
        //         { 
        //             if (!_playerList[i].Alive || _playerList[i].gameObject == null)
        //             {
        //                 Debug.Log("몬스터 시야가 먼저?");
        //                 _playerListRev.Remove(_playerList[i]);
        //                 _playerList.Remove(i);
        //             }
        //             
        //         }
        //     }
        // }

        if (_player.Count > 0)
        {
            int index = 0;
            foreach (KeyValuePair<Collider, Player_Test> player in _player)
            {
                if (!player.Value.Alive || player.Value.gameObject == null)
                {
                    _removeList[index] = player.Key;
                    index++;
                }
            }


            for (int i = 0; i < _removeList.Length; i++)
            {
                if (_removeList[i] == null)
                {
                    continue;
                }

                _player.Remove(_removeList[i]);
            }
        }

    }
 
    private void OnTriggerStay(Collider other)
    {
        if(_unit.UnitState.HasFlag(Unit_Test.State.Battle))
        {
            return;
        }
          
        if (other.CompareTag("Player"))
        {
            Debug.Log($"플레이어 감지?");
            //이거 왜 레이어 들어가면 안되지?
            Debug.DrawRay(_parent.position + Vector3.up, other.transform.position - _parent.position, Color.red, 0.3f);
            
            
            //주기
            _rayTime += Time.deltaTime;
            if (_rayTime < _rayTimeFrequency)
            {
                return;
            }
            _rayTime = 0;

            
            
            if (Physics.Raycast(_parent.position + Vector3.up, other.transform.position - _parent.position, out hit, _distance, _layer))
            { 
                if (!hit.collider.gameObject.CompareTag("Player"))
                { 
                    return;
                }
                
                Player_Test currentPlayer = other.GetComponent<Player_Test>();

                if (!currentPlayer.Alive)
                { 
                    return;
                } 

                if(_player.ContainsKey(other))
                {
                    return;
                }

                _player.Add(other,currentPlayer); 

            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("몬스터 시야에 딕셔너리 다시 들어감???");
        //Debug.Log($"콜라이더 감지?");
        //if (other.CompareTag("Player"))
        //{
        //    Debug.Log($"플레이어 감지?");
        //    //이거 왜 레이어 들어가면 안되지?
        //
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, other.transform.position - transform.position,out hit))
        //    {
        //        if(!hit.collider.CompareTag("Player"))
        //        {
        //            return;
        //        }
        //    Debug.Log($"레이캐스팅 감지?");
        //         Player_Test currentPlayer = other.GetComponent<Player_Test>();
        //        if(!currentPlayer.Alive)
        //        {
        //            Debug.Log("죽었을 때 들어오나?");
        //            return;
        //        }
        //         for (int i = 0; i < dictionarySize; i++)
        //         {
        //             if (_playerList.ContainsKey(playerindex))
        //             {
        //                 playerindex++;
        //        
        //                 if (playerindex == 4)
        //                 {
        //                     playerindex = 0;
        //                 }
        //             }
        //             else
        //             {
        //                 _playerList.Add(playerindex, currentPlayer);
        //                 _playerListRev.Add(currentPlayer, playerindex);
        //                 Debug.Log($"현재 플레이어 Enter 인덱스 : {playerindex}");
        //                 Debug.Log($"현재 플레이어 Enter _playerList.Count : {_playerList.Count}");
        //                 
        //                 break;
        //             }
        //         }
        //
        //    }
        //
        //}
         
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            _player.Remove(other);

           //Player_Test currentPlayer = other.GetComponent<Player_Test>();
           //
           //if(_playerListRev.ContainsKey(currentPlayer))
           //{
           //
           //    int currentPlayerIndex = _playerListRev[currentPlayer];
           //
           //    _playerList.Remove(currentPlayerIndex);
           //    _playerListRev.Remove(currentPlayer);
           //
           //    Debug.Log($"현재 플레이어 Exit 인덱스 : {currentPlayerIndex}");
           //    Debug.Log($"현재 플레이어 Exit _playerList.Count : {_playerList.Count}");
           //
           //}
            


        }
    }
}
