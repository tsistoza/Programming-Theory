using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    // Components
    private Camera mainCam;
    public LayerMask groundMask;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return hitInfo.point;
        } else
        {
            return Vector3.zero;
        }
    }

    // Get aim direction from 
    public Vector3 GetAimDirection ()
    {
        Vector3 position = GetMousePosition();
        if (position != Vector3.zero)
        {
            Vector3 bulletDirection = position - transform.position;
            bulletDirection.y = 0;
            return bulletDirection;
        }
        return Vector3.zero;
    }
    
    // Visualizing RayCasting
    private void Test()
    {
        transform.position = GetMousePosition();
    }
}
