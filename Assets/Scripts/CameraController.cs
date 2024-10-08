using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Modifiers
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private float movementSpd;

    // Components
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position,
            player.transform.position + targetOffset, movementSpd*Time.deltaTime);
    }
}
