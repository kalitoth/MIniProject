using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;

    private Vector3Int currentPos;
    void Start()
    {

        currentPos = _grid.WorldToCell(transform.position);
        transform.position = currentPos;
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
        currentPos += _grid.WorldToCell(Vector3Int.forward);
        transform.position = currentPos;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
        currentPos += _grid.WorldToCell(Vector3Int.right);
        transform.position = currentPos;
        }
    }
}
