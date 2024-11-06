using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{

    public Transform target;
    float speed = 20;
    Path path;
    int targetIndex;
    public NodeGrid grid;
    public float turnDst = 5;

    // Start is called before the first frame update
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        grid = GameObject.Find("A*").GetComponent<NodeGrid>();
    }

    // Update is called once per frame
    void Update()
    {

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

        while (true)
        {
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
