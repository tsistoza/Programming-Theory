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
    [SerializeField] private int numBullets = 10;
    [SerializeField] private int bulletSpd = 1;

    // Components
    private Rigidbody playerRb;
    public GameObject bulletPrefab;
    private ObjectPooler poolScript;
    private AimController aimScript;
     
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        poolScript = gameObject.GetComponent<ObjectPooler>();
        aimScript = gameObject.GetComponent<AimController>();
        poolScript.CreatePooledObjects(bulletPrefab, numBullets);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Fire();
    }

    // class methods

    private void PlayerMovement()
    {
        playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirZ = Input.GetAxisRaw("Vertical");
        playerRb.AddForce(new Vector3(moveSpd * dirX, 0, moveSpd * dirZ));
    }
    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = poolScript.GetPooledObject();
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            Debug.Log(aimScript.GetAimDirection());
            bullet.GetComponent<Rigidbody>().AddForce(aimScript.GetAimDirection()*bulletSpd);
        }
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
}
