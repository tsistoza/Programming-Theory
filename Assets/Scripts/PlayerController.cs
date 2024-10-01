using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Modifiers
    public float moveSpd = 1.0f;

    // Components
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");
        playerRb.AddForce(new Vector3(moveSpd * dirX, 0, moveSpd * dirZ));
    }
}
