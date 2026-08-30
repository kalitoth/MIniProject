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
        if(playerMoving == null)
        {
            Debug.Log("무빙 시프트에 플레이어 무빙이 없다");
        }
        
    }
    
    public void MovingShift()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray_Test.RayCamTo(out _hit);
                Debug.Log("무빙시프트에서 레이 발사");
            }
        }

        if (_hit.collider != null)
            {
                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    playerMoving = null;
                    playerMoving = _hit.collider.gameObject.GetComponent<PlayerMoving>();

                }
            }

        if (playerMoving == null)
        {
            return;
        }
        playerMoving.Hit = _hit;
        Debug.Log("무빙시프트에서 히트가 들어갔나?");
        if(_hit.collider == null)
        {
            Debug.Log("무빙시프트에서 히트콜라이더 가 널");
        }
    }

}
