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
    [SerializeField] private int damagePerBullet = 1;
    public int DamagePerBullet { 
        get { return damagePerBullet; } 
        private set {  damagePerBullet = value; }
    }
    [SerializeField] private bool cooldownInvicibility;

    // Components
    private Rigidbody playerRb;
    public GameObject bulletPrefab;
    private ObjectPooler poolScript;
    private AimController aimScript;
    [SerializeField] private GameObject bulletTransform;
     
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        poolScript = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        aimScript = gameObject.GetComponent<AimController>();
        bulletTransform = GameObject.Find("Bullet Transform");
        poolScript.CreatePooledObjects(bulletPrefab, numBullets);
    }

    void Update()
    {
        Fire();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    // class methods

    private void PlayerMovement()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        playerRb.velocity = new Vector3(dirX*moveSpd, playerRb.velocity.y, dirZ*moveSpd);
    }
    private void Fire()
    {
        Vector3 aim = aimScript.GetAimDirection().normalized;
        transform.forward = aim;
        bulletTransform.transform.position = transform.position + aim*2;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = poolScript.GetPooledObject();
            bullet.transform.position = bulletTransform.transform.position;
            bullet.SetActive(true);
            Debug.Log(aimScript.GetAimDirection());
            bullet.GetComponent<Rigidbody>().velocity = aimScript.GetAimDirection().normalized * bulletSpd;
            //bullet.GetComponent<Rigidbody>().AddForce(aimScript.GetAimDirection().normalized*bulletSpd, ForceMode.Impulse);
        }
    }

    private void PlayerLife()
    {
        PlayerHitPoints--;
        if (PlayerHitPoints <= 0)
        {
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
