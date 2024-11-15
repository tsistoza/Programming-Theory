using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// All Enemies will inherit this class
/// </summary>
public class Enemy : MonoBehaviour
{
    // Modifiers
    [SerializeField] private float enemySpd = 1.0f;
    private bool isGrounded;
    private Node currentNode;
    private Node endNode;
    [SerializeField] private float turnDst = 5.0f;
    [SerializeField] private float turnSpeed = 3.0f;
    [SerializeField] private float minPathUpdateTime = 0.1f;
    [SerializeField] private float pathUpdateMoveThreshold = 0.4f;

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
    [SerializeField] private NodeGrid nodegrid;
    [SerializeField] private GameObject Player;
    private Vector3[] waypoints;
    private int targetIndex;
    private IEnumerator followPathCoroutine;

    private bool isPoisoned;
    Cooldown duration;
    Cooldown timer;

    void Start()
    {
        gunScript = GameObject.Find("Player").GetComponent<GunHandler>();
        perkScript = GameObject.Find("Player").GetComponent<PerkHandler>();
        nodegrid = GameObject.Find("A*").GetComponent<NodeGrid>();
        Player = GameObject.Find("Hider");
        duration = new Cooldown((float)perkScript.PoisonDuration);
        timer = new Cooldown((float)2f);
        isGrounded = false;
        EnemyHPScaling();
        //StartCoroutine(UpdatePath());
        PathRequestManager.RequestPath(transform.position, Player.transform.position, OnPathFound);
        followPathCoroutine = null;
    }

    private void Update()
    {
        if (isPoisoned)
        {
            PoisonDamage(duration, timer);
        }

        currentNode = nodegrid.NodeFromWorldPoint(transform.position);
        endNode = nodegrid.NodeFromWorldPoint(Player.transform.position);
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.m_gameOver && isGrounded && !MenuMain.Instance.paused)
        {
            //EnemyMove();
        }
    }
    
    // Path Finding
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {   
        if (pathSuccessful)
        {
            this.waypoints = waypoints;
            targetIndex = waypoints.Length - 1;
            if (followPathCoroutine != null) StopCoroutine(followPathCoroutine);
            followPathCoroutine = FollowPath();
            StartCoroutine(followPathCoroutine);
        }
        return;
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, Player.transform.position, OnPathFound);
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold; ;
        Vector3 targetPosOld = Player.transform.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((Player.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                if (PathRequestManager.Instance.thread) PathThreadManager.RequestPathThread(new PathRequest(transform.position, Player.transform.position, OnPathFound));
                else PathRequestManager.RequestPath(transform.position, Player.transform.position, OnPathFound);
                targetPosOld = Player.transform.position;
            }
        }
    }


    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = waypoints[0];
        bool followingPath = true;
        targetIndex = 0;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= waypoints.Length) yield break;
                currentWaypoint = waypoints[targetIndex];
            }

            if (followingPath)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpd * Time.deltaTime);
            }
            yield return null;
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

    private void OnDrawGizmos()
    {
        for (int i = targetIndex; i<waypoints.Length-1; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(waypoints[i], Vector3.one);

            if (i == targetIndex) Gizmos.DrawLine(transform.position, waypoints[i]);
            else Gizmos.DrawLine(waypoints[i-1], waypoints[i]);
        }
    }

}
