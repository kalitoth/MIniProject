using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems; 
public class PlayerShift : MonoBehaviour
{
    
    //초기 플레이어 
    Player_Test _player;
    //이거 나중에 고치기
    //플레이어 4명일때 
    [SerializeField]
    Player_Test[] _playerParty = new Player_Test[4];
     
    //레이
    [SerializeField]
    private Camera _camera;
    private RaycastHit _playerShiftHit;
    private RaycastHit _SkillShiftHit;

    public RaycastHit SkillShiftHit
    {
        get { return _playerShiftHit; }
        set { _playerShiftHit = value; }
    }

    Ray _ray;
    LayerMask _layerMask;
    float _rayMaxDistance = 500f;

    //public Player_Test Player => _player;

    public Player_Test Player
    {
        get { return _player;}
        set { _player = value; }
    }

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
        _player = _playerParty[0];
        _player._playerMoving.enabled = true; 
    }
    public void Update()
    {
        MovingShift();
        ChangePlayer();

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
                RayCamTo(out _playerShiftHit, _layerMask);
                Debug.Log("무빙시프트에서 레이 발사");

                if (_playerShiftHit.collider != null)
                {
                    if (_playerShiftHit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("여기 들어오나?");
                       if(_player != null)
                       {
                            _player._playerMoving.enabled = false; 
                       }
                        _player = _playerShiftHit.collider.gameObject.GetComponent<Player_Test>(); 
                        _player._playerMoving.enabled = true;
                        Debug.Log($"무빙시프트의 플레이어 {_player}");
                    }
                }


                RaycastHit hit;

                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(_ray, out hit, _rayMaxDistance, _layerMask);

                if (hit.collider != null)
                {
                    Debug.Log("스킬 바꿈?");
                    if (!_player.UnitState.HasFlag(Unit_Test.State.Skill))
                    {
                        _SkillShiftHit = hit;
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

    void ChangePlayer()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {

            for (int i = 0; i < _playerParty.Length; i++)
            {
                if (_playerParty[i] == null)
                {
                    continue;
                }
                _playerParty[i]._playerMoving.enabled = true;
            }

            //_player = _player;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_playerParty[0] == null)
            {
                return;
            }
            for (int i = 0; i < _playerParty.Length; i++)
            {
                if (_playerParty[i] == null)
                {
                    continue;
                }
                _playerParty[i]._playerMoving.enabled = false;
            }
            _player = _playerParty[0];
            _player._playerMoving.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_playerParty[1] == null)
            {
                return;
            }

            for (int i = 0; i < _playerParty.Length; i++)
            {
                if (_playerParty[i] == null)
                {
                    continue;
                }
                _playerParty[i]._playerMoving.enabled = false;
            }
            _player = _playerParty[1];
            _player._playerMoving.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_playerParty[2] == null)
            {
                return;
            }

            for (int i = 0; i < _playerParty.Length; i++)
            {
                if (_playerParty[i] == null)
                {
                    continue;
                }
                _playerParty[i]._playerMoving.enabled = false;
            }
            _player = _playerParty[2];
            _player._playerMoving.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_playerParty[3] == null)
            {
                return;
            }

            for (int i = 0; i < _playerParty.Length; i++)
            {
                if (_playerParty[i] == null)
                {
                    continue;
                }
                _playerParty[i]._playerMoving.enabled = false;
            }
            _player = _playerParty[3];
            _player._playerMoving.enabled = true;
        }
    }
}
