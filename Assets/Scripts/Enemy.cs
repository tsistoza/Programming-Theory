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
    [SerializeField] private PlayerController controlScript;

    void Start()
    {
       controlScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.m_gameOver)
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
        EnemyHitpoints -= controlScript.DamagePerBullet;
        if (enemyHitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyWithId(int index)
    {
        switch(index)
        {
            case 1: EnemyHitpoints = 2; EnemySpd = 4; break;
            case 2: EnemyHitpoints = 3; EnemySpd = 4; break;
            default: EnemyHitpoints = 1; EnemySpd = 8; break;
        }
    }
}
