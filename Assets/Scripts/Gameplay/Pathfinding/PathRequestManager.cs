using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pathfinding
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static PathRequestManager instance;
        Pathfinder pathfinder;

        bool isCalculatingPath;

        public static PathRequestManager GetInstance() => instance;

        private void Awake()
        {
            pathfinder = GetComponent<Pathfinder>();

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
        }
        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryCalculateNext();
        }

        void TryCalculateNext()
        {
            if (!isCalculatingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isCalculatingPath = true;
                pathfinder.CalculatePath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedCalculatingPath(Vector3[] path, bool success)
        {
            if (path.Length == 0)
                success = false;
            currentPathRequest.callback(path, success);
            isCalculatingPath = false;
            TryCalculateNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callbackAction)
            {
                pathStart = start;
                pathEnd = end;
                callback = callbackAction;
            }
        }

    }
}

