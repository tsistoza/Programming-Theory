using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _pathEnd;
        callback = _callback;
    }
}

public class PathThreadManager : MonoBehaviour
{ 
    public static PathThreadManager Instance { get; private set; }
    private PathFinding pathFinding;

    private bool isProcessingPath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    /*public static void RequestPathThread(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            Instance.pathFinding.StartFindPath(request, FinishedProcessingPath)
        };

        return;
    }*/

    /*public void FinishedProcessingPath(Vector3[] path, bool success, PathRequest originalRequest)
    {
        return;
    }*/
}
