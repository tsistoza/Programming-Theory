using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// All Enemies will inherit this class
/// </summary>
public class Enemy : MonoBehaviour
{
    // Modifiers
    [SerializeField] private float enemySpd = 1.0f;
    private bool isGrounded;

    public float EnemySpd { 
        get { return enemySpd; }
        set { enemySpd = value; } 
    }
    [SerializeField] private int enemyHitpoints;
    public int EnemyHitpoints {
        get { return enemyHitpoints; }
        set { enemyHitpoints = value; } 
    }

    // Components
    [SerializeField] private GunHandler gunScript;
    [SerializeField] private PerkHandler perkScript;
    public List<Perk> perks;

    private bool isPoisoned;
    Cooldown duration;
    Cooldown timer;

    void Start()
    {
        gunScript = GameObject.Find("Player").GetComponent<GunHandler>();
        perkScript = GameObject.Find("Player").GetComponent<PerkHandler>();
        duration = new Cooldown((float)perkScript.PoisonDuration);
        timer = new Cooldown((float)2f);
        isGrounded = false;
        EnemyHPScaling();
    }

    private void Update()
    {
        if (isPoisoned)
        {
            PoisonDamage(duration, timer);
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.m_gameOver && isGrounded && !MenuMain.Instance.paused)
        {
            EnemyMove();
        }
    }


    // Class Methods
    private void EnemyMove()
    {
        Rigidbody enemyRb = gameObject.GetComponent<Rigidbody>();
        enemyRb.velocity = new Vector3(0, enemyRb.velocity.y, 0);
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        Vector3 enemyMoveForce = new Vector3(
            playerPos.x - gameObject.transform.position.x,
            0,
            playerPos.z - gameObject.transform.position.z);
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        gameObject.GetComponent<Rigidbody>().AddForce(enemyMoveForce.normalized * EnemySpd, ForceMode.Impulse);
    }

    public void DealDamage()
    {
        perks = GameObject.Find("Player").GetComponent<PlayerController>().perks;
        EnemyHitpoints -= gunScript.DamagePerBullet;

        if (!isPoisoned && perks.Contains(Perk.Poison_Perk))
        {
            duration.Refresh();
            isPoisoned = true;
        }

        if (enemyHitpoints <= 0) Destroy(gameObject);
    }

    public void PoisonDamage(Cooldown duration, Cooldown timer)
    {
        if (duration.Wait()) isPoisoned = false;
        if (timer.Wait())
        {
            Debug.Log("Enemy Ticked From Poison");
            EnemyHitpoints -= perkScript.PoisonDamage;
        }
        if (EnemyHitpoints <= 0) Destroy(gameObject);
    }

    public void EnemyHPScaling()
    {
        int addHp = Mathf.RoundToInt(((EnemyHitpoints*GameManager.Instance.WaveNumber)*0.5f));
        Debug.Log(addHp);
        EnemyHitpoints += addHp;
        return;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Walls")) isGrounded = true;
 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Walls")) isGrounded = false;
    }

}
