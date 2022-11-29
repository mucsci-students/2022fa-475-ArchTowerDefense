using System.Collections.Generic;
using UnityEngine;

public class GwNode
{
    public int nodeID;
    public Vector3 position;
    public Dictionary<int, GwConnection> connections;

    // Pathfinding vars
    public float gCost;
    public float hCost;
    public int parentNodeID;

    public GwNode(int nodeID, Vector3 position)
    {
        this.nodeID = nodeID;
        this.position = position;

        connections = new Dictionary<int, GwConnection>();
    }

    /// <summary>
    /// Add a new connection from this node to another.
    /// </summary>
    /// <param name="connectedNodeID">The ID of the connected node.  Same as node GameObject name.</param>
    /// <param name="disabled">Connection is enabled/disabled by default.</param>
    /// <param name="speedWeight">Speed weight score of connection.</param>
    /// <param name="subnodes">Array of subnode positions, closest to furthest.</param>
    public void AddConnection(int connectedNodeID, bool disabled, float speedWeight, Vector3[] subnodes)
    {
        if (!connections.ContainsKey(connectedNodeID))
        {
            connections[connectedNodeID] = new GwConnection(connectedNodeID, disabled, speedWeight, subnodes);
        }
        else
        {
            UpdateConnection(connectedNodeID, disabled, speedWeight, subnodes);
        }
    }

    /// <summary>
    /// Update connection properties to connected node.
    /// </summary>
    /// <param name="connectedNodeID">The ID of the connected node.  Same as node GameObject name.</param>
    /// <param name="isDisabled">Connection is enabled/disabled by default.</param>
    /// <param name="speedWeight">Speed weight score of connection.</param>
    /// <param name="subnodes">Array of subnode positions, closest to furthest.</param>
    public void UpdateConnection(int connectedNodeID, bool isDisabled, float speedWeight, Vector3[] subnodes)
    {
        if (connections.ContainsKey(connectedNodeID))
        {
            connections[connectedNodeID].disabled = isDisabled;
            connections[connectedNodeID].speedWeight = speedWeight;
            connections[connectedNodeID].subnodes = subnodes;
        }
    }

    /// <summary>
    /// Disable a connection.
    /// </summary>
    /// <param name="connectedNodeID">The ID of the connected node.  Same as node GameObject name.</param>
    public void DisableConnection(int connectedNodeID)
    {
        if (connections.ContainsKey(connectedNodeID))
        {
            connections[connectedNodeID].disabled = true;
        }
    }

    /// <summary>
    /// Enable a connection.
    /// </summary>
    /// <param name="connectedNodeID">The ID of the connected node.  Same as node GameObject name.</param>
    public void EnableConnection (int connectedNodeID)
    {
        if (connections.ContainsKey(connectedNodeID))
        {
            connections[connectedNodeID].disabled = false;
        }
    }

    /// <summary>
    /// Update connection speed weight property.
    /// </summary>
    /// <param name="nodeToID">The ID of the connected node.  Same as node GameObject name.</param>
    /// <param name="speedWeight">New speed weight setting</param>
    public void SetConnectionSpeedWeight(int nodeToID, float speedWeight)
    {
        if (connections.ContainsKey(nodeToID))
        {
            connections[nodeToID].speedWeight = speedWeight;
        }
    }

    /// <summary>
    /// Removes a connection.
    /// </summary>
    /// <param name="connectedNodeID">The ID of the connected node.  Same as node GameObject name.</param>
    public void RemoveConnection(int connectedNodeID)
    {
        if (connections.ContainsKey(connectedNodeID))
        {
            connections.Remove(connectedNodeID);
        }
    }

    /// <summary>
    /// Resets pathfinding var state when finding a new path
    /// </summary>
    public void ResetPathfindingVars()
    {
        gCost = 0;
        hCost = 0;
        parentNodeID = -1;
    }

    /// <summary>
    /// Calculate A* FCost (g + h)
    /// </summary>
    /// <returns>FCost value</returns>
    public float FCost()
    {
        return gCost + hCost;
    }
}