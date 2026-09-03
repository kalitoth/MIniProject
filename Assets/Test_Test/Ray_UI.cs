using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ray_UI : MonoBehaviour
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
    private void Awake()
    {
        _layer = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster") | 1 <<LayerMask.NameToLayer("Ground");
    }
    void Start()
    {
        if(_camera == null)
        {
            Debug.Log("레이에 카메라 인스펙터 없음");
        }
    }
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                RayCamTo(out _hit);
                RayVisual();
            }
        }
    }
    public void RayCamTo(out RaycastHit hit)
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out hit, _distance, _layer); 
    }
    

    public void RayVisual()
    { 
       Debug.DrawLine(_ray.origin, _hit.point, Color.red, 0.3f);
    }
}
