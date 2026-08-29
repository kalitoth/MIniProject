using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerMovingShift : MonoBehaviour
{
    Ray_Test _ray_Test;
    //초기 플레이어
    [SerializeField]
    PlayerMoving playerMoving;

    RaycastHit _hit;

    void Start()
    {
        _ray_Test = GetComponent<Ray_Test>();

        if( _ray_Test == null )
        {
            Debug.Log("레이가 없다");
        }
        
    }
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);
            }
        }
            
    }
    public void MovingShift()
    {
        {
            if (_hit.collider != null)
            {
                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    playerMoving = _hit.collider.gameObject.GetComponent<PlayerMoving>();

                }
            }

        }

        if (playerMoving == null)
        {
            return;
        }
        playerMoving.Hit = _hit;
    }

}
