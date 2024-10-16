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
    [SerializeField] private int enemyHitpoints = 2;
    public int EnemyHitpoints {
        get { return enemyHitpoints; }
        set { enemyHitpoints = value; } 
    }

    // Components
    [SerializeField] private GunHandler gunScript;

    void Start()
    {
       gunScript = GameObject.Find("Player").GetComponent<GunHandler>();
        isGrounded = false;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.m_gameOver && isGrounded)
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
        EnemyHitpoints -= gunScript.DamagePerBullet;
        if (enemyHitpoints <= 0)
        {
            Destroy(gameObject);
        }
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
