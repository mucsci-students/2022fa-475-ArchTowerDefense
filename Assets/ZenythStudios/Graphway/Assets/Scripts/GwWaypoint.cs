using UnityEngine;

public class GwWaypoint
{
    public Vector3 position;
    public float speed;

    public GwWaypoint(Vector3 position, float speed = 1)
    {
        this.position = position;
        this.speed = speed;
    }
}