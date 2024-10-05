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

    // Components
    private Rigidbody playerRb;
    private List<GameObject> pooledObjects;
    public GameObject bulletPrefab;
     
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < numBullets; i++)
        {
            GameObject obj = (GameObject)Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform);
        }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = GetPooledObject();
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = gameObject.transform.position;
                bullet.GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
            }
        }
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) { return pooledObjects[i]; }
        }
        return null;
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
