using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{

    Transform _unit;
    float _forward = 7.5f;
    float _up = 0.7f;

    int dictionarySize = 4;

    public Dictionary<int, Player_Test> _playerList; 
    public Dictionary<Player_Test, int> _playerListRev; 

    int playerindex = 0;
     
    private void Awake()
    {
        _unit = transform.parent.GetComponent<Transform>(); 

        _playerList = new Dictionary<int, Player_Test>(dictionarySize);
        _playerListRev = new Dictionary<Player_Test, int>(dictionarySize);

        Debug.Log("몬스터 시야 생성");

    }

    private void Update()
    {
        transform.position = _unit.position + _unit.rotation * (Vector3.forward * _forward + Vector3.up * _up);

        if(_playerList.Count > 0)
        {
            for (int i = 0; i < dictionarySize; i++)
            {
                if(_playerList.ContainsKey(i))
                {
                    if(!_playerList[i].gameObject.activeSelf || _playerList[i].gameObject == null)
                    {
                        _playerListRev.Remove(_playerList[i]);
                        _playerList.Remove(i);
                    }
                    
                }
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Test currentPlayer = other.GetComponent<Player_Test>();

            for(int i = 0; i < dictionarySize; i++)
            {
                if (_playerList.ContainsKey(playerindex))
                {
                    playerindex++;

                    if (playerindex == 4)
                    {
                        playerindex = 0;
                    }
                }
                else
                {
                    _playerList.Add(playerindex, currentPlayer);
                    _playerListRev.Add(currentPlayer, playerindex);
                    Debug.Log($"현재 플레이어 Enter 인덱스 : {playerindex}");
                    Debug.Log($"현재 플레이어 Enter _playerList.Count : {_playerList.Count}");
                    
                    break;
                }
            }

        }
         
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Test currentPlayer = other.GetComponent<Player_Test>();

            int currentPlayerIndex = _playerListRev[currentPlayer];

            _playerList.Remove(currentPlayerIndex);
            _playerListRev.Remove(currentPlayer);

            Debug.Log($"현재 플레이어 Exit 인덱스 : {currentPlayerIndex}");
            Debug.Log($"현재 플레이어 Exit _playerList.Count : {_playerList.Count}");


        }
    }
}
