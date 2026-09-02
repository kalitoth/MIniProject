using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Ray_Skill_SkillButton : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;

    public RaycastHit Hit
    {
        get { return _hit; }
    }
    Ray _ray;

    LayerMask _layer;

    float _distance = 500f;

    //현재 플레이어
    Player_Test _player;

    PlayerMovingShift _movingShift;
    private void Awake()
    {
        _layer = 1 << LayerMask.NameToLayer("Player");
    }
    void Start()
    {
        _movingShift = GetComponent<PlayerMovingShift>();

        if (_camera == null)
        {
            Debug.Log("레이에 카메라 인스펙터 없음");
        }
        if (_movingShift == null)
        {
            Debug.Log("무빙 시프트가 없다");
        }
         
    }
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _player = _movingShift.Player;

                RaycastHit hit;

                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(_ray, out hit, _distance, _layer);

                if (hit.collider != null)
                {
                   if (!_player._state.HasFlag(Player_Test.State.Skill))
                   {
                       _hit = hit;
                   }
                } 

            }
        }
    } 

    public void RayVisual()
    {
        Debug.DrawLine(_ray.origin, _hit.point, Color.red, 0.3f);
    }
}
