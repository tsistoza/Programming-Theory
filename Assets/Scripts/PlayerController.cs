using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Stats
    public float moveSpd = 1.0f;
    [SerializeField] private int playerHitPoints = 1;
    [SerializeField] private int playerMaxHp;
    [SerializeField] private int numBullets = 10;
    [SerializeField] private bool cooldownInvicibility;

    // Get Set
    public int PlayerMaxHp { get { return playerMaxHp; } set { playerMaxHp = value; } }
    public int PlayerHitPoints
    {
        get { return playerHitPoints; }
        private set { playerHitPoints = value; }
    }


    // Components
    private Rigidbody playerRb;
    public GameObject bulletPrefab;
    private ObjectPooler poolScript;
     
    void Start()
    {
        playerMaxHp = playerHitPoints;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        poolScript = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        poolScript.CreatePooledObjects(bulletPrefab, numBullets);
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.m_gameOver)
        {
            PlayerMovement();
        }
    }

    // class methods

    private void PlayerMovement()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        playerRb.velocity = new Vector3(dirX*moveSpd, playerRb.velocity.y, dirZ*moveSpd);
    }

    private void PlayerLife()
    {
        PlayerHitPoints--;
        if (PlayerHitPoints <= 0)
        {
            GameManager.Instance.m_gameOver = true;
            GameManager.Instance.GameOver();
        }
    }

    // Events
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !cooldownInvicibility)
        {
            PlayerLife();
            cooldownInvicibility = true;
            StartCoroutine(InvicibilityCooldown());
            Debug.Log("Player Hit by Enemy");
        }
    }
    IEnumerator InvicibilityCooldown()
    {
        yield return new WaitForSeconds(5);
        cooldownInvicibility = false;
    }
}
