using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(KillInSecs());
        }

        IEnumerator KillInSecs()
        {
            yield return new WaitForSeconds(10);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Killed");
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<Enemy>().DealDamage();
        }
        if (collision.gameObject.CompareTag("Walls")) gameObject.SetActive(false);
    }
}
