using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    [SerializeField] private float moveSpd;
    [SerializeField] private Transform playerPos;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerPos.position, Time.deltaTime * moveSpd);
    }
}
