using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Modifiers
    [SerializeField] private int pos = 1;

    // Components
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        gameObject.transform.position = player.transform.position + (Vector3.up * pos);
    }
}
