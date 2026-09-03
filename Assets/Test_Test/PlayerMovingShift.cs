using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems; 
public class PlayerMovingShift : MonoBehaviour
{
    
    //초기 플레이어 
    [SerializeField]
    Player_Test _player;
    //이거 나중에 고치기
    //플레이어 4명일때 
    [SerializeField]
    Player_Test[] _players = new Player_Test[4];

    //레이
    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;
    Ray _ray;
    LayerMask _layerMask;
    float _rayMaxDistance = 500f;

    public Player_Test Player => _player;

    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Player") ;
         
    }
    void Start()
    {
       
       if(_player == null)
       {
           Debug.Log("무빙 시프트에 플레이어가 없다");
       }
        _player._playerMoving.enabled = true; 
    }
    public void Update()
    {
        MovingShift();
    }
    public void MovingShift()
    {
        if(_player.UnitState.HasFlag(Unit_Test.State.Skill))
        {
            return;
        }

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
                       if(_player != null)
                       {
                            _player._playerMoving.enabled = false; 
                       }
                        _player = _hit.collider.gameObject.GetComponent<Player_Test>(); 
                        _player._playerMoving.enabled = true;
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
