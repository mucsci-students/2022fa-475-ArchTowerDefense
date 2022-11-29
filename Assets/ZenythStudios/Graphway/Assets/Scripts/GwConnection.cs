using UnityEngine;


public class GwConnection
{
    public int connectedNodeID;
    public bool disabled;
    public float speedWeight;
    public Vector3[] subnodes;

    public GwConnection(int connectedNodeID, bool disabled, float speedWeight, Vector3[] subnodes)
    {
        this.connectedNodeID = connectedNodeID;
        this.disabled = disabled;
        this.speedWeight = speedWeight;
        this.subnodes = subnodes;
    }
}
