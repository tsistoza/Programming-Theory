using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Vector3 playerPos;

    [SerializeField] private float rotationSpd;
    [SerializeField] private Vector3 parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        parent = transform.parent.position;
        //transform.position = Vector3.Lerp(gameObject.transform.position, playerPos, rotationSpd);
        transform.RotateAround(parent, Vector3.up, rotationSpd * Time.deltaTime);
    }
}
