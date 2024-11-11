using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.XPath;
using UnityEngine;


public class PathThreadManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();

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

    private void Start()
    {
        if (!PathRequestManager.Instance.thread) Destroy(this); 
    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemInQueue = results.Count;
            lock (results)
            {
                for (int i=0; i<itemInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPathThread(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            Instance.pathFinding.FindPathThread(request, Instance.FinishedProcessingPath);
        };
        threadStart.Invoke();

        return;
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (Instance.results)
        {
            Instance.results.Enqueue(result);
        }
        return;
    }
}
public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

public struct PathRequest
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