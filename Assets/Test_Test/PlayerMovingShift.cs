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
        _layerMask = 1 << LayerMask.NameToLayer("Player") ;
         
    }
    void Start()
    {
       
        if(playerMoving == null)
        {
            Debug.Log("무빙 시프트에 플레이어 무빙이 없다");
        }
        playerMoving.enabled = true;
    }
    public void Update()
    {
        MovingShift();
    }
    public void MovingShift()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                RayCamTo(out _hit, _layerMask);
                Debug.Log("무빙시프트에서 레이 발사");

                if (_hit.collider != null)
                {
                    if (_hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("여기 들어오나?");
                       if(playerMoving != null)
                       {
                           playerMoving.enabled = false;
                       }
                       playerMoving = _hit.collider.gameObject.GetComponent<PlayerMoving>();
                       playerMoving.enabled = true; 
                    }
                }

            }
        }

         
    }

    public void RayCamTo(out RaycastHit hit, LayerMask layerMask)
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out hit, _rayMaxDistance, layerMask);
         
    }
}
