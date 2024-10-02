using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Modifiers
    public float moveSpd = 1.0f;
    [SerializeField] private int playerHitPoints = 1;
    public int PlayerHitPoints
    {
        get { return playerHitPoints; }
        private set { playerHitPoints = value; }
    }

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

    // Events
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // PlayerHitPoints = PlayerHitPoints - 1;
            Debug.Log("Player Hit by Enemy");
        }
    }

    private void Fire()
    {

    }
}
