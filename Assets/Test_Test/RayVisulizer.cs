using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayVisulizer : MonoBehaviour
{

    void Start()
    {
        
    }

    
    void Update()
    {
       
    }
    
    public void RayVisual(Ray ray, RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
             
            Debug.DrawLine(ray.origin, hit.point,Color.red,0.3f);
        }
    }
}
