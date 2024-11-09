using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{

    public Transform target;
    [SerializeField] private float speed = 20;
    [SerializeField] private float turnSpeed = 3;
    private Path path;
    private int targetIndex;
    public NodeGrid grid;
    public float turnDst = 5;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        grid = GameObject.Find("A*").GetComponent<NodeGrid>();
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        return;
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        while (true)
        {
            Vector3 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else pathIndex++;
            }

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
