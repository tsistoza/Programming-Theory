using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Test();
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

    private void Aim ()
    {
        Vector3 position = GetMousePosition();
        if (position != Vector3.zero)
        {
            Vector3 direction = position - transform.position;
            direction.y = -192;
            transform.forward = direction;
        }
    }
    
    // Visualizing RayCasting
    private void Test()
    {
        transform.position = GetMousePosition();
    }
}
