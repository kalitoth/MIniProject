using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerMovingShift : MonoBehaviour
{
    
    //초기 플레이어
    [SerializeField]
    PlayerMoving playerMoving;

    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;

    Ray _ray;

    LayerMask _layerMask;

    float _rayMaxDistance = 500f;
    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Ground");
        _layerMask += 1 << LayerMask.NameToLayer("Player");
    }
    void Start()
    {
       
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
                RayCamTo(out _hit, _layerMask);
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

    public void RayCamTo(out RaycastHit hit, LayerMask layerMask)
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out _hit, _rayMaxDistance, layerMask);
        hit = _hit;
    }
}
