using System.Collections;
using System.Collections.Generic;
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
}
