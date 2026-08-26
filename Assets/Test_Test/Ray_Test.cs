using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray_Test : MonoBehaviour
{

    [SerializeField]
    private Camera _camera;
    private RaycastHit _hit;
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
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
    }
    public void RayCamTo(out RaycastHit hit)
    {
        Physics.Raycast(_ray, out _hit);

        hit = _hit;
    }

    public void RayVisual()
    { 
        if (Input.GetMouseButtonDown(0))
        {

            Debug.DrawLine(_ray.origin, _hit.point, Color.red, 0.3f);
        }
    }
}
