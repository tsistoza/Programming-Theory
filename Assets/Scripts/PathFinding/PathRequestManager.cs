using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PathRequestManager instance { get; private set; }
    private PathFinding pathFinding;

    private bool isProcessingPath;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(pathRequest);
        instance.TryProcessNext();
        return;
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
        return;
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

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
    
}
