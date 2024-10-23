using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Stats
    public float moveSpd;
    [SerializeField] private int playerHitPoints;
    [SerializeField] private int playerMaxHp;
    [SerializeField] private int numBullets;
    [SerializeField] private bool cooldownInvicibility;
    

    // Player Perks
    public List<Perk> perks;

    // Variables
    private int m_regenDelay;
    Cooldown m_regen;

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
        perks = new List<Perk>();
        m_regen = new Cooldown(10);
        playerMaxHp = playerHitPoints;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        poolScript = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        poolScript.CreatePooledObjects(bulletPrefab, numBullets);
    }

    private void Update()
    {
        PlayerRegen();
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

    private void PlayerDealDamage()
    {
        PlayerHitPoints--;
        if (PlayerHitPoints <= 0)
        {
            GameManager.Instance.m_gameOver = true;
            GameManager.Instance.GameOver();
        }
    }

    private void PlayerRegen ()
    {
        if (m_regen.Wait() && PlayerHitPoints < PlayerMaxHp)
        {
            PlayerHitPoints++;
            m_regen.Refresh();
        }
    }

    // Events
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !cooldownInvicibility)
        {
            PlayerDealDamage();
            cooldownInvicibility = true;
            m_regen.Refresh();
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
