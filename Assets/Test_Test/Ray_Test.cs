using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ray_Test : MonoBehaviour
{

    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;

    public RaycastHit Hit
    {
        get { return _hit; }
    }
    Ray _ray;

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

        Physics.Raycast(_ray, out _hit);

        hit = _hit;
    }

    public void RayVisual()
    { 
       Debug.DrawLine(_ray.origin, _hit.point, Color.red, 0.3f);
    }
}
