using System;
using UnityEngine;

public class GwJob
{
    public Vector3 origin;
    public Vector3 targetPosition;
    public Action<GwWaypoint[]> callback;
    public bool clampToEndNode;
    public bool debugMode;

    public GwJob(Vector3 origin, Vector3 targetPosition, Action<GwWaypoint[]> callback, bool clampToEndNode, bool debugMode)
    {
        this.origin = origin;
        this.targetPosition = targetPosition;
        this.callback = callback;
        this.clampToEndNode = clampToEndNode;
        this.debugMode = debugMode;
    }
}