using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Vector3 playerPos;

    [SerializeField] private float rotationSpd;
    [SerializeField] private Transform objectToRotateAround;
    [SerializeField] private float pushForce;
    [SerializeField] private float radius;

    // Start is called before the first frame update
    void Start()
    {
        objectToRotateAround = GameObject.Find("Orbiter").transform;
        transform.parent = GameObject.Find("Orbiter").transform;
    }

    private void FixedUpdate()
    {
        Vector3 dir = (transform.position - objectToRotateAround.position).normalized;
        transform.position = (objectToRotateAround.position) + (dir * radius);
        transform.RotateAround(objectToRotateAround.position, Vector3.up, rotationSpd * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 vec = gameObject.transform.position - transform.parent.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(vec * pushForce, ForceMode.Impulse);
            collision.gameObject.GetComponent<Enemy>().DealDamage();
        }
    }
}