using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{

    public Transform target;
    float speed = 5;
    Vector3[] path;
    int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        return;
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length) yield break;
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
    }
}
