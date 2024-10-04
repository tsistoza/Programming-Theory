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
    public float EnemySpd
    {
        get { return enemySpd; }set { enemySpd = value; }
    }
    [SerializeField] private int enemyHitpoints = 1;
    public int EnemyHitpoints
    {
        get { return enemyHitpoints; } set {  enemyHitpoints = value; }
    }

    // Variables


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
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
        gameObject.GetComponent<Rigidbody>().AddForce(enemyMoveForce.normalized * EnemySpd);
    }
}
