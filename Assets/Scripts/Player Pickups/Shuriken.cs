using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Vector3 playerPos;

    [SerializeField] private float rotationSpd;
    [SerializeField] private Vector3 parent;
    [SerializeField] private float pushForce;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.position - Vector3.up*30;
    }

    private void FixedUpdate()
    {
        parent = transform.parent.position - Vector3.up*30;
        transform.RotateAround(parent, Vector3.up, rotationSpd * Time.deltaTime);
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
